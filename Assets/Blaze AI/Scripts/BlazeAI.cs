using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BlazeAISpace;
using UnityEditor;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(NavMeshObstacle))]

public class BlazeAI : MonoBehaviour {
    
    [Header("PATHING")]
    [Tooltip("The navmesh layers that are walkable by the NPC.")]
    public LayerMask groundLayers;
    
    [Tooltip("Path recalculation every seconds. The lower the number the more frequent the recalculation giving better quality of obstacle avoidance but more CPU intensive. It all depends on your game and target hardware.")]
    [Range(0.3f, 5f)]
    public float pathRecalculationRate = 0.3f;

    [Tooltip("The gameobject that will do the pathfinding and navigation. This object must be outside the boundries of the navmesh obstacle.")]
    public Transform pathFindingProxy;

    [Min(0f), Tooltip("The PathFindingProxy is the object responsible for pathfinding. Set the offset position to be in front and outside the boundries of the navmesh obstacle. The same offset will be used in negative value (behind) when moving backwards so take care of this. Use the edit screen to debug and see this as a green sphere.")]
    public float proxyOffset = 0.8f;

    [Tooltip("Have a gravity effect on the NPC to keep the character controller grounded.")]
    public bool enableGravity = true;

    [Tooltip("Enabling this will make the npc use root motion, this gives more accurate and realistic movement related to the animation, the movement speed will be based on the animation speed.")]
    public bool useRootMotion = false;

    [Header("OBSTACLES")]
    [Tooltip("Setting this to true will prevent the NPC facing too close to an obstacle at a specified distance from the end point. Good for realism, most of the times you don't want your NPC to stand facing an obstacle. The ray is shown in the editor view during game play.")]
    public bool avoidFacingObstacles = true;

    [Tooltip("The distance of the raycast to avoid facing an obstacles. When the ray hits an obstacle at the way point. It'll idle the NPC.")]
    public float obstacleRayDistance = 3f;

    [Tooltip("Position the ray with an offset from the center that will detect the obstacles in front.")]
    public Vector3 obstacleRayOffset = Vector3.zero;

    [Tooltip("The layers of obstacles you want to avoid facing too closely.")]
    public LayerMask obstacleLayers;

    [Header("AVOIDANCE")]
    [Tooltip("When two BlazeAI npc touch one another one will wait to give room while the other moves by so here set the tag(s) of the npc with the BlazeAI script.")]
    public List<string> tagsToAvoid = new List<string>();

    [Header("PATROL ROUTES")]
    public BlazeAISpace.Waypoints waypoints;

    [Header("CONE OF SIGHT AND RANGE")]
    public BlazeAISpace.Vision vision;

    [Header("NORMAL STATE")]
    [Tooltip("The NPC has not seen any of the hostile tags in the Vision class.")]
    public BlazeAISpace.NormalState normalState;

    [Header("ALERT STATE")]
    [Tooltip("The NPC has seen any of the alert tags or hostile tags in the Vision class but no longer thus becoming alert.")]
    public BlazeAISpace.AlertState alertState;

    [Header("ATTACK STATE")]
    [Tooltip("Attack state is when this NPC finds an enemy (hostile tag) within it's cone of vision.")]
    public BlazeAISpace.AttackState attackState;

    [Header("DISTRACTIONS & TURNING")]
    public BlazeAISpace.Distractions distractions;
    
    [Header("HITS & DAMAGE")]
    public BlazeAISpace.Hits hits;

    [Header("DEATH")]
    public BlazeAISpace.Death death;

    [Header("ADD PROFILE"), Tooltip("Add a Blaze profile with pre-set properties and options. To create a Blaze profile, right click in project window -> Create -> Blaze AI -> Blaze Profile.")]
    public BlazeProfile blazeProfile;
    [Tooltip("Keep this inspector synced with the profile. Changes can be done from the profile itself not the inspector.")]
    public bool profileSync = true;
    
    public BlazeProfile lastProfile { get; set; }


    #region System Variables

    public enum State 
    {
        normal,
        alert,
        attack,
        hit
    }

    public State state { get; set; }
    public int waypointIndex { get; set; }
    public bool reachedEnd { get; set; }
    public bool distracted { get; set; }
    public GameObject enemyToAttack { get; set; }
    public Vector3 checkEnemyPosition { get; set; }
    public bool enemyInSight { get; set; }
    public float captureEnemyTimeStamp { get; set; }
    public bool alertedByOther { get; set; }
    public bool attackBackUp { get; set; }
    public GameObject enemyScheduled { get; set; }
    public float stopPriority { get; set; }
    public bool stop { get; set; }
    public bool crowdedAttack { get; set; }
    public Vector3 lastCheckedEnemyPosition { get; set; }
    public bool idleAttack { get; set; }
    public Vector3 reachedEndEnemyPosition { get; set; }
    public bool isAttacking { get; set; }
    public bool startCoverTimer { get; set; }
    public bool takeCoverDelay { get; set; }
    public GameObject coverObject { get; set; }
    public BlazeAI backedUpBy { get; set; }
    
    BlazeAISpace.AnimationManager animationManager;
    NavMeshObstacle agentObstacle;
    NavMeshAgent navmeshAgent;
    CharacterController controller;
    Animator anim;
    NavMeshPath path;
    CapsuleCollider capsuleCollider;
    NavMeshHit hit;
    NavMeshHit hit2;
    Transform visionT;
    
    Queue<Vector3> cornerQueue = new Queue<Vector3>();
    Vector3 endPoint;
    bool normalStateActive;
    bool alertStateActive;
    bool attackStateActive;
    bool hasPath = false;
    Vector3 currentDestination = Vector3.zero;
    float pathFramesElapsed;
    bool wpIdleTriggered = false;
    bool wpRandomMode = false;
    bool activateRay = false;
    Transform distractionLocation;
    Vector3 distractionPosition;
    bool distractionTurn = false;
    bool passDistractionCheck;
    bool goingToDistractionPoint = false;
    bool waypointInterrupted;
    int visionFramesElapsed = 0;
    int alertFramesElapsed = 0;
    int FRAMES_THRESHOLD = 4;
    bool waitFrameRan = false;
    Vector3 enemyPosition = Vector3.zero;
    Vector3 debugPlayerDir = Vector3.zero;
    Vector3 debugTargetDir = Vector3.zero;
    bool targetingEnemy = false;
    float returnNormalTimer = 0f; 
    float alertOthersInAttackTimer = 0f;
    bool isSeenVisionAlertTags;
    string seenVisionAlertTag;
    bool attackPreparationsLaunched = false;
    bool shouldAttack = false;
    float attackTimer = 0f;
    bool startAttackTimer;
    bool isHit = false;
    float stopTimer;
    int detectOthersFrames = 0;
    float controllerRadius;
    Vector3 controllerPosition;
    bool startIntervalTimer;
    float intervalTimer = 0f;
    Vector3 backupPoint;
    int backupPass = 0;
    bool backIsDeadEnd = false;
    float defaultDistanceFromEnemy;
    float defaultMoveBackwardsDist;
    bool isAgent;
    bool agentEnabled = false;
    float proxyDistance;
    float emptyEnemyTimer = 0f;
    bool slowMove = false;
    float slowMoveTimer = 0f;
    Vector3 rootMotionDirection;
    float rootMotionSpeed;
    bool reachedEndBeforeDistraction;
    bool forceTurn;
    float sphereDetectionFrame;
    float stateWalkTimer = 0f;
    bool stateWalkTimerRun = false;
    bool goingToVisionAlertTag;
    bool startWaypointRotation;
    float waypointRotationTimer = 0f;
    float waypointRotationAnimationTimer = 0f;
    int waypointTurnDir;
    Collider[] hideColliders = new Collider[10];
    int hideFrames = 0;
    float coverTimer = 0f;
    float actualCoverTime = 0f;
    bool coverLocationSet = false;
    bool findCoverFired = false;
    GameObject targetedEnemyInCover;
    float coverHeight;
    float takeCoverDelayTimer = 0f;
    Vector3 coverNormal;
    bool coverBlockingAttack = false;
    bool coveringAway = false;
    float attackDelayTimer = 0f;
    bool tookCover = false;
    bool sawOnce = false;
    GameObject proxyCopy;
    GameObject enemyCover;
    bool getEnemyCover = false;
    
    #endregion
    
    #region Engine Methods

    //Start is called before the first frame update
    void Start()
    {
        agentObstacle = GetComponent<NavMeshObstacle>();
        navmeshAgent = GetComponent<NavMeshAgent>();
        controller = GetComponent<CharacterController>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        anim = GetComponent<Animator>();
        animationManager = new AnimationManager(anim);
        path = new NavMeshPath();

        enableAgentCoroutine = EnableAgent();
        disableAgentCoroutine = DisableAgent();

        AdjustComponentsStatesOnStart();
        SetAgentObstacle();
        vision.buildDictionaryOfAlert();

        proxyDistance = (new Vector3(pathFindingProxy.position.x, transform.position.y, pathFindingProxy.position.z) - transform.position).sqrMagnitude;
        waypointIndex = 0;
        stopPriority = Random.Range(0f, 100f);

        DisableAllSystemScripts();

        //normal state if to be used on game start
        if (normalState.useNormalStateOnStart) {
            state = State.normal;
            if (waypoints.instantMoveAtStart) normalState.InstantMoveChangeVals();
            
            //setting the waypoints loop to true forces the npc to move
            //if waypoints length is only 1
            bool temp = waypoints.loop;
            if (waypoints.waypoints.Length == 1) {
                waypoints.loop = true;
            }

            StartCoroutine(NormalStateIdle());
            waypoints.loop = temp;
        }

        //alert state if to be used on game start
        if (alertState.useAlertStateOnStart) {
            state = State.alert;
            if (waypoints.instantMoveAtStart) alertState.InstantMoveChangeVals();
            
            bool temp = waypoints.loop;
            if (waypoints.waypoints.Length == 1) {
                waypoints.loop = true;
            }
            
            StartCoroutine(AlertStateIdle());
            waypoints.loop = temp;
        }

        defaultDistanceFromEnemy = attackState.distanceFromEnemy;
        defaultMoveBackwardsDist = attackState.moveBackwardsDist;
        
        attackState.RandomizeAttackIntervals();
        attackState.Validate();
        ValidateProperties();
    }

    //Update is called once per frame
    void Update()
    {
        if (vision.head) visionT = vision.head;
        else visionT = transform;

        //check for functionalities
        FlaggedFunctions();

        //trigger vision checking
        VisionCheck();

        //states and movement
        if (state == State.attack) AttackStateMovementTrigger();
        if (state == State.normal) NormalStateMovementTrigger();
        if (state == State.alert) AlertStateMovementTrigger();
        
        AgentSpeeds();
        MainToClassesUpdate();

        isAttacking = startAttackTimer;
    }

    //Fixed Update for physics
    void FixedUpdate()
    {
        //always de-activate obstacle ray when in attack state
        if (state == State.attack) {
            activateRay = false;
        }

        //avoid facing an obstacle too closely when patrolling way points
        if(avoidFacingObstacles && activateRay && !reachedEnd){
            RaycastHit hit;
            if (Physics.Raycast(transform.position + obstacleRayOffset, transform.TransformDirection(Vector3.forward), out hit, obstacleRayDistance, obstacleLayers)) {
                activateRay = false;
                StopAgent();
            }

            //for debugging (seeing) the obstacle prevention ray - can delete
            Debug.DrawRay(transform.position + obstacleRayOffset, transform.TransformDirection(Vector3.forward) * obstacleRayDistance, Color.yellow);
        }
    }

    //validate GUI properties
    void OnValidate()
    {
        if (state == State.attack) return;

        if (path != null) {
            waypointInterrupted = true;
            StateWalk();
        }
        
        if (waypoints != null) waypoints.WaypointsSystemValidation();
        StatesInspectorValidation();
        
        if (attackState != null) {
            attackState.Validate();
            if (attackState.coverShooterOptions.coverShooter) vision.visionDuringAttackState.sightRange = attackState.distanceFromEnemy + attackState.coverShooterOptions.searchDistance;
        }

        if (pathFindingProxy) pathFindingProxy.transform.localPosition = new Vector3(0f, 0f, proxyOffset);

        if (useRootMotion) GetComponent<Animator>().updateMode = AnimatorUpdateMode.AnimatePhysics;
        else GetComponent<Animator>().updateMode = AnimatorUpdateMode.Normal;

        if (vision != null) {
            if (vision.maxSightLevel < 0f) vision.maxSightLevel = 0f;
            if (vision.sightLevel < 0f) vision.sightLevel = 0f;
        }

        #if UNITY_EDITOR
        foreach (var tag in tagsToAvoid) {
            if (System.Array.IndexOf(vision.hostileTags, tag) >= 0) {
                EditorUtility.DisplayDialog("Error",$"The tag name = {tag} is set in both [Tags To Avoid] and [Hostile Tags] in vision. You can't have the same tag in both.", "OK");
            }
        }
        #endif

        if (attackState.coverShooterOptions.coverShooter) attackState.alwaysLookAtEnemy = true;
        if (waypoints.waypoints.Length > 0) {
            for (int i=0; i<waypoints.waypoints.Length; i++) {
                if (waypoints.waypoints[i] == Vector3.zero) waypoints.waypoints[i] = transform.position;
            }
        }

        if (lastProfile != blazeProfile) LoadProfile(blazeProfile);
    }

    //use Gizmos to help debugging in editor view
    void OnDrawGizmosSelected()
    {
        if (vision.head) visionT = vision.head;
        else visionT = transform;

        //show the vision spheres
        vision.ShowVisionSpheres(visionT);

        //debug the ray cast
        if (!Application.isPlaying && avoidFacingObstacles) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position + obstacleRayOffset, transform.position + obstacleRayOffset + transform.TransformDirection(Vector3.forward) * obstacleRayDistance);
        }

        //show the waypoints
        waypoints.ShowWayPoints();

        if (pathFindingProxy != null) {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(pathFindingProxy.transform.position, 0.2f);
        }

        if (GetComponent<CharacterController>() != null) {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position + new Vector3(0f, GetComponent<CharacterController>().height/2f, 0f), alertState.alertRadius);
        }

        attackState.CoverShooterGizmos(transform);
    }

    //animator motion
    void OnAnimatorMove()
    {
        if (useRootMotion) {
            rootMotionDirection = anim.deltaPosition / Time.deltaTime;
            rootMotionSpeed = rootMotionDirection.magnitude;

            //set agent velocity to root motion direction
            navmeshAgent.velocity = rootMotionDirection;
            if (!navmeshAgent.enabled && !shouldAttack) transform.position = new Vector3(anim.rootPosition.x, transform.position.y, anim.rootPosition.z);
        }
    }

    #endregion
    
    #region Normal State
    
    //waypoint reached and now should go idle
    //enabling/disabling scripts/animations and waypoints
    IEnumerator NormalStateIdle(bool overrideStop = false)
    {
        if (!overrideStop) StartCoroutine(disableAgentCoroutine);

        //if no more waypoints, and not in a loop, then flag to stand still
        if (!overrideStop && !waypoints.loop && (waypointIndex + 1) == waypoints.waypoints.Length) overrideStop = true;
        
        //if checking distraction point, and property is checked, 
        //trigger special animation when reached else play normal animations
        if (goingToDistractionPoint) {
            if (!overrideStop) {
                //if went to distraction location and no check distraction animation
                //although set to true then use the idle animation of waypoint
                if (distractions.distractionCheckAnimation) {
                    if (distractions.distractionCheckAnimationName.Length > 0)
                        animationManager.PlayAnimationState(distractions.distractionCheckAnimationName, distractions.distractionCheckTransition);
                    else
                        animationManager.PlayAnimationState(normalState.idleAnimationName, normalState.idleAnimationTransition);
                }else{
                    animationManager.PlayAnimationState(normalState.idleAnimationName, normalState.idleAnimationTransition, normalState.useAnimations);
                }
            }else{
                animationManager.PlayAnimationState(normalState.idleAnimationName, normalState.idleAnimationTransition, normalState.useAnimations);
            }
        }else{
            //trigger random idle animation if set to do so
            if (normalState.useRandomAnimationsOnIdle && (normalState.randomIdleAnimationNames.Length > 0) && !distracted && !overrideStop && !normalState.instantMoveChange) {
                
                //generate a chance
                int num = Random.Range(0, 10);

                //play random animation if random number is bigger than 5
                if(num >= 5){
                    string randomAnimation = normalState.randomIdleAnimationNames[Random.Range(0, normalState.randomIdleAnimationNames.Length)];
                    animationManager.PlayAnimationState(randomAnimation, normalState.randomIdleAnimationTransition);
                }else{
                    animationManager.PlayAnimationState(normalState.idleAnimationName, normalState.idleAnimationTransition, normalState.useAnimations);
                }

            }else{
                //trigger idle animation
                animationManager.PlayAnimationState(normalState.idleAnimationName, normalState.idleAnimationTransition, normalState.useAnimations);
            }
        }

        alertState.DisableScripts();
        normalState.TriggerScripts("idle");

        state = State.normal;
        wpIdleTriggered = true;

        slowMove = false;
        slowMoveTimer = 0f;

        startWaypointRotation = false;
        waypointRotationTimer = 0f;
        waypointRotationAnimationTimer = 0f;
        
        //if method parameter true (sudden stop for distraction) break out of the coroutine
        if (overrideStop) {
            waypointInterrupted = true;
            yield break;
        }

        //get the wait time for later use depending on checked properties
        float waitTime;
        
        if (normalState.randomizeWaitTime) {
            waitTime = Random.Range(normalState.randomizeWaitTimeBetween.x, normalState.randomizeWaitTimeBetween.y); 
        } else {
            waitTime = normalState.waitTime;
        }

        if (goingToDistractionPoint) waitTime = distractions.checkingTime;
        yield return new WaitForSeconds(waitTime);
        
        FromIdleToWalk();
    }

    //triggers the movement and idle of the normal state
    void NormalStateMovementTrigger()
    {
        if (forceTurn) return;

        alertOthersInAttackTimer = 0f;
        attackState.distanceFromEnemy = defaultDistanceFromEnemy;

        //state movement trigger
        if (normalStateActive && !distracted) {
            if (reachedEnd) {
                if (!wpIdleTriggered) {
                    if (WaypointRotationCheck()) {
                        TriggerWaypointRotation();
                    }else{
                        StopAllCoroutines();
                        StartCoroutine(NormalStateIdle());
                    }
                }
            }else{
                animationManager.PlayAnimationState(normalState.moveAnimationName, normalState.moveAnimationTransition, normalState.useAnimations);  
                wpIdleTriggered = false;
                MoveToDestination(endPoint);
            }
        }

        if (normalState.playAudiosOnPatrol) 
        {
            normalState.audioPlayTimer += Time.deltaTime;
            if(normalState.audioPlayTimer >= normalState.playAudioEvery && !distracted){        
                StopSystemsAudios("normal");
                normalState.PlayRandomPatrolAudio();
            }
        }
    }

    #endregion

    #region Alert State
    
    //alert state idle
    public IEnumerator AlertStateIdle(bool overrideStop = false)
    {
        if (!overrideStop) StartCoroutine(disableAgentCoroutine);

        //if no more waypoints, and not in a loop, then flag to stand still
        if (!overrideStop && !waypoints.loop && (waypointIndex + 1) == waypoints.waypoints.Length) overrideStop = true;
        
        //if checking distraction point, and property is checked, 
        //trigger special animation when reached else play normal animations
        if (goingToDistractionPoint) {
            
            if (!overrideStop) {
                //if went to distraction location and no check distraction animation
                //although set to true then use the idle animation of waypoint
                if (distractions.distractionCheckAnimation) {

                    if (distractions.distractionCheckAnimationName.Length > 0)
                        animationManager.PlayAnimationState(distractions.distractionCheckAnimationName, distractions.distractionCheckTransition, distractions.distractionCheckAnimation);
                    else
                        animationManager.PlayAnimationState(alertState.idleAnimationName, alertState.idleAnimationTransition, alertState.useAnimations);
                
                }else{
                    animationManager.PlayAnimationState(alertState.idleAnimationName, alertState.idleAnimationTransition, alertState.useAnimations);
                }

            }else{
                animationManager.PlayAnimationState(alertState.idleAnimationName, alertState.idleAnimationTransition, alertState.useAnimations);
            }

        }else{
            //trigger random idle animation if set to do so
            if (alertState.useRandomAnimationsOnIdle && (alertState.randomIdleAnimationNames.Length > 0) && !isSeenVisionAlertTags && !overrideStop && !alertState.instantMoveChange) {
                
                //generate a chance
                int num = Random.Range(0, 10);
                
                //play random animation if random number is bigger than 5
                if(num >= 5){
                    string randomAnimation = alertState.randomIdleAnimationNames[Random.Range(0, alertState.randomIdleAnimationNames.Length)];
                    animationManager.PlayAnimationState(randomAnimation, alertState.randomIdleAnimationTransition);
                }else{
                    animationManager.PlayAnimationState(alertState.idleAnimationName, alertState.idleAnimationTransition, alertState.useAnimations);
                }

            }else{
                //trigger idle animation
                if (!isSeenVisionAlertTags) animationManager.PlayAnimationState(alertState.idleAnimationName, alertState.idleAnimationTransition, alertState.useAnimations);
            }

            //if got alert by vision alert tags and need to play an animation at the location
            if (isSeenVisionAlertTags) {
                if (vision.alertTagsDict[seenVisionAlertTag].animationName.Length > 0)
                    animationManager.PlayAnimationState(vision.alertTagsDict[seenVisionAlertTag].animationName, alertState.moveAnimationTransition);
                else
                    animationManager.PlayAnimationState(alertState.idleAnimationName, alertState.idleAnimationTransition, alertState.useAnimations);
            }
        }

        normalState.DisableScripts();
        alertState.TriggerScripts("idle");
        
        state = State.alert;
        wpIdleTriggered = true;
        
        slowMove = false;
        slowMoveTimer = 0f;

        startWaypointRotation = false;
        waypointRotationTimer = 0f;
        waypointRotationAnimationTimer = 0f;
        
        //if method parameter true (sudden stop for distraction) break out of the coroutine
        if (overrideStop) {
            waypointInterrupted = true;
            yield break;
        }

        //get the wait time for later use depending on checked properties
        float waitTime;
        if (alertState.randomizeWaitTime) waitTime = Random.Range(alertState.randomizeWaitTimeBetween.x, alertState.randomizeWaitTimeBetween.y); 
        else waitTime = alertState.waitTime;

        //override previous wait times if seen vision alert tags
        if (isSeenVisionAlertTags) waitTime = vision.alertTagsDict[seenVisionAlertTag].time;
        if (goingToDistractionPoint) waitTime = distractions.checkingTime;

        yield return new WaitForSeconds(waitTime);

        if (checkEnemyPosition != Vector3.zero) {
            TurnToAttack();
            yield break;
        }

        FromIdleToWalk();
    }

    //triggers the movement and idle of the alert state
    void AlertStateMovementTrigger()
    {
        if (forceTurn) return;

        alertOthersInAttackTimer = 0f;
        attackState.distanceFromEnemy = defaultDistanceFromEnemy;

        //constantly alert surrounding enemies if alert
        if (alertStateActive) AlertSurrounding(false);

        //movement
        if (alertStateActive && !distracted) {
            if (reachedEnd) {
                if (!wpIdleTriggered) {
                    if (WaypointRotationCheck()) {
                        TriggerWaypointRotation();
                    }else{
                        StopAllCoroutines();
                        StartCoroutine(AlertStateIdle());
                    }
                }
            }else{
                animationManager.PlayAnimationState(alertState.moveAnimationName, alertState.moveAnimationTransition, alertState.useAnimations);
                wpIdleTriggered = false;
                MoveToDestination(endPoint);
            }
        }

        //patrol audio
        if (alertState.playAudiosOnPatrol) {
            alertState.audioPlayTimer += Time.deltaTime;
            if (alertState.audioPlayTimer >= alertState.playAudioEvery && !distracted) {
                StopSystemsAudios("alert");
                alertState.PlayRandomPatrolAudio();
            }
        }
    }

    //alert other npc to attack situation
    void AlertSurrounding(bool attackSituation = true)
    {
        //if not set to alert others, quit
        if (!alertState.alertOthers) return;
        
        if (alertFramesElapsed == FRAMES_THRESHOLD) {
            Collider[] collisions = Physics.OverlapSphere(transform.position + new Vector3(0f, controller.height / 2f, 0f), alertState.alertRadius);

            foreach (var npc in collisions)
            {
                if (System.Array.IndexOf(alertState.tagsToAlert, npc.tag) >= 0 && (npc.gameObject != gameObject)) {
                    BlazeAI script = npc.GetComponent<BlazeAI>();
                    if (script != null)
                    {
                        if (!script.alertState.receiveAlertFromOthers) return;
                        
                        //flag that the alert is an attack state alert
                        if (attackSituation)
                        {
                            if ((enemyToAttack != null && !script.enemyInSight && (script.captureEnemyTimeStamp < captureEnemyTimeStamp)) || checkEnemyPosition != Vector3.zero) {
                                if (enemyToAttack) {
                                    script.checkEnemyPosition = ValidateEnemyYPoint(enemyToAttack.transform.position);
                                    script.attackState.surprised.isSurprised = true;
                                    if (script.state != State.attack) script.TurnToAttack(true);
                                }else{
                                    if (checkEnemyPosition != Vector3.zero && script.lastCheckedEnemyPosition != checkEnemyPosition && checkEnemyPosition != script.reachedEndEnemyPosition && captureEnemyTimeStamp > script.captureEnemyTimeStamp) {
                                        script.checkEnemyPosition = checkEnemyPosition;
                                        script.attackState.surprised.isSurprised = true;
                                        if (script.state != State.attack) script.TurnToAttack(true);
                                    }
                                }
                            }
                        }else{
                            //make sure other npc don't turn those who have enemy in view to alert state
                            if (!script.enemyInSight || script.state != State.attack) {
                                
                                if (!isSeenVisionAlertTags) {
                                    if (script.state == State.hit || script.wpIdleTriggered || script.checkEnemyPosition == Vector3.zero) return;
                                }

                                if (isSeenVisionAlertTags && checkEnemyPosition != Vector3.zero) {
                                    if (vision.alertTagsDict[seenVisionAlertTag].callOthersToLocation) {
                                        if (script.reachedEndEnemyPosition != checkEnemyPosition && script.checkEnemyPosition != checkEnemyPosition) {
                                            script.checkEnemyPosition = checkEnemyPosition;
                                            script.ChangeState("alert", true);
                                        }
                                    }else{
                                        script.ChangeState("alert");
                                    }
                                }   
                            }
                        }
                    }
                }
            }
        }else{
            alertFramesElapsed++;
        }
    }
    
    //count down timer to get back to normal state from alert state
    void ReturnNormalTimer()
    {
        if (state == State.alert) {
            if (alertState.returnToNormalState)
            {
                if (enemyToAttack && checkEnemyPosition != Vector3.zero) returnNormalTimer = 0f;
                if (checkEnemyPosition == Vector3.zero) returnNormalTimer += Time.deltaTime;
                if (returnNormalTimer >= alertState.timeBeforeReturningNormal) {
                    
                    state = State.normal;

                    attackStateActive = false;
                    alertStateActive = false;
                    normalStateActive = false;

                    StopAllCoroutines();
                    
                    returnNormalTimer = 0f;
                    StartCoroutine(PrepareReturn());
                }
            }
        }else{
            returnNormalTimer = 0f;
        }
    }

    //preparing to return to normal state from alert state
    IEnumerator PrepareReturn()
    {
        //play audio
        if (alertState.playAudioOnReturn) {
            alertState.ChooseRandomAudioOnReturn();
        }

        //check if to use animations
        if (alertState.useAnimationOnReturn) {
            animationManager.PlayAnimationState(alertState.animationNameOnReturn, alertState.animationOnReturnTransition);
        }else{
            animationManager.PlayAnimationState(normalState.idleAnimationName, normalState.idleAnimationTransition, normalState.useAnimations);
        }

        yield return new WaitForSeconds(alertState.returningDuration);

        BackToNormal();
    }

    //trigger going back to normal
    void BackToNormal()
    {
        state = State.normal;
                    
        alertStateActive = false;
        normalStateActive = true;
        
        returnNormalTimer = 0f;
        reachedEnd = true;

        float temp = normalState.waitTime;
        normalState.waitTime = 1f;

        StartCoroutine(NormalStateIdle());
        normalState.waitTime = temp;
    }
    
    #endregion

    #region Attack State
    
    //trigger attack state movement functions
    void AttackStateMovementTrigger()
    {
        //attack state movement
        if (attackStateActive) {
            if (attackState.coverShooterOptions.coverShooter) {
                if (enemyToAttack) {
                    coveringAway = false;

                    if (coverLocationSet) {
                        shouldAttack = false;
                        if (CoverOccupied(coverObject)) coverLocationSet = false;
                        else MoveToCover();
                    }else{
                        if (shouldAttack) {
                            CarveAccordingToEnemy();
                            RaycastHit hit;
                            Vector3 dir = enemyToAttack.transform.position - transform.position;
                            
                            //check if cover blocking the attack
                            var layersToHit = ~transform.gameObject.layer | ~attackState.layersToIgnore | attackState.coverShooterOptions.coverLayers;
                            if (Physics.Raycast(transform.position, dir, out hit, Mathf.Infinity, layersToHit)) {
                                
                                if ((System.Array.IndexOf(vision.hostileTags, hit.transform.tag) >= 0) || (attackState.coverShooterOptions.coverShooter && getEnemyCover && enemyCover != null && hit.transform.gameObject != coverObject)) {
                                    attackDelayTimer += Time.deltaTime;
                                   
                                    if (tookCover) {
                                        if (attackDelayTimer >= 0.1f) {
                                            coverBlockingAttack = false;
                                            MeleeAndRangedAttackMovement();
                                            sawOnce = true;
                                        }else{
                                            animationManager.PlayAnimationState(attackState.moveForwardAnimationName, attackState.moveForwardAnimationTransition, attackState.useAnimations);
                                            MoveToDestination(enemyToAttack.transform.position);
                                            coverBlockingAttack = true;
                                            idleAttack = false;
                                            reachedEnd = false;
                                        }
                                    }else{
                                        coverBlockingAttack = false;
                                        MeleeAndRangedAttackMovement();
                                        sawOnce = true;
                                    }
                                    
                                }else{
                                    if (sawOnce) return;
                                    animationManager.PlayAnimationState(attackState.moveForwardAnimationName, attackState.moveForwardAnimationTransition, attackState.useAnimations);
                                    attackDelayTimer = 0.05f;

                                    coverBlockingAttack = true;
                                    idleAttack = false;
                                    reachedEnd = false;

                                    MoveToDestination(enemyToAttack.transform.position);
                                }
                            }

                        }else{
                            if (startIntervalTimer) {
                                idleAttack = false;
                                FindCover(true);
                                if (CoverOccupied(coverObject)) MeleeAndRangedAttackMovement();
                            }
                            else {
                                idleAttack = false;
                                reachedEnd = false;
                                
                                FindCover(true);
                                if (CoverOccupied(coverObject)) MeleeAndRangedAttackMovement();
                            }

                            attackDelayTimer = 0f;
                        }
                    }
                }else{
                    if (coverLocationSet) {
                        if (CoverOccupied(coverObject)) FindCover(true, coverObject);
                        else MoveToCover();
                        coveringAway = true;
                    }else{
                        if (!startCoverTimer) MeleeAndRangedAttackMovement();
                    }
                }
            }else{
                MeleeAndRangedAttackMovement();
            }
        }
        
        //alert others when in attack state, after certain set of time
        //to give the player some roon in a couple of frames
        alertOthersInAttackTimer += Time.deltaTime;
        if (alertOthersInAttackTimer >= attackState.alertOthersTime) {
            AlertSurrounding();
        }
    }
    
    //melee and ranged attack movement
    void MeleeAndRangedAttackMovement()
    {   
        coverLocationSet = false;
        findCoverFired = false;
        tookCover = false;

        //vision detected an enemy
        if (enemyToAttack) {
            
            //if targeted enemy no longer has the hostile tag
            if (System.Array.IndexOf(vision.hostileTags, enemyToAttack.transform.tag) < 0) {
                wpIdleTriggered = false;
                ResetAttackingTimer();
                RemoveFromEnemyScehduler();
                FromAttackStateReturnAlert();
                checkEnemyPosition = Vector3.zero;
                lastCheckedEnemyPosition = endPoint;
                return;
            }
            
            float distance = Vector3.Distance(new Vector3(enemyToAttack.transform.position.x, transform.position.y, enemyToAttack.transform.position.z), transform.position);
            float minDistance;

            lastCheckedEnemyPosition = endPoint;

            //change the minimum distance if set to attack
            if (shouldAttack) {
                minDistance = attackState.attackDistance;
                attackState.distanceFromEnemy = defaultDistanceFromEnemy;
                attackState.moveBackwardsDist = defaultMoveBackwardsDist;
            }else{
                if (attackBackUp) minDistance = attackState.moveBackwardsDist;
                else minDistance = attackState.distanceFromEnemy;
            }
            
            //made a validation on the editor to not allow attack distance being bigger than distance from enemy
            if (distance <= attackState.moveBackwardsDist && !shouldAttack) {
                rotateToTarget(enemyToAttack.transform.position, 5f);
                BackupMovement();
            }else{
                if ((distance) <= attackState.moveBackwardsDist && !shouldAttack) {
                    rotateToTarget(enemyToAttack.transform.position, 5f);
                    
                    //back up from enemy
                    if (!shouldAttack && attackState.moveBackwards) {
                        if (shouldAttack && !attackState.moveBackwardsAttack) return;
                        BackupMovement();
                    }

                }else{
                    //distance difference from enemy
                    float distDiff;
                    distDiff = (distance - minDistance);
                    
                    if (distDiff <= 0.3f) {
                        if (attackState.coverShooterOptions.coverShooter && getEnemyCover && enemyCover != null) {
                            
                            RaycastHit hit;
                            Vector3 dir = enemyToAttack.transform.position - transform.position;
                            if (Physics.Raycast(transform.position, dir, out hit, Mathf.Infinity)) {
                                if (hit.transform.gameObject == enemyCover || hit.transform.gameObject == enemyToAttack) {
                                    AttackPosition();
                                }
                            }

                        }else{
                            AttackPosition();
                        }
                        
                    }else{
                        if (startAttackTimer) return;
                        if (idleAttack && !shouldAttack && (distance - minDistance) <= 0.5f) return;
                    
                        attackBackUp = false;
                        targetingEnemy = true;    
                        wpIdleTriggered = false;
                        
                        startIntervalTimer = false;
                        idleAttack = false;
                        reachedEnd = false;
                        backedUpBy = null;

                        attackState.distanceFromEnemy = defaultDistanceFromEnemy;
                        RemoveFromEnemyScehduler();

                        if (attackState.useAnimations) {
                            animationManager.PlayAnimationState(attackState.moveForwardAnimationName, attackState.moveForwardAnimationTransition);
                        }

                        StartCoroutine(WaitFrameSetup());
                        MoveToDestination(enemyToAttack.transform.position);
                    }
                }
            }

        }else{
            //if enemy position has been told by another NPC
            if (checkEnemyPosition != Vector3.zero) {
                
                attackBackUp = false;
                idleAttack = false;
                targetingEnemy = false;

                lastCheckedEnemyPosition = checkEnemyPosition;
                attackState.distanceFromEnemy = 2f;
                
                RemoveFromEnemyScehduler();
                ResetAttackingTimer();

                float distance = (new Vector3(checkEnemyPosition.x, transform.position.y, checkEnemyPosition.z) - transform.position).sqrMagnitude;
                float minDistance = attackState.distanceFromEnemy;
                
                if (reachedEnd) {
                    state = State.alert;
                    attackStateActive = false;

                    attackBackUp = false;
                    waitFrameRan = false;
                    
                    reachedEndEnemyPosition = checkEnemyPosition;
                    checkEnemyPosition = Vector3.zero;
                    lastCheckedEnemyPosition = Vector3.zero;

                    ResetAttackingTimer();
                    FromAttackStateReturnAlert();
                }else{
                    if (distance > (minDistance * minDistance)) {

                        if (attackState.useAnimations) {
                            animationManager.PlayAnimationState(attackState.moveForwardAnimationName, attackState.moveForwardAnimationTransition);
                        }else{
                            animationManager.PlayAnimationState(alertState.moveAnimationName, alertState.moveAnimationTransition, alertState.useAnimations);
                        }

                        reachedEnd = false;
                        MoveToDestination(checkEnemyPosition);
                        wpIdleTriggered = false;
                        backedUpBy = null;
                    }else{
                        reachedEnd = true;
                    }
                }
            }else{
                attackState.distanceFromEnemy = defaultDistanceFromEnemy;
                float distance = Vector3.Distance(new Vector3(enemyPosition.x, transform.position.y, enemyPosition.z), transform.position);

                attackBackUp = false;
                idleAttack = false;

                if (!attackState.coverShooterOptions.coverShooter) shouldAttack = false;
                RemoveFromEnemyScehduler();

                //if path is unreachable
                if (!PathSetup(enemyPosition)) {
                    FromAttackStateReturnAlert();
                    return;
                }
                
                if (distance > 0.4f) {

                    if (attackState.useAnimations) {
                        animationManager.PlayAnimationState(attackState.moveForwardAnimationName, attackState.moveForwardAnimationTransition);
                    }else{
                        animationManager.PlayAnimationState(alertState.moveAnimationName, alertState.moveAnimationTransition, alertState.useAnimations);
                    }

                    reachedEnd = false;
                    MoveToDestination(enemyPosition);
                    backedUpBy = null;
                }else{
                    wpIdleTriggered = false;
                    attackBackUp = false;
                    waitFrameRan = false;
                    
                    reachedEndEnemyPosition = checkEnemyPosition;
                    checkEnemyPosition = Vector3.zero;
                    lastCheckedEnemyPosition = Vector3.zero;

                    ResetAttackingTimer();
                    FromAttackStateReturnAlert();
                    reachedEnd = true;
                }
            }
        }
    }

    // this will trigger when agent is in attack position
    void AttackPosition()
    {
        if (shouldAttack) {
            if (!startAttackTimer) Attack();
        } else {
            attackBackUp = false;
            targetingEnemy = true;
            
            reachedEnd = true;
            waitFrameRan = false;
            IdleAttackState();

            wpIdleTriggered = false;
            backupPass = 0;
            idleAttack = true;

            if (!attackState.attackInIntervals) ScheduleToEnemy();
            else startIntervalTimer = true;
        }
    }

    //prepare to attack
    void AttackPreparations()
    {   
        StopAllCoroutines();

        normalStateActive = false;
        alertStateActive = false;
        backupPass = 0;

        distracted = false;
        reachedEnd = false;
        waitFrameRan = false;

        startWaypointRotation = false;
        waypointRotationTimer = 0f;
        waypointRotationAnimationTimer = 0f;

        if (attackState.surprised.useSurprised) Surprised();
        else TurnToAttack();
    }
    
    //turn NPC to attack state
    public void TurnToAttack (bool calledFromAnother = false)
    {   
        attackState.surprised.startSurprisedTimerState = false;
        attackState.surprised.startSurprisedTimer = 0f;
       
        StopSystemsAudios("attack");
        
        alertStateActive = false;
        normalStateActive = false;
        isSeenVisionAlertTags = false;

        //when in surprised mode and suddenly the enemy hides
        if (!calledFromAnother && enemyToAttack == null && state != State.hit) {
            endPoint = enemyPosition;
            wpIdleTriggered = false;
            FromAttackStateReturnAlert();
            return;
        }

        state = State.attack;
        
        //called from another NPC
        if (calledFromAnother) {

            if (alertedByOther) return;
            alertedByOther = true;
            
            StopAllCoroutines();
            StartCoroutine(AlertWaitToAttack());
            SetAttackChance();
            
            return;
        }

        alertState.DisableScripts();
        normalState.DisableScripts();

        attackStateActive = true;
        SetAttackChance();
        
        if (isAgent) {
            agentObstacle.enabled = false;
            navmeshAgent.enabled = true;
        }
    }

    //make this npc in an idle-attack state waiting for his turn to attack
    void IdleAttackState()
    {   
        animationManager.PlayAnimationState(attackState.idleAnimationName, attackState.idleAnimationTransition, attackState.useAnimations);
        if (enemyToAttack) rotateToTarget(enemyToAttack.transform.position, 5f);
        if (!attackState.coverShooterOptions.coverShooter) StartCoroutine(DisableAgent());

        CarveAccordingToEnemy();
        backedUpBy = null;
    }
    
    //from attack state return to patrolling in alert state
    void FromAttackStateReturnAlert()
    {
        if (!wpIdleTriggered) {
            state = State.alert;
            attackStateActive = false;
            enemyToAttack = null;
            waitFrameRan = false;
            backedUpBy = null;

            StopAllCoroutines();
            attackBackUp = false;
            ResetAttackingTimer();
            
            float temp = alertState.waitTime;
            alertState.waitTime = attackState.timeToReturnAlert;
            SetAgentObstacle();
            
            StartCoroutine(AlertStateIdle());
            alertState.waitTime = temp;
        }
    }

    //trigger the surprised emotion
    void Surprised()
    {
        //if not surprised before
        if (!attackState.surprised.isSurprised && state == State.normal) {
            attackState.surprised.isSurprised = true;
            animationManager.PlayAnimationState(attackState.surprised.surprisedAnimationName, attackState.surprised.surprisedAnimationTransition, attackState.surprised.useAnimations);
            StopSystemsAudios("attack");
            attackState.PlaySurprisedAudio();
            attackState.surprised.startSurprisedTimerState = true;
        }else{
            if (!attackState.surprised.startSurprisedTimerState) TurnToAttack();
        }
    }

    //if this NPC got alerted by another, turn to alert for a brief moment
    //then resume to attack mode moving to hostile location
    IEnumerator AlertWaitToAttack()
    {
        normalStateActive = false;
        alertStateActive = false;
        distracted = false;

        animationManager.PlayAnimationState(alertState.idleAnimationName, alertState.idleAnimationTransition, alertState.useAnimations);
        yield return new WaitForSeconds(0.4f);

        state = State.attack;
        attackStateActive = true;

        alertedByOther = false;
        reachedEnd = false;
    }

    //in attack state if enemy is too close then backup
    void AttackBackup()
    {   
        if (!useRootMotion) pathFindingProxy.transform.SetParent(transform);
        pathFindingProxy.transform.localPosition = new Vector3(0f, transform.position.y + controller.height, -proxyOffset);

        idleAttack = true;
        StartCoroutine(DisableAgent());
        
        isAgent = false;
        agentEnabled = false;
        
        controller.enabled = true;
        if (!backIsDeadEnd) reachedEnd = false;

        Vector3 targetPosition = transform.position - (transform.forward * 1f);
        backupPoint = GetSamplePosition(ValidateEnemyYPoint(targetPosition), navmeshAgent.height * 2f - 1f);
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position + controller.center, transform.TransformDirection(-Vector3.forward), out hit, 2f, obstacleLayers))
        {
            backupPoint = Vector3.zero;
        }

        if (backupPoint == Vector3.zero) {
            attackBackUp = false;
            reachedEnd = true;
            backIsDeadEnd = true;
            IdleAttackState();
        }else{
            backIsDeadEnd = false;
            attackBackUp = true;
            animationManager.PlayAnimationState(attackState.moveBackwardsAnimationName, attackState.moveBackwardsAnimationTransition, attackState.useAnimations);
        }
    }

    //actual backup movement
    void BackupMovement()
    {
        AttackBackup();
        wpIdleTriggered = false;
        StartCoroutine(WaitFrameSetup());
        
        MoveToDestination(backupPoint);
        ScheduleToEnemy();

        //move backwards and attack if set
        if (attackState.moveBackwardsAttack) Attack(true);
    }
    
    //let this npc schedule itself to the list of enemies inside the enemy
    void ScheduleToEnemy()
    {
        if (attackState.attackInIntervals) return;
        if (enemyToAttack == enemyScheduled) return;

        BlazeAIEnemyManager script = enemyToAttack.GetComponent<BlazeAIEnemyManager>();

        if (script == null) {
            enemyToAttack.AddComponent<BlazeAIEnemyManager>();
            script = enemyToAttack.GetComponent<BlazeAIEnemyManager>();
        }

        if (!script.enemiesScheduled.Contains(this) && (enemyToAttack == script.transform.gameObject)) {
            script.enemiesScheduled.Add(this);
        }

        enemyScheduled = enemyToAttack;
    }
    
    //gets called by the enemy manager to let this NPC go for an attack
    public void GoForAttack()
    {
        waitFrameRan = false;
        shouldAttack = true;

        if (!attackState.moveBackwardsAttack) attackBackUp = false;
        
        idleAttack = false;
        backupPass = 0;
    }

    //NPC has reached attack position
    void Attack(bool backingup = false)
    {   
        if (backingup) {
            animationManager.PlayAnimationState(attackState.moveBackwardsAttackAnimationName, attackState.moveBackwardsAttackAnimationTransition, attackState.moveBackwardsAttack);
        }else{
            attackState.SetRandomAttackAnimation();
            animationManager.PlayAnimationState(attackState.currentAttackAnimation, attackState.attackAnimationTransition);
        }
        
        if (attackState.attackScript != null) attackState.attackScript.enabled = true;
        
        startAttackTimer = true;
        attackState.PlayAttackAudio();
    }

    //stop attack and return to attack idle state
    void StopAttack()
    {
        waitFrameRan = false;
        shouldAttack = false;
        startAttackTimer = false;
        
        wpIdleTriggered = false;
        attackBackUp = false;
        attackTimer = 0f;
        
        if (attackState.attackScript != null) attackState.attackScript.enabled = false;

        //if attacking is set to intervals and interval randomization is enabled
        attackState.RandomizeAttackIntervals();
        if (attackState.attackInIntervals && !attackState.coverShooterOptions.coverShooter) startIntervalTimer = true;
    }

    //remove this NPC from the scheduler of the targeted enemy
    void RemoveFromEnemyScehduler()
    {
        if (enemyToAttack == null || attackState.attackInIntervals) return; 
        BlazeAIEnemyManager script = enemyToAttack.GetComponent<BlazeAIEnemyManager>();

        if (script == null) return;
        script.RemoveEnemy(this);

        enemyScheduled = null;
    }

    //reset the timer that lets the ranged npc fire repeatedly
    void ResetAttackingTimer()
    {
        startIntervalTimer = false;
        intervalTimer = 0f;
    }

    // call agent to check passed location
    public void CallAgentToLocation(Vector3 location, float time=0f, string animationName = null)
    {
        if (!enabled || state == State.attack) return;

        StopAllCoroutines();
        ResetAllFlags();

        state = State.hit;
        reachedEnd = false;
        
        if (animationName != null) animationManager.PlayAnimationState(animationName, 0.3f, true, true);
        else animationManager.PlayAnimationState(alertState.idleAnimationName, alertState.idleAnimationTransition, true, true);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(ValidateEnemyYPoint(location), out hit, navmeshAgent.height * 2f - 1f, NavMesh.AllAreas))
        {
            normalStateActive = false;
            alertStateActive = false;

            wpIdleTriggered = true;
            captureEnemyTimeStamp = Time.time;
            
            endPoint = hit.position;
            checkEnemyPosition = endPoint;

            SetupPath(endPoint);
            StartCoroutine(CallAgentToLocationCoroutine(time));
        }else{
            state = State.alert;
            alertStateActive = true;
            normalStateActive = false;
            attackStateActive = false;
            StateWalk();
        }
    }

    // wait before moving to called location
    IEnumerator CallAgentToLocationCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        TurnToAttack();
    }

    #endregion

    #region Cover

    //moving to cover
    void MoveToCover()
    {   
        if (startCoverTimer || shouldAttack) return;
        
        float reachDistance = 0.3f;
        float dist = (new Vector3(endPoint.x, transform.position.y, endPoint.z) - transform.position).sqrMagnitude;
        pathFindingProxy.transform.SetParent(transform);

        if (dist <= reachDistance * reachDistance) {
            TakeCover();
        }else{
            animationManager.PlayAnimationState(attackState.moveForwardAnimationName, attackState.moveForwardAnimationTransition, attackState.useAnimations);
            idleAttack = false;
            MoveToDestination(endPoint);

            if (targetedEnemyInCover) {
                RaycastHit hit;
                Vector3 dir = targetedEnemyInCover.transform.position - transform.position;
                if (Physics.Raycast(transform.position, dir, out hit, Mathf.Infinity, attackState.coverShooterOptions.coverLayers)) {
                    
                    if (System.Array.IndexOf(vision.hostileTags, hit.transform.tag) < 0 && hit.transform.gameObject == coverObject) {
                        coverNormal = hit.normal;
                        coverNormal.z = 0f;
                        if (!shouldAttack && !attackState.coverShooterOptions.moveToCoverCenter && coverNormal != Vector3.zero && dist <= 1f) TakeCover(true);
                    }

                }
            }
        }
    }

    //in cover method
    void TakeCover(bool delay = false)
    {   
        if (delay) {
            takeCoverDelay = true;
        }else{
            
            if (!useRootMotion && attackState.coverShooterOptions.coverAnimations.rotateToNormal) {
                pathFindingProxy.transform.SetParent(transform.parent);
            }

            intervalTimer = 0f;
            takeCoverDelay = false;
            takeCoverDelayTimer = 0f;
            coverTimer = 0f;

            idleAttack = true;
            findCoverFired = false;
            string animToPlay = "";
            
            if (coverHeight >= attackState.coverShooterOptions.coverAnimations.highCoverHeight) {
                animToPlay = attackState.coverShooterOptions.coverAnimations.highCoverAnimation;
                attackState.coverShooterOptions.coverAnimations.EnableCoverScript("high"); 
            }
            
            if (coverHeight <= attackState.coverShooterOptions.coverAnimations.lowCoverHeight) {
                animToPlay = attackState.coverShooterOptions.coverAnimations.lowCoverAnimation;
                attackState.coverShooterOptions.coverAnimations.EnableCoverScript("low");
            }
            
            animationManager.PlayAnimationState(animToPlay, attackState.coverShooterOptions.coverAnimationTransition);
            
            if (attackState.randomizeAttackIntervals) actualCoverTime = Random.Range(attackState.randomizeAttackIntervalsBetween.x, attackState.randomizeAttackIntervalsBetween.y);
            else actualCoverTime = attackState.attackInIntervalsTime;

            startCoverTimer = true;
            tookCover = true;
            sawOnce = false;
        }
    }

    //attack when in cover
    void FromCoverAttack()
    {
        shouldAttack = true;
        startCoverTimer = false;
        coverTimer = 0f;
        coverLocationSet = false;

        if (attackState.coverShooterOptions.attackEnemyCover == CoverShooterOptions.AttackEnemyCover.AlwaysAttackCover) {
            getEnemyCover = true;
        }else if (attackState.coverShooterOptions.attackEnemyCover == CoverShooterOptions.AttackEnemyCover.Randomize) {
            int random = Random.Range(1,3);
            if (random % 2 == 0) getEnemyCover = true;
            else getEnemyCover = false;
        }else{
            getEnemyCover = false;
        }
    }

    //search for a cover location
    public void FindCover(bool overrun = false, GameObject go = null)
    {   
        coverBlockingAttack = false;
        
        if (overrun) {
            if (findCoverFired || !enemyToAttack || shouldAttack) return;
        }else{
            if (coverLocationSet || findCoverFired || !enemyToAttack || shouldAttack || startIntervalTimer) return;
        }

        if (!overrun) {
            if (hideFrames >= 5) hideFrames = 0;
            else { 
                hideFrames++;
                return;
            }
        }else{
            attackStateActive = false;
            startCoverTimer = false;
            coverLocationSet = false;
        }

        findCoverFired = true;
        
        for (int i = 0; i < hideColliders.Length; i++) {
            hideColliders[i] = null;
        }

        int hits = Physics.OverlapSphereNonAlloc(transform.position, attackState.coverShooterOptions.searchDistance, hideColliders, attackState.coverShooterOptions.coverLayers, queryTriggerInteraction: QueryTriggerInteraction.Collide);
        int hitReduction = 0;
        bool noCover = true;

        // eliminate bad options
        for (int i=0; i<hits; i++) {
            if (Vector3.Distance(hideColliders[i].transform.position, transform.position) > attackState.coverShooterOptions.searchDistance || hideColliders[i].bounds.size.y < attackState.coverShooterOptions.minObstacleHeight)
            {
                hideColliders[i] = null;
                hitReduction++;
            }else{
                Collider[] col;
                col = Physics.OverlapSphere(new Vector3(hideColliders[i].transform.position.x, transform.position.y, hideColliders[i].transform.position.z), (hideColliders[i].bounds.size.x + hideColliders[i].bounds.size.z), Physics.AllLayers, queryTriggerInteraction: QueryTriggerInteraction.Collide);
                Collider[] col2;
                col2 = Physics.OverlapSphere(transform.position, attackState.coverShooterOptions.searchDistance, Physics.AllLayers, QueryTriggerInteraction.Collide);

                // check if cover has npc occupying
                foreach (var item in col) {
                    BlazeAI script = item.GetComponent<BlazeAI>();
                    
                    if (script != null) {
                        if (go != null) {
                            if (script.coverObject != null && hideColliders[i] != null) {
                                if (hideColliders[i].transform.gameObject == go || script.coverObject == go) {
                                    hideColliders[i] = null;
                                    hitReduction++;
                                    
                                    continue;
                                }
                            }
                        }else{
                            if (script != this) {
                                if (script.coverObject != null && hideColliders[i] != null) {
                                    if (script.coverObject == hideColliders[i].transform.gameObject) {
                                        hideColliders[i] = null;
                                        hitReduction++;
            
                                        continue;
                                    }
                                }
                            }
                        }
                    }

                    // check if nearby npcs going to same location
                    foreach (var item2 in col2) {
                        BlazeAI npc = item2.GetComponent<BlazeAI>();
                        if (npc != null) {
                            if (npc != this) {
                                if (npc.coverObject != null && hideColliders[i] != null) {
                                    if (go != null) {
                                        if (npc.coverObject == go) {
                                            hideColliders[i] = null;
                                            hitReduction++;

                                            break;
                                        }
                                    }else{
                                        if (npc.coverObject == hideColliders[i].transform.gameObject) {
                                            hideColliders[i] = null;
                                            hitReduction++;

                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        hits -= hitReduction;
        System.Array.Sort(hideColliders, ColliderArraySortComparer);

        // if no obstacles found
        if (hits <= 0) {
            startIntervalTimer = true;
            coverLocationSet = false;
            findCoverFired = false;
            attackStateActive = true;
            return;
        }

        // if found obstacles
        for (int i = 0; i < hits; i++)
        {
            if (NavMesh.SamplePosition(hideColliders[i].transform.position, out NavMeshHit hit, 5f, navmeshAgent.areaMask))
            {
                if (!NavMesh.FindClosestEdge(hit.position, out hit, NavMesh.AllAreas))
                {
                    // found nothing
                    noCover = true;
                }

                if (Vector3.Dot(hit.normal, (enemyToAttack.transform.position - hit.position).normalized) < attackState.coverShooterOptions.hideSensitivity)
                {
                    coverLocationSet = true;
                    endPoint = hit.position;
                    reachedEnd = false;
                    coverObject = hideColliders[i].transform.gameObject;
                    coverHeight = hideColliders[i].bounds.size.y;
                    attackStateActive = true;
                    noCover = false;
                    break;
                }
                else
                {
                    // Since the previous spot wasn't facing "away" enough from the target, we'll try on the other side of the object
                    if (NavMesh.SamplePosition(hideColliders[i].transform.position - (enemyToAttack.transform.position - hit.position).normalized * 2, out NavMeshHit hit2, 5f, navmeshAgent.areaMask))
                    {
                        if (!NavMesh.FindClosestEdge(hit2.position, out hit2, NavMesh.AllAreas))
                        {
                            // found nothing
                            noCover = true;
                        }

                        if (Vector3.Dot(hit2.normal, (enemyToAttack.transform.position - hit2.position).normalized) < attackState.coverShooterOptions.hideSensitivity)
                        {
                            coverLocationSet = true;
                            endPoint = hit2.position;
                            reachedEnd = false;
                            coverObject = hideColliders[i].transform.gameObject;
                            coverHeight = hideColliders[i].bounds.size.y;
                            attackStateActive = true;
                            noCover = false;
                            break;
                        }
                    }
                }
            }
        }

        //no cover found
        if (noCover) {
            coverLocationSet = false;
            startIntervalTimer = true;
        }else{
            if (CoverOccupied(coverObject)) {
                coverLocationSet = false;
                startIntervalTimer = true;
            }
        }

        attackStateActive = true;
        findCoverFired = false;
    }

    //check if cover is occupied by another npc
    bool CoverOccupied(GameObject go)
    {
        if (go == null) return true;

        Collider objectCollider = go.GetComponent<Collider>();
        Collider[] col;
        col = Physics.OverlapSphere(new Vector3(go.transform.position.x, transform.position.y, go.transform.position.z), (objectCollider.bounds.size.x + objectCollider.bounds.size.z), Physics.AllLayers, QueryTriggerInteraction.Collide);
        
        foreach (var item in col) {
            BlazeAI script = item.GetComponent<BlazeAI>();
            if (script != null) {
                if (script != this) {
                    if (script.coverObject != null) {
                        if (script.coverObject == go) 
                        {
                            float thisDistance = (go.transform.position - transform.position).sqrMagnitude;
                            float otherDistance = (go.transform.position - script.transform.position).sqrMagnitude;
                            
                            // if this npc is more far from cover
                            if (thisDistance > otherDistance) {
                                findCoverFired = false;
                                return true;
                            }else{
                                if (script.startCoverTimer || script.takeCoverDelay) {
                                    findCoverFired = false;
                                    return true;
                                }
                            }
                            
                        }
                    }
                }
            }
        }

        return false;
    }

    int ColliderArraySortComparer(Collider A, Collider B)
    {
        if (A == null && B != null)
        {
            return 1;
        }
        else if (A != null && B == null)
        {
            return -1;
        }
        else if (A == null && B == null)
        {
            return 0;
        }
        else
        {
            return Vector3.Distance(transform.position, A.transform.position).CompareTo(Vector3.Distance(transform.position, B.transform.position));
        }
    }

    //set the attack chance if cover shooter
    void SetAttackChance()
    {
        if (attackState.coverShooterOptions.coverShooter) {
            if (attackState.coverShooterOptions.firstSightChance != CoverShooterOptions.FirstSightChance.TakeCover) {
                if (attackState.coverShooterOptions.firstSightChance == CoverShooterOptions.FirstSightChance.Randomize) {
                    int luck = Random.Range(1, 3);
                    if (luck % 2 == 0) shouldAttack = false;
                    else shouldAttack = true;
                }else{
                    shouldAttack = true;
                }
            }
        }
    }

    #endregion

    #region Movement

    //trigger the state walk
    public void StateWalk()
    {
        if (stateWalkTimerRun) return;
        stateWalkTimerRun = true;

        if (!Application.isPlaying) return;

        bool _distracted = distracted;
        ResetAllFlags();

        //if random mode chosen
        if (waypoints.randomize) {
            if (!wpRandomMode) {
                wpRandomMode = true;
                RandomNavmeshLocation();
                waitFrameRan = false;
                StopAllCoroutines();
                StartCoroutine(WaitFrameSetup());
            }
        }else{
            //if looping is set to true and reached end, return index to 0
            if (waypointIndex >= (waypoints.waypoints.Length - 1)) {
                if (waypoints.loop) waypointIndex = 0;
            }else{
                if (_distracted) {
                    if (!waypointInterrupted && (normalStateActive || alertStateActive) && reachedEndBeforeDistraction) waypointIndex++;
                }else{
                    if (!waypointInterrupted && (normalStateActive || alertStateActive)) waypointIndex++;
                }
            }
            
            if (waypoints.waypoints.Length > 0) {
                endPoint = waypoints.waypoints[waypointIndex];
            }
        
            wpRandomMode = false;
            waitFrameRan = false;

            StopAllCoroutines();
            StartCoroutine(WaitFrameSetup());
        }

        //trigger the scripts and the animations
        if (state == State.normal) {
            normalState.TriggerScripts("walking");
            normalStateActive = true;
        }else{
            alertState.TriggerScripts("walking");
            alertStateActive = true;
        }
    }

    //move to passed destination
    void MoveToDestination(Vector3 pos, float reachDistance = 0.32f)
    {
        if (reachedEnd) return;

        if ((state == State.normal || state == State.alert) && !useRootMotion) {
            if (slowMove) {

                float transitionTime = 0f;
                
                if (state == State.normal) transitionTime = normalState.moveAnimationTransition;
                if (state == State.alert) transitionTime = alertState.moveAnimationTransition;

                slowMoveTimer += Time.deltaTime;
                
                if (slowMoveTimer >= transitionTime) {
                    slowMove = false;
                    slowMoveTimer = 0f;
                } 
            }
        }

        if (goingToVisionAlertTag) reachDistance = 2f;

        distractionTurn = false;
        pathFramesElapsed += Time.deltaTime;
        endPoint = pos;

        Vector3 currentTransform;
        float tempMagnitude;
        
        currentTransform = new Vector3(pathFindingProxy.position.x, transform.position.y, pathFindingProxy.position.z);
        tempMagnitude = (new Vector3(pos.x, transform.position.y, pos.z) - currentTransform).sqrMagnitude;
        
        if (tempMagnitude <= reachDistance * reachDistance) {

            reachedEnd = true;
            waitFrameRan = false;
            goingToVisionAlertTag = false;
            
            reachedEndEnemyPosition = checkEnemyPosition;
            checkEnemyPosition = Vector3.zero;
            pathFramesElapsed = 0f;

            //if the location the NPC is going to is the distraction point, then play audio
            if (goingToDistractionPoint) {
                StopSystemsAudios("distraction");
                distractions.TriggerDistractionSearchAudio();
            }

        } else { 
            if (pathFramesElapsed >= pathRecalculationRate) {
                pathFramesElapsed = 0f;
                if (isAgent) {
                    if (navmeshAgent.enabled && !agentObstacle.enabled) navmeshAgent.SetDestination(ValidateEnemyYPoint(pos));
                }else{
                    SetupPath(pos);
                }
            }
        }
        
        if (!isAgent) MoveToPoint();
        pos = new Vector3(pos.x, transform.position.y, pos.z);
        
        //trigger a raycast when distance is at the ray distance value 
        //to avoid facing an obstacle too closely
        if (avoidFacingObstacles) {
            if ((pos - currentTransform).sqrMagnitude <= obstacleRayDistance) {
                Vector3 toOther = (pos - transform.position).normalized;
                float dotProd = Vector3.Dot(toOther, transform.forward);
                
                if (dotProd >= 0.8f) activateRay = true;
                else activateRay = false;
            }
        }
    }

    //smooth rotate NPC to target
    void rotateToTarget(Vector3 location, float speed)
    {
        Quaternion lookRotation = Quaternion.LookRotation((location - transform.position).normalized);
        lookRotation = new Quaternion(0f, lookRotation.y, 0f, lookRotation.w);
        transform.rotation = Quaternion.Slerp(new Quaternion(0f, transform.rotation.y, 0f, transform.rotation.w), lookRotation, speed * Time.deltaTime);
    }

    //setup up the navmesh path to the destination
    void SetupPath(Vector3 destination)
    {
        bool pathValidation = NavMesh.CalculatePath(ValidateEnemyYPoint(pathFindingProxy.position), ValidateEnemyYPoint(destination), NavMesh.AllAreas, path);

        //if no path to destination
        if (!pathValidation || (path.status == NavMeshPathStatus.PathPartial)) {
            if (state != State.attack) {
                if (!IsPointOnNavMesh(ValidateEnemyYPoint(pathFindingProxy.position), 0.5f)) {
                    forceTurn = true;
                    StopAllCoroutines();
                    return;
                }else{
                    StopAgent();
                    StateWalk();
                }
                return;
            } else {
                if (!attackBackUp && !targetingEnemy) {
                    state = State.alert;
                    StateWalk();
                    checkEnemyPosition = Vector3.zero;
                    lastCheckedEnemyPosition = Vector3.zero;
                    return;
                }else{
                    return;
                }
            }
        }

        cornerQueue = new Queue<Vector3>();
        int max = path.corners.Length;

        for (int i = 1; i < max; i++) {
            cornerQueue.Enqueue(path.corners[i]);
        }
        
        GetNextCorner();
    }

    // get the next corner
    void GetNextCorner()
    {
        if (cornerQueue.Count > 0) {
            currentDestination = cornerQueue.Dequeue();
            hasPath = true;
        }else{
            hasPath = false;
        }
    }
    
    //move the proxy and lerp the NPC behind it
    void MoveToPoint()
    {
        //set the speeds of movement and rotation
        float speed;
        float rotationSpeed;
        
        if (state == State.normal) {
            speed = normalState.moveSpeed;
            rotationSpeed = normalState.rotationSpeed;
        }else if (state == State.alert) {
            speed = alertState.moveSpeed;
            rotationSpeed = alertState.rotationSpeed;
        }else{
            if (attackBackUp) speed = attackState.moveBackwardsSpeed;
            else speed = attackState.moveSpeed;
            rotationSpeed = alertState.rotationSpeed;
        }

        //decrease speed at the beginning of movement
        if (!useRootMotion) {
            if (state == State.normal || state == State.alert) {
                if (slowMove) speed = speed / 2f;
            }
        }
        
        float minDistance = 0f;
        if (shouldAttack) minDistance = 0.2f;
        else minDistance = 0.1f;
        
        //move the path finding proxy
        if (hasPath) {
            float currentDistance = (pathFindingProxy.position - new Vector3(currentDestination.x, pathFindingProxy.position.y, currentDestination.z)).sqrMagnitude;
            float distanceFromNPC = (new Vector3(pathFindingProxy.position.x, transform.position.y, pathFindingProxy.position.z) - transform.position).sqrMagnitude;

            if ((distanceFromNPC) <= proxyDistance) {
                if (currentDistance > minDistance) {
                    if (!useRootMotion) pathFindingProxy.transform.position = Vector3.MoveTowards(pathFindingProxy.transform.position, new Vector3(currentDestination.x, pathFindingProxy.transform.position.y, currentDestination.z), Time.deltaTime * speed);
                }else{
                    GetNextCorner();
                }
            }
        }

        //move the controller (Actual NPC)
        var direction = (new Vector3(pathFindingProxy.transform.position.x, transform.position.y, pathFindingProxy.transform.position.z) - transform.position);
        
        if (!useRootMotion) { 
            if (controller.enabled) controller.Move(direction * (((speed/2f) + 1f) * Time.deltaTime)); 
        } else { 
            if (controller.enabled && shouldAttack) controller.Move(rootMotionDirection * Time.deltaTime); 
        }
        
        if (!attackBackUp) rotateToTarget(currentDestination, rotationSpeed);
    }

    //apply a gravity force to keep controller grounded
    void KeepToGravity()
    {
        if (stop) return;
        if (isAgent) return;
        if (!controller.enabled) return;

        bool isGrounded = controller.isGrounded;
        int verticalVelocity = 0;

        if (isGrounded) {
            verticalVelocity -= 0;
        }else{
            verticalVelocity -= 1;
        }

        var moveVector = new Vector3(0, verticalVelocity, 0);
        controller.Move(moveVector);
    }

    //obstacle disabling takes a frame to compeltely close
    //so wait and then set up the path
    IEnumerator WaitFrameSetup() 
    {
        if (waitFrameRan) yield break;
        if (isAgent || agentEnabled) yield break;
        if (attackState.coverShooterOptions.coverShooter && shouldAttack) yield break;
        
        waitFrameRan = true;
        
        if (!attackBackUp) {
            if (!goingToDistractionPoint) {
                if (!useRootMotion) {
                    pathFindingProxy.transform.SetParent(transform.parent);
                    proxyCopy.transform.localPosition = new Vector3(0f, 0f, proxyOffset);
                    pathFindingProxy.transform.position = proxyCopy.transform.position;
                    
                } else {
                    pathFindingProxy.transform.SetParent(transform);
                    pathFindingProxy.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                    pathFindingProxy.transform.localPosition = new Vector3(0f, 0f, proxyOffset);
                }
            }
        } else {
            if (backupPass == 0) {
                pathFindingProxy.transform.SetParent(transform);
                pathFindingProxy.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                pathFindingProxy.transform.localPosition = new Vector3(0f, 0f, -proxyOffset);
            }
        }

        if (attackBackUp) {
            if (backupPass == 0) backupPass++;
            SetupPath(backupPoint);
        }else{
            if (enemyToAttack) SetupPath(enemyToAttack.transform.position);
            else SetupPath(endPoint);
        }

        reachedEnd = false;
        yield break;
    }

    //from state idle to respective state walk
    void FromIdleToWalk()
    {
        if (state == State.normal) {

            if (normalState.instantMoveChange) normalState.InstantMoveReturnsVals();
            
            if (goingToDistractionPoint) {
                goingToDistractionPoint = false;
                distractions.DisableScript();
                StateWalk();
                waypointInterrupted = false;
            }else{
                waypointInterrupted = false;
                StateWalk();
            }
            
            normalStateActive = true;

            //check to trigger slow movement at the beginning
            if (normalState.useAnimations && !useRootMotion) {
                if (normalState.moveAnimationName.Length > 0) slowMove = true;
            }
        }
        else if (state == State.alert) {

            if (alertState.instantMoveChange) alertState.InstantMoveReturnsVals();
            
            if (goingToDistractionPoint) {
                goingToDistractionPoint = false;
                distractions.DisableScript();
                StateWalk();
                waypointInterrupted = false;
            }else{
                waypointInterrupted = false;
                StateWalk();
            }

            alertStateActive = true;

            //check to trigger slow movement at the beginning
            if (alertState.useAnimations && !useRootMotion) {
                if (alertState.moveAnimationName.Length > 0) slowMove = true;
            }
        }
    }

    //check if current waypoint stop has a rotation
    bool WaypointRotationCheck() 
    {
        if (goingToDistractionPoint || (state != State.normal && state != State.alert) || waypoints.randomize || isSeenVisionAlertTags || goingToVisionAlertTag) return false;
        if ( (waypointIndex+1) > waypoints.waypoints.Length || waypointIndex < 0) return false;
        
        float dist = Vector3.Distance(new Vector3(waypoints.waypoints[waypointIndex].x, transform.position.y, waypoints.waypoints[waypointIndex].z), new Vector3(pathFindingProxy.transform.position.x, transform.position.y, pathFindingProxy.transform.position.z));
        if (dist > 0.5f) return false;

        if ((waypoints.waypointsRotation[waypointIndex].x != 0 || waypoints.waypointsRotation[waypointIndex].y != 0)) {
            Vector3 toOther = (new Vector3(transform.position.x + waypoints.waypointsRotation[waypointIndex].x, transform.position.y, transform.position.z + waypoints.waypointsRotation[waypointIndex].y) - transform.position).normalized;
            float dotProd = Vector3.Dot(toOther, transform.forward);
            
            if (dotProd < 0.97f) return true;
            else return false;
            
        }else{
            return false;
        }
    }

    //trigger the rotation
    void TriggerWaypointRotation() 
    {
        if (state == State.normal) {
            StartCoroutine(NormalStateIdle(true));
        } else if (state == State.alert) {
            StartCoroutine(AlertStateIdle(true));
        }

        startWaypointRotation = true;
        Vector3 heading = new Vector3(transform.position.x + waypoints.waypointsRotation[waypointIndex].x, 0f, transform.position.z + waypoints.waypointsRotation[waypointIndex].y) - transform.position;
        float dirNum = distractions.AngleDir(transform.forward, heading, transform.up);

        waypointTurnDir = (int)dirNum;
    }

    //smooth rotate NPC to target
    void rotateToWaypoint(float[] degrees, float speed)
    {
        Quaternion lookRotation = Quaternion.LookRotation((new Vector3(transform.position.x + degrees[0], transform.position.y, transform.position.z + degrees[1]) - transform.position).normalized);
        lookRotation = new Quaternion(0f, lookRotation.y, 0f, lookRotation.w);
        transform.rotation = Quaternion.Slerp(new Quaternion(0f, transform.rotation.y, 0f, transform.rotation.w), lookRotation, speed * Time.deltaTime);
    }

    //enable navmesh agent for crowd control
    IEnumerator enableAgentCoroutine;
    IEnumerator EnableAgent()
    {
        pathFindingProxy.transform.SetParent(transform);
        agentObstacle.enabled = false;

        if (agentEnabled || isAgent) yield break;

        StopCoroutine(disableAgentCoroutine);
        agentEnabled = true;
        
        yield return new WaitForSeconds(0.03f);
        
        isAgent = true;
        controller.enabled = false;
        navmeshAgent.enabled = true;
    }

    //disable navmesh agent
    IEnumerator disableAgentCoroutine;
    IEnumerator DisableAgent()
    {
        if (!useRootMotion && !distracted) pathFindingProxy.transform.SetParent(transform.parent);
        navmeshAgent.enabled = false;
        if (!isAgent) yield break;

        StopCoroutine(enableAgentCoroutine);
        agentEnabled = false;
        
        yield return new WaitForSeconds(0.03f);
        
        waitFrameRan = false;
        agentObstacle.enabled = true;

        controller.enabled = true;
        isAgent = false;
    }

    //method for stopping the agent completely
    public void StopAgent()
    {
        reachedEnd = true;
        navmeshAgent.enabled = false;
        isAgent = false;
        agentEnabled = false;
    }

    #endregion

    #region Distractions

    //responsible for distracting the enemy and calling stop/idle on all states
    public void Distract(Transform location, bool groupDistraction = false)
    {
        if (enabled && distractions.alwaysUse && !distracted && (state != State.attack) && !attackState.surprised.startSurprisedTimerState) {
            StopAllCoroutines();
            StopSystemsAudios("distraction");

            normalStateActive = false;
            alertStateActive = false;

            reachedEndBeforeDistraction = reachedEnd;
            reachedEnd = true;

            activateRay = false;
            distracted = true;

            startWaypointRotation = false;
            waypointRotationTimer = 0f;
            waypointRotationAnimationTimer = 0f;

            pathFindingProxy.transform.SetParent(transform);

            //if set, on distraction turn to alert
            if (distractions.turnAlertOnDistraction && state != State.alert) {
                state = State.alert;
            }

            if (state == State.normal) {
                StartCoroutine(NormalStateIdle(true));
                normalState.StopCurrentAudio();
            }

            if (state == State.alert) {
                StartCoroutine(AlertStateIdle(true));
                alertState.StopCurrentAudio();
            }

            if (groupDistraction) passDistractionCheck = true;
            else passDistractionCheck = false;
            
            //validate and enable script
            distractions.EnableScript();
            
            distractionLocation = location;
            distractionPosition = location.position;
            distractions.waitBeforeTurn = true;
        }
    }

    //coroutine for waiting before turning to distraction
    IEnumerator WaitBeforeTurn(float time, bool skipAutoTurn = false)
    {
        yield return new WaitForSeconds(time);
        if (!skipAutoTurn) distractionTurn = true;
        
        //get distraction direction (left or right)
        //and play the corresponding animation
        if (distractions.useTurnAnimations) {
            Vector3 toOther = (distractionLocation.position - transform.position).normalized;
            float dotProd = Vector3.Dot(toOther, transform.forward);
            
            //play animation only when distraction is NOT in front of NPC
            if (dotProd < 0.98f) {
                Vector3 heading = distractionLocation.position - transform.position;
                float dirNum = distractions.AngleDir(transform.forward, heading, transform.up);

                //dir to turn
                if (dirNum == 1) {
                    if (state == State.normal)
                        animationManager.PlayAnimationState(distractions.rightTurnAnimationNameNormal, distractions.rightTurnTransitionNormal);
                    else
                        animationManager.PlayAnimationState(distractions.rightTurnAnimationNameAlert, distractions.rightTurnTransitionAlert);
                }
                
                if (dirNum == -1) {
                    if (state == State.normal)
                        animationManager.PlayAnimationState(distractions.leftTurnAnimationNameNormal, distractions.leftTurnTransitionNormal);
                    else
                        animationManager.PlayAnimationState(distractions.leftTurnAnimationNameAlert, distractions.leftTurnTransitionAlert);
                }
            }
        }
        
        //if false - means he wasn't in a group so control flow should be normal
        if (!passDistractionCheck) {
            //normal control flow check whether NPC is supposed to check location or not
            if (distractions.moveToDistractionLocation){
                yield return StartCoroutine(WaitBeforeMovingToLocation(distractions.moveToDistractionReactTime));
            }else
                yield return StartCoroutine(TurnToDistractionNoMoving(distractions.moveToDistractionReactTime));
        }else{
            yield return StartCoroutine(TurnToDistractionNoMoving(distractions.moveToDistractionReactTime));
        }
    }

    //coroutine for waiting before moving to distraction location
    IEnumerator WaitBeforeMovingToLocation(float time)
    {
        yield return new WaitForSeconds(time);

        StartCoroutine(distractions.EnableAudiosToBePlayedAgain());
        Vector3 pos = ValidateEnemyYPoint(distractionPosition);
        
        if (!useRootMotion) pathFindingProxy.transform.SetParent(transform.parent);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(pos, out hit, navmeshAgent.height * 2f - 1f, NavMesh.AllAreas)) {
            distracted = false;
            endPoint = pos;
            waitFrameRan = false;
    
            goingToDistractionPoint = true;
            yield return StartCoroutine(WaitFrameSetup());

            if (state == State.normal) { 
                normalState.TriggerScripts("walking");
                normalStateActive = true;
            }

            if (state == State.alert) {
                alertState.TriggerScripts("walking");
                alertStateActive = true;
            }
        }else{
            StateWalk();
        }
    }

    //a coroutine special for if auto turn is selected but moving to location isn't
    IEnumerator TurnToDistractionNoMoving(float time)
    {
        yield return new WaitForSeconds(time);
        if (!useRootMotion) pathFindingProxy.transform.SetParent(transform.parent);
        
        wpRandomMode = false;
        passDistractionCheck = false;
        waypointInterrupted = false;

        if (state == State.normal) normalStateActive = true;
        if (state == State.alert) alertStateActive = true;

        StateWalk();
        distractions.DisableScript();

        StartCoroutine(distractions.EnableAudiosToBePlayedAgain());
    }

    #endregion

    #region Navmesh

    //get random point from navmesh
    void RandomNavmeshLocation() 
    {
        float walkRadius = 20f;
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        randomDirection += transform.position;
        
        NavMeshHit hit;
        Vector3 point;

        NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
        point = hit.position;

        float distance = (new Vector3(point.x, transform.position.y, point.y) - transform.position).sqrMagnitude;
        if (distance <= 5f * 5f) {
            RandomNavmeshLocation();
            return;
        }

       endPoint = point;
    }

    //check if path is complete
    bool PathSetup(Vector3 point)
    {
        if (path.status == NavMeshPathStatus.PathComplete) { 
            return true;
        }else{
            if (state == State.normal) {
                StopAgent();
                StartCoroutine(NormalStateIdle());
            }

            if (state == State.alert) {
                StopAgent();
                StartCoroutine(AlertStateIdle());
            }

            return false;
        }
    }

    //check whether point is on navmesh or not
    bool IsPointOnNavMesh(Vector3 point, float radius = 2f)
    {
        NavMeshHit hit;

        if (NavMesh.SamplePosition(point, out hit, radius, NavMesh.AllAreas)) return true;
        else return false;
    }

    //get random position within point
    Vector3 GetSamplePosition(Vector3 point, float range)
    {
        NavMeshHit hit;

        if (NavMesh.SamplePosition(point, out hit, range, NavMesh.AllAreas)) return hit.position;
        else return Vector3.zero;
    }

    //get the correct y position of an enemy
    Vector3 ValidateEnemyYPoint(Vector3 pos)
    {
        if (!IsPointOnNavMesh(pos, 0.3f)) {
            RaycastHit downHit;
            if (Physics.Raycast(pos, -Vector3.up, out downHit, Mathf.Infinity, groundLayers)) {
                return downHit.point;
            }else{
                return new Vector3(pos.x, transform.position.y, pos.z);
            }
        }else{
            return pos;
        }
    }

    #endregion
    
    #region NPC Setup

    //restructure the gameobjects heirarchy for use by BlazeAI
    public void BuildNPC()
    {      
        //make a new container gameobject
        GameObject containerGO = new GameObject();
        containerGO.name = gameObject.name + "Container";
        containerGO.transform.localPosition = transform.position;

        //make this current object a child of the generated object
        transform.parent = containerGO.transform;

        GameObject proxyGO = new GameObject();
        proxyGO.name = "PathFindingProxy";

        proxyGO.transform.parent = containerGO.transform;
        proxyGO.transform.localPosition = new Vector3(0f, 0f, 0f);
        pathFindingProxy = proxyGO.transform;

        CharacterController buildController = GetComponent<CharacterController>();
        agentObstacle = GetComponent<NavMeshObstacle>();

        buildController.center = new Vector3 (0f, 0.9f, 0f);
        buildController.radius = 0.3f;
        buildController.height = 1.75f;
        buildController.stepOffset = 0.1f;
        buildController.skinWidth = 0.01f;

        navmeshAgent = GetComponent<NavMeshAgent>();
        navmeshAgent.enabled = false;
        
        AgentSpeeds();
        SetAgentObstacle(true);

        capsuleCollider = GetComponent<CapsuleCollider>();
        capsuleCollider.height = buildController.height;
        capsuleCollider.radius = buildController.radius;
        capsuleCollider.center = buildController.center;
        capsuleCollider.enabled = false;
        capsuleCollider.isTrigger = true;
        
        Debug.Log("Set the proxy offset property in the inspector to a good offset position. This positions the pathfinding proxy. Make sure the proxy is not within the navmesh obstacle.");
        Debug.Log("You can now edit the navmesh obstacle properties in it's new heirarchal structure.");
        Debug.Log("Make sure you set the character controller properties (height, center, radius) to whatever suits your needs.");
        Debug.LogWarning("Don't change the name of the PathFindingProxy gameobject (for identification purposes)");
        Debug.LogWarning("If you're going to use animations then don't forget to set the Animator Controller and avatar");
    }

    //check whether the build structure is correct
    public bool CheckNPCBuild()
    {
        if (transform.parent != null) {
            foreach (Transform item in transform.parent)
            {
                if (item.gameObject.name == "PathFindingProxy") {
                    return true;
                }
            }
            return false;
        }else{
            return false;
        }
    }

    //best obstacle settings
    //obstacle is imperative for agents avoiding each other
    void SetAgentObstacle(bool buildRun = false)
    {
        //OVERRIDE THE SETTINGS HERE IF NEEDED
        agentObstacle.carving = true;
        agentObstacle.carvingMoveThreshold = 0f;
        agentObstacle.carvingTimeToStationary = 0f;
        agentObstacle.carveOnlyStationary = false;
        
        if (buildRun) agentObstacle.size = new Vector3(0.5f, 1f, 0.1f);
    }

    //remove the carve from enemy if AI vs AI
    public void TurnCarveOff()
    {
        agentObstacle.carving = false;
    }

    //disable/enable carve on enemy
    void CarveAccordingToEnemy()
    {
        if (enemyToAttack) {
            BlazeAI script = enemyToAttack.GetComponent<BlazeAI>();
            if (script != null) {
                script.TurnCarveOff();
            }else{
                SetAgentObstacle();
            }
        }else{
            SetAgentObstacle();
        }
    }

    //set the agent speeds
    void AgentSpeeds()
    {
        navmeshAgent.speed = attackState.moveSpeed - 3f;
        navmeshAgent.angularSpeed = 360f;
        navmeshAgent.acceleration = 9999f;
    }
    
    //adjust the important components that need to be enabled on start
    void AdjustComponentsStatesOnStart()
    {
        navmeshAgent.enabled = false;
        capsuleCollider.enabled = true;
        capsuleCollider.isTrigger = true;
        
        agentObstacle.enabled = true;
        controller.enabled = true;
        
        capsuleCollider.height = controller.height;
        capsuleCollider.radius = 0.9f;
        capsuleCollider.center = controller.center;

        if (useRootMotion) anim.updateMode = AnimatorUpdateMode.AnimatePhysics;
        else anim.updateMode = AnimatorUpdateMode.Normal;

        if (attackState.coverShooterOptions.coverShooter) navmeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        else navmeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;

        proxyCopy = new GameObject();
        proxyCopy.transform.SetParent(transform);
        proxyCopy.transform.localPosition = Vector3.zero;
        proxyCopy.name = "ProxyCopy";
    }

    #endregion

    #region Vision
    
    // vision cone
    void VisionCheck()
    {
        //run every 4 frames for better performance
        if (visionFramesElapsed == FRAMES_THRESHOLD) {
            visionFramesElapsed = 0;
            
            float radius, angle;
            RaycastHit rayHit;
            List<GameObject> enemiesToAttack = new List<GameObject>();
            
            //set the radius and angle according to state
            if (state == State.normal) {
                if (attackState.surprised.startSurprisedTimerState) {
                    angle = vision.visionDuringAttackState.coneAngle;
                    radius = vision.visionDuringAttackState.sightRange;
                }else{
                    angle = vision.visionDuringNormalState.coneAngle;
                    radius = vision.visionDuringNormalState.sightRange;
                }
            } else if (state == State.alert) {
                angle = vision.visionDuringAlertState.coneAngle;
                radius = vision.visionDuringAlertState.sightRange;
            } else {
                angle = vision.visionDuringAttackState.coneAngle;
                radius = vision.visionDuringAttackState.sightRange;
            }
            
            Collider[] collisions = Physics.OverlapSphere(visionT.position + new Vector3(0f, vision.sightLevel, 0f), radius - 2f);
            List<Collider> collisionsList = new List<Collider>(collisions);
            List<GameObject> collisionObjects = new List<GameObject>();
            int visionLoopMax = collisionsList.Count;

            //filter colliders of same gameobjects
            for (var i=0; i<visionLoopMax; i+=1) {
                if (collisionObjects.Contains(collisionsList[i].transform.gameObject)) {
                    collisionsList.RemoveAt(i);
                    i--;
                    visionLoopMax--;
                }else{
                    collisionObjects.Add(collisionsList[i].transform.gameObject);
                }
            }

            //loop the filtered collisions
            foreach (var hit in collisionsList) {
                //check for alert tags
                if (state != State.attack) {
                    if (vision.alertTagsDict.ContainsKey(hit.tag)) {
                        Vector3 npcDir = transform.position + new Vector3(0f, vision.sightLevel, 0f);
                        Vector3 targetDir = hit.transform.position - (transform.position + new Vector3(0f, vision.sightLevel, 0f));

                        debugPlayerDir = npcDir;
                        debugTargetDir = targetDir;
                        
                        //check if alert tag is within vision
                        if (Physics.Raycast(npcDir, targetDir, out rayHit)) 
                        {
                            float alertHeight = rayHit.transform.position.y - (transform.position.y + vision.sightLevel);
                            
                            if (alertHeight < vision.sightLevel + vision.maxSightLevel) {
                                if (vision.alertTagsDict.ContainsKey(rayHit.transform.tag)) {
                                    
                                    //check if within cone of vision
                                    if (Vector3.Angle(transform.forward, targetDir) <= (angle * 0.5f)) {
                                        state = State.alert;
                                        seenVisionAlertTag = rayHit.transform.tag;

                                        normalStateActive = false;
                                        rayHit.transform.tag = vision.alertTagsDict[seenVisionAlertTag].fallBackTag;

                                        //play random audio specific of this tag
                                        StopSystemsAudios("vision");
                                        vision.AlertTagsPlayRandomAudio(seenVisionAlertTag);
                                        isSeenVisionAlertTags = true;

                                        //vision alert tags options such as animations playing and moving to location
                                        if (vision.alertTagsDict[seenVisionAlertTag].moveToLocation) {
                                            endPoint = ValidateEnemyYPoint(hit.transform.position);
                                            reachedEnd = false;
                                        }else{
                                            animationManager.PlayAnimationState(vision.alertTagsDict[seenVisionAlertTag].animationName, alertState.moveAnimationTransition);
                                            StartCoroutine(AlertStateIdle());
                                        }

                                        alertStateActive = true;
                                        checkEnemyPosition = ValidateEnemyYPoint(hit.transform.position);
                                        waypointInterrupted = true;
                                        if (waypointIndex == 0) waypointIndex -= 1;
                                    }
                                }
                            }
                        }
                    }
                }

                //check if any of the collisions is hostile
                if (System.Array.IndexOf(vision.hostileTags, hit.tag) >= 0) {
                    Vector3 npcDir = transform.position + new Vector3(0f, vision.sightLevel, 0f);
                    Vector3 targetDir = hit.transform.position - npcDir;
                
                    debugPlayerDir = npcDir;
                    debugTargetDir = targetDir;
                    
                    //check if enemy is within vision through raycast and ignore layers
                    RaycastHit[] multiHitCast;
                    int ignoredLayerMasks;
                    
                    if (state != State.attack) {
                        ignoredLayerMasks = attackState.layersToIgnore;
                        multiHitCast = Physics.RaycastAll(npcDir, targetDir, Mathf.Infinity, ~ignoredLayerMasks);
                    }else{
                        if (attackState.coverShooterOptions.coverShooter) ignoredLayerMasks = attackState.layersToIgnore | attackState.coverShooterOptions.coverLayers;
                        else ignoredLayerMasks = attackState.layersToIgnore;
                        multiHitCast = Physics.RaycastAll(npcDir, targetDir, Mathf.Infinity, ~ignoredLayerMasks);
                    }

                    if (multiHitCast.Length > 0) {
                        System.Array.Sort(multiHitCast, (x,y) => x.distance.CompareTo(y.distance));

                        List<RaycastHit> orderedHitList = new List<RaycastHit>(multiHitCast);
                        int hitsMax = orderedHitList.Count;
                        
                        if (hitsMax > 0) {

                            for (int i=0; i<hitsMax; i++) {
                                if (tagsToAvoid.Contains(orderedHitList[i].transform.tag)) {
                                    orderedHitList.Remove(orderedHitList[i]);
                                }
                            }

                            GameObject suspect = orderedHitList[0].transform.gameObject;
                            Vector3 suspectDir = suspect.transform.position - npcDir;
                            float suspectHeight = suspect.transform.position.y - (visionT.position.y + vision.sightLevel);
                            
                            if (suspectHeight <= vision.sightLevel + vision.maxSightLevel) {
                                if (System.Array.IndexOf(vision.hostileTags, suspect.transform.tag) >= 0) {
                                    RaycastHit lastHit;
                                    if (angle < 180f) {
                                        //check if within cone of vision (width)
                                        if (Vector3.Angle(suspectDir, visionT.forward) <= (angle * 0.5f)) {
                                            if (Physics.Raycast(transform.position + new Vector3(0f, vision.sightLevel, 0f), suspectDir, out lastHit, Mathf.Infinity, ~ignoredLayerMasks)) {
                                                if (lastHit.transform.gameObject == suspect) enemiesToAttack.Add(suspect);
                                            }
                                        }
                                    }else{
                                        if (Vector3.Angle(suspectDir, visionT.forward) <= (angle * 0.5f)) {
                                            if (Physics.Raycast(transform.position + new Vector3(0f, vision.sightLevel, 0f), suspectDir, out lastHit, Mathf.Infinity, ~ignoredLayerMasks)) {
                                                if (lastHit.transform.gameObject == suspect) enemiesToAttack.Add(suspect);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //target the enemy of least distance
            float minDistance = Mathf.Infinity;
            float currentDistance;

            if (enemiesToAttack.Count > 0) {

                foreach (GameObject enemy in enemiesToAttack)
                {
                    currentDistance = (enemy.transform.position - transform.position).sqrMagnitude;
                    if (currentDistance < minDistance * minDistance) {
                        minDistance = currentDistance;
                        enemyToAttack = enemy;
                        targetedEnemyInCover = enemy;
                    }
                }

                captureEnemyTimeStamp = Time.time;  //make a timestamp
                enemyInSight = true;

                if (!coverLocationSet) endPoint = ValidateEnemyYPoint(enemyToAttack.transform.position);
                checkEnemyPosition = Vector3.zero;
                enemyPosition = endPoint;
                
                if (state != State.attack && !isHit) AttackPreparations();
                Debug.DrawRay(debugPlayerDir, debugTargetDir, Color.red);

            //player has got out of enemy vision (no enemies within vision)
            }else{
                if (startAttackTimer) return;

                //flag that there is no enemy to attack
                enemyToAttack = null;
                //flag not a single enemy has been found within the sphere
                enemyInSight = false;
                targetingEnemy = false;
            }

        }else{ 
            visionFramesElapsed++; 
        }
    }

    void CollisionFunc(BlazeAI script, GameObject hit) 
    {
        if (state == State.attack) {
            if (!reachedEnd) return;
            if (!attackState.coverShooterOptions.coverShooter) {
                if (enemyToAttack != null) {
                    if (hit != enemyToAttack) {
                        // backup if hit another npc in attack state
                        if ((idleAttack && reachedEnd && !attackBackUp)) {
                            attackState.moveBackwardsDist += 1f;
                            attackState.distanceFromEnemy += 1f;
                            backedUpBy = script;
                            return;
                        }else{ 
                            return; 
                        }
                    }else{
                        return;
                    }
                }else{
                    return;
                }
            }else{
                return;
            }
        }

        // collisions on patrol
        if (tagsToAvoid.Contains(hit.transform.tag))
        {
            if (script != null) {
                if (script.stopPriority < stopPriority) {
                    //if the other npc is already stopped then quit
                    if (script.stop || script.reachedEnd) return;

                    normalStateActive = false;
                    alertStateActive = false;
                    
                    if (state == State.normal) animationManager.PlayAnimationState(normalState.idleAnimationName, normalState.idleAnimationTransition, normalState.useAnimations);
                    if (state == State.alert) animationManager.PlayAnimationState(alertState.idleAnimationName, alertState.idleAnimationTransition, alertState.useAnimations);
                    
                    stop = true;

                    controllerRadius = controller.radius;
                    controller.radius = 0.01f;

                    controllerPosition = controller.center;
                    controller.center = new Vector3(0.5f, controllerPosition.y, controllerPosition.z);
                }
            }
        }
    }

    //detect nearby enemies when in root motion only to stop npc
    void SphereDetection()
    {
        if (sphereDetectionFrame == FRAMES_THRESHOLD) {
            sphereDetectionFrame = 0;
            float radius;
            
            if (attackState.coverShooterOptions.coverShooter) {
                if (state == State.attack) radius = controller.radius + 1f;
                else radius = controller.radius + 1f;
            }else{
                if (state == State.attack) radius = controller.radius + 0.1f;
                else radius = controller.radius + 0.3f;
            }

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
            List<BlazeAI> uniqueScripts = new List<BlazeAI>();
            
            foreach (var hit in hitColliders) {

                // check if touched by hostile
                if (System.Array.IndexOf(vision.hostileTags, hit.transform.tag) >= 0) {
                    enemyToAttack = hit.transform.gameObject;
                    AttackPreparations();
                    return;
                }

                // check if touched by another agent
                var script = hit.GetComponent<BlazeAI>();
                if (script != null) {
                    if (!uniqueScripts.Contains(script) && script != this) uniqueScripts.Add(script);
                }
            }

            if (uniqueScripts.Count > 0) {
                foreach (var script in uniqueScripts) {
                    Vector3 targetDir = script.transform.position - (transform.position + new Vector3(0f, transform.position.y, 0f));
                    if (state == State.attack && attackState.coverShooterOptions.coverShooter) {
                        if (script.backedUpBy != null) if (script.backedUpBy != this) CollisionFunc(script, script.transform.gameObject);
                    }else{
                        if (Vector3.Angle(transform.forward, targetDir) <= (180f * 0.5f)) {
                            CollisionFunc(script, script.transform.gameObject);
                        }
                    }
                }
            }

        } else { sphereDetectionFrame++; }
    }

    #endregion

    #region Audios
    
    //reset and fix the audio patrol timers depending on the state
    void PatrolAudioTimersFix(State state)
    {
        if (state == State.normal) {
            alertState.audioPlayTimer = 0f;
        }

        if (state == State.alert) {
            normalState.audioPlayTimer = 0f;
        }

        if (state == State.attack) {
            normalState.audioPlayTimer = 0f;
            alertState.audioPlayTimer = 0f;
        }
    }

    //play random NPC audio sources of patrols
    public void PlayPatrolAudio()
    {
        if (state == State.normal){
            normalState.PlayRandomPatrolAudio();
        }
    }

    //stop audios of all systems to leave room for a particular one
    public void StopSystemsAudios(string audioSystemNotToStop)
    {
        if (audioSystemNotToStop == "normal") {
            alertState.StopCurrentAudio();
            distractions.StopCurrentAudio();
            vision.StopCurrentAudio();
        }
        else if (audioSystemNotToStop == "alert") {
            normalState.StopCurrentAudio();
            distractions.StopCurrentAudio();
            vision.StopCurrentAudio();
        }
        else if (audioSystemNotToStop == "distraction") {
            normalState.StopCurrentAudio();
            alertState.StopCurrentAudio();
            vision.StopCurrentAudio();
        }
        else if (audioSystemNotToStop == "hit") {
            normalState.StopCurrentAudio();
            alertState.StopCurrentAudio();
            vision.StopCurrentAudio();
            distractions.StopCurrentAudio();
        }
        else if (audioSystemNotToStop == "attack") {
            normalState.StopCurrentAudio();
            alertState.StopCurrentAudio();
            vision.StopCurrentAudio();
            distractions.StopCurrentAudio();
        }
        else if (audioSystemNotToStop == "death") {
            normalState.StopCurrentAudio();
            alertState.StopCurrentAudio();
            vision.StopCurrentAudio();
            distractions.StopCurrentAudio();
            attackState.StopCurrentAudio();
        }
    }

    #endregion

    #region Hits

    //hit this NPC
    public void Hits (GameObject enemy = null)
    {
        if (!enabled) return;
        
        StopAllCoroutines();
        ResetAllFlags();

        StopSystemsAudios("attack");
        StopSystemsAudios("hit");

        hits.PlayAudio();
        isHit = true;

        if (hits.cancelAttackIfHit) shouldAttack = false;
        animationManager.PlayAnimationState(hits.animationName, hits.animationTransition, hits.useAnimation, true);
        
        if (!startCoverTimer) state = State.hit;
        reachedEnd = false;
        
        StartCoroutine(ReturnFromHit(enemy));
    }

    //return to alert state
    IEnumerator ReturnFromHit (GameObject enemyObject = null)
    {
        yield return new WaitForSeconds(hits.hitDuration);
        
        if (startCoverTimer) {
            findCoverFired = false;
            startCoverTimer = false;
            reachedEnd = false;
            isHit = false;
            FindCover(true, coverObject);
            if (CoverOccupied(coverObject)) shouldAttack = true;
            yield break;
        }

        if (coverLocationSet) {
            state = State.attack;
            attackStateActive = true;
            isHit = false;
            yield break;
        }
        
        if (enemyObject == null) {
            state = State.alert;
            normalStateActive = false;
            
            attackStateActive = false;
            alertStateActive = true;

            waypointInterrupted = true;
            StateWalk();
        }else{
            NavMeshHit hit;
            if (NavMesh.SamplePosition(ValidateEnemyYPoint(enemyObject.transform.position), out hit, navmeshAgent.height * 2f - 1f, NavMesh.AllAreas))
            {
                normalStateActive = false;
                alertStateActive = false;

                wpIdleTriggered = true;
                captureEnemyTimeStamp = Time.time;
                
                endPoint = hit.position;
                checkEnemyPosition = endPoint;

                SetupPath(endPoint);
                TurnToAttack();
            }else{
                state = State.alert;
                normalStateActive = false;
                
                attackStateActive = false;
                alertStateActive = true;

                waypointInterrupted = true;
                StateWalk();
            }
        }
        
        isHit = false;
    }

    #endregion

    #region Death
    
    //kill the NPC
    public void Death()
    {
        if (!enabled) return;

        alertState.DisableScripts();
        normalState.DisableScripts();
        distractions.DisableScript();
        attackState.DisableScript();

        animationManager.PlayAnimationState(death.animationName, death.animationTransition, death.useAnimation);
        
        StopSystemsAudios("death");
        death.PlayAudio();
        
        agentObstacle.enabled = false;
        controller.enabled = false;
        navmeshAgent.enabled = false;
        capsuleCollider.enabled = false;

        death.TriggerScript();
        enabled = false;
    }

    #endregion

    #region Methods Running in Update
    
    //functionalities that require state check in Update
    void FlaggedFunctions()
    {
        // hard rotate to end point to avoid orbits
        if (cornerQueue.Count <= 0 && !reachedEnd && (state == State.normal || state == State.alert)) {
            if (!navmeshAgent.enabled) {
                float dist = (new Vector3(currentDestination.x, transform.position.y, currentDestination.z) - new Vector3(pathFindingProxy.position.x, transform.position.y, pathFindingProxy.position.z)).sqrMagnitude;
                if (dist <= 3f * 3f) {
                    rotateToTarget(currentDestination, 10f);
                }
            }
        }

        //turn to distraction when flagged
        if (distractionTurn) rotateToTarget(distractionPosition, distractions.turnSpeed);
        
        // waypoint rotation
        if (startWaypointRotation) {
            
            waypointRotationAnimationTimer += Time.deltaTime;

            //play animation
            if (waypointRotationAnimationTimer >= waypoints.timeBeforeTurning) {

                //turn right
                if (waypointTurnDir == 1) {
                    if (state == State.normal)
                        animationManager.PlayAnimationState(distractions.rightTurnAnimationNameNormal, distractions.rightTurnTransitionNormal);
                    else
                        animationManager.PlayAnimationState(distractions.rightTurnAnimationNameAlert, distractions.rightTurnTransitionAlert);
                }

                //turn left
                if (waypointTurnDir == -1) {
                    if (state == State.normal)
                        animationManager.PlayAnimationState(distractions.leftTurnAnimationNameNormal, distractions.leftTurnTransitionNormal);
                    else
                        animationManager.PlayAnimationState(distractions.leftTurnAnimationNameAlert, distractions.leftTurnTransitionAlert);
                }

                float[] tempArr = new float[2];
                tempArr[0] = waypoints.waypointsRotation[waypointIndex].x;
                tempArr[1] = waypoints.waypointsRotation[waypointIndex].y;

                rotateToWaypoint(tempArr, waypoints.turnSpeed);
                Vector3 toOther = (new Vector3(transform.position.x + waypoints.waypointsRotation[waypointIndex].x, transform.position.y, transform.position.z + waypoints.waypointsRotation[waypointIndex].y) - transform.position).normalized;
                float dotProd = Vector3.Dot(toOther, transform.forward);
                
                if (dotProd >= 0.90f) {
                    waypointRotationTimer += Time.deltaTime;
                    if (waypointRotationTimer >= 0.2f) {

                        startWaypointRotation = false;
                        waypointRotationTimer = 0f;
                        waypointRotationAnimationTimer = 0f;
                        
                        if (state == State.normal) {
                            StartCoroutine(NormalStateIdle());
                        }else if (state == State.alert) {
                            StartCoroutine(AlertStateIdle());
                        }
                    }
                }
            }
        }

        //distraction turn to look
        if (distractions.waitBeforeTurn) {
            
            distractions._waitBeforeTurnInterval += Time.deltaTime;
            if(distractions._waitBeforeTurnInterval >= distractions.turnReactionTime){
                
                distractions.waitBeforeTurn = false;
                distractions._waitBeforeTurnInterval = 0f;
            
                if (distractions.autoTurn) {
                    //false for don't skip auto turn
                    StartCoroutine(WaitBeforeTurn(distractions.turnReactionTime, false));
                }else{
                    //true to skip auto turn
                    StartCoroutine(WaitBeforeTurn(distractions.turnReactionTime, true));
                }
            }
        }
        
        //if gravity is enabled, trigger gravity method
        if (enableGravity) KeepToGravity();

        //count the duration of surprised state
        if (attackState.surprised.startSurprisedTimerState) {
            attackState.surprised.startSurprisedTimer += Time.deltaTime;

            if (attackState.surprised.alwaysRotate) rotateToTarget(enemyToAttack.transform.position, 5f);
            if (attackState.surprised.startSurprisedTimer >= attackState.surprised.surprisedDuration)
            {
                TurnToAttack();
            }
        }

        //timer for attack duration and backing up
        if (startAttackTimer) {
            attackTimer += Time.deltaTime;
            if (attackState.coverShooterOptions.coverShooter || attackState.alwaysLookAtEnemy) rotateToTarget(enemyToAttack.transform.position, 10f);
            if (attackTimer >= attackState.currentAttackTime) {
                StopAttack();
            }
        }

        //stop NPC for one second if touches another NPC
        if (stop) {
            stopTimer += Time.deltaTime;
            if (stopTimer >= 1f) {
                stop = false;

                controller.radius = controllerRadius;
                controller.center = controllerPosition;
                
                if (state == State.normal) normalStateActive = true;
                if (state == State.alert) alertStateActive = true;

                stopTimer = 0f;
            }
        }

        //the timer for ranged npc to attack repeatedly
        if (startIntervalTimer) {
            intervalTimer += Time.deltaTime;
            if (intervalTimer >= attackState.attackInIntervalsTime) {
                startIntervalTimer = false;
                intervalTimer = 0f;

                shouldAttack = true;
                attackBackUp = false;
                backupPass = 0;

                waitFrameRan = false;
                wpIdleTriggered = false;

                StartCoroutine(WaitFrameSetup());
            }
        }

        //fix and set the audio patrol timers
        PatrolAudioTimersFix(state);

        //if in alert mode will return to normal mode (if set so)
        ReturnNormalTimer();

        //when in attack state and idle-attack not triggered and not attacking
        //then turn agent on
        if (attackState.coverShooterOptions.coverShooter) {
            if (coverBlockingAttack && state == State.attack) StartCoroutine(EnableAgent());
            else {
                if (state == State.attack && !idleAttack && !startAttackTimer) StartCoroutine(EnableAgent());
                else StartCoroutine(DisableAgent());
            }
        }else{
            if (state == State.attack && !attackBackUp && !idleAttack && !shouldAttack) {
                StartCoroutine(EnableAgent());
            }else{
                StartCoroutine(DisableAgent());
            }
        }

        //avoid having agent enabled when state isn't attack
        if (isAgent && state != State.attack) {
            StartCoroutine(disableAgentCoroutine);
        }

        // if (useRootMotion) SphereDetection();
        SphereDetection();
        
        //always turn off attack flag after two seconds if there's no enemy
        if (enemyToAttack == null) {
            emptyEnemyTimer += Time.deltaTime;
            if (emptyEnemyTimer >= 2f) {
                emptyEnemyTimer = 0f;
                shouldAttack = false;
            }
        }else{
            emptyEnemyTimer = 0f;
        }

        //always reset the slow move if not either alert or normal state
        if (state == State.attack || state == State.hit) {
            slowMove = false;
            slowMoveTimer = 0f;
        }

        //flags to reset if attack state
        if (state == State.attack) {
            goingToDistractionPoint = false;
            goingToVisionAlertTag = false;
        }

        //rotate npc if there is no path in order to find a path around
        if (forceTurn && state != State.attack) {
            pathFindingProxy.transform.SetParent(transform);
            transform.Rotate(0f, 100f * Time.deltaTime, 0f);
            
            if (state == State.normal) {
                animationManager.PlayAnimationState(distractions.rightTurnAnimationNameNormal, distractions.rightTurnTransitionNormal);
            }

            if (state == State.alert) {
                animationManager.PlayAnimationState(distractions.rightTurnAnimationNameAlert, distractions.rightTurnTransitionAlert);
            }

            bool pathValidation = NavMesh.CalculatePath(ValidateEnemyYPoint(pathFindingProxy.position), ValidateEnemyYPoint(transform.position + (transform.forward * 5f)), NavMesh.AllAreas, path);
            
            if (pathValidation) {
                if (!useRootMotion) pathFindingProxy.transform.SetParent(transform.parent);
                
                forceTurn = false;
                StateWalk();
            }
        }
        
        //limit the state walk method running to avoid stack overflow
        if (stateWalkTimerRun) {
            stateWalkTimer += Time.deltaTime;
            if (stateWalkTimer >= 0.1f) {
                stateWalkTimer = 0f;
                stateWalkTimerRun = false;
            }
        }

        //start waiting in cover
        if (startCoverTimer) {
            coverTimer += Time.deltaTime;
            
            //rotate to match normal
            if (attackState.coverShooterOptions.coverAnimations.rotateToNormal) {
                pathFindingProxy.transform.SetParent(transform);
                Quaternion rot = Quaternion.FromToRotation(Vector3.forward, coverNormal);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 300f * Time.deltaTime);
            }
            
            if (enemyToAttack) {
                if (coverTimer >= 0.23f) {
                    RaycastHit hit;
                    Vector3 dir = enemyToAttack.transform.position - transform.position;
                    
                    //check if cover is blown
                    if (Physics.Raycast(transform.position, dir, out hit, Mathf.Infinity, ~attackState.layersToIgnore)) {
                        if (System.Array.IndexOf(vision.hostileTags, hit.transform.tag) >= 0) {
                            
                            if (attackState.coverShooterOptions.coverBlownState == CoverShooterOptions.CoverBlownState.AlwaysAttack) FromCoverAttack();
                            else if (attackState.coverShooterOptions.coverBlownState == CoverShooterOptions.CoverBlownState.TakeCover) FindCover(true, coverObject);
                            else {
                                int chance = Random.Range(1, 3);
                                if (chance == 1) FromCoverAttack();
                                else FindCover(true, coverObject);
                            }
                            
                        }
                    }
                }

            }else{
                if (!coveringAway) {
                    startCoverTimer = false;
                    coverTimer = 0f;
                    FromAttackStateReturnAlert();
                }
            }

            if (coverTimer >= actualCoverTime) FromCoverAttack();

        }else{
            if (attackState.coverShooterOptions.coverShooter) {
                attackState.coverShooterOptions.coverAnimations.DisableCoverScripts();
            }
        }

        //wait a few before taking cover
        if (takeCoverDelay) {
            takeCoverDelayTimer += Time.deltaTime;
            if (takeCoverDelayTimer >= 0.23f) TakeCover();
        }

        if (attackState.coverShooterOptions.coverShooter && shouldAttack && getEnemyCover) {

            int maxColliders = 1;
            Collider[] coverCols = new Collider[maxColliders];
            Collider enemyCol = null;

            if (enemyToAttack) enemyCol = enemyToAttack.GetComponent<Collider>();

            if (enemyCol == null) {
                enemyCover = null;
            }else{
                int numColliders = Physics.OverlapSphereNonAlloc(enemyToAttack.transform.position, enemyCol.bounds.size.x + enemyCol.bounds.size.z + 0.2f, coverCols, attackState.coverShooterOptions.coverLayers);

                if (coverCols[0]) {
                    if (coverObject != null) {
                        if (coverObject != coverCols[0].gameObject) enemyCover = coverCols[0].gameObject;
                    }
                    else enemyCover = null;
                } 
                else {
                    enemyCover = null;
                }
            }
        }
    }

    //this function updates properties from this main script to the other classes
    void MainToClassesUpdate()
    {
        if (attackState.surprised.startSurprisedTimerState) distractions.inAttack = true;
        else distractions.inAttack = attackStateActive;
    }

    #endregion
    

    // validate the states on inspector change
    // can only use one state on start
    void StatesInspectorValidation()
    {
        if (alertState == null || normalState == null) return;

        if (!alertState.useAlertStateOnStart && !normalState.useNormalStateOnStart) {
            normalState.useNormalStateOnStart = true;
            alertStateActive = false;
        }

        if (alertState.useAlertStateOnStart && normalState.useNormalStateOnStart) {
            alertState.useAlertStateOnStart = !alertState.inspectorStateOfStart;
            normalState.useNormalStateOnStart = !normalState.inspectorStateOfStart;
        }

        normalState.inspectorStateOfStart = normalState.useNormalStateOnStart;
        alertState.inspectorStateOfStart = alertState.useAlertStateOnStart;
    }
    
    // change state coming from another NPC
    public void ChangeState(string stringState, bool goToVisionAlertTag = false)
    {
        if (stringState == "alert")
        {
            state = State.alert;
            normalStateActive = false;
            alertStateActive = true;
        }

        if (goToVisionAlertTag) {
            StopAllCoroutines();
            endPoint = checkEnemyPosition;
            reachedEnd = false;
            goingToVisionAlertTag = true;
        }
    }

    // disable all scripts on game start
    void DisableAllSystemScripts()
    {
        normalState.DisableScripts();
        alertState.DisableScripts();
        distractions.DisableScript();
        death.DisableScript();
        attackState.DisableScript();
        attackState.coverShooterOptions.coverAnimations.DisableCoverScripts();
    }

    // reset all the flags
    void ResetAllFlags()
    {
        distracted = false;
        goingToDistractionPoint = false;
        distractionTurn = false;
        wpRandomMode = false;
        activateRay = false;
        isSeenVisionAlertTags = false;
        waitFrameRan = false;
        goingToVisionAlertTag = false;
        startWaypointRotation = false;
        waypointRotationTimer = 0f;
        waypointRotationAnimationTimer = 0f;
    }

    // load the profile properties
    public void LoadProfile(BlazeProfile profile)
    {
        if (profile == null) return;

        groundLayers = profile.groundLayers;
        pathRecalculationRate = profile.pathRecalculationRate;
        proxyOffset = profile.proxyOffset;
        enableGravity = profile.enableGravity;
        useRootMotion = profile.useRootMotion;

        avoidFacingObstacles = profile.avoidFacingObstacles;
        obstacleRayDistance = profile.obstacleRayDistance;
        obstacleRayOffset = profile.obstacleRayOffset;
        obstacleLayers = profile.obstacleLayers;

        tagsToAvoid = new List<string>(profile.tagsToAvoid);
        waypoints.instantMoveAtStart = profile.waypoints.instantMoveAtStart;
        waypoints.loop = profile.waypoints.loop;
        waypoints.randomize = profile.waypoints.randomize;
        
        vision.alertTags = new Vision.AlertOptions[profile.vision.alertTags.Length];
        for (var i=0; i<vision.alertTags.Length; i++) {
            vision.alertTags[i].tag = profile.vision.alertTags[i].tag;
            vision.alertTags[i].fallBackTag = profile.vision.alertTags[i].fallBackTag;
            vision.alertTags[i].animationName = profile.vision.alertTags[i].animationName;
            vision.alertTags[i].time = profile.vision.alertTags[i].time;
            vision.alertTags[i].moveToLocation = profile.vision.alertTags[i].moveToLocation;
            vision.alertTags[i].callOthersToLocation = profile.vision.alertTags[i].callOthersToLocation;
        }
        vision.hostileTags = profile.vision.hostileTags;
        vision.visionDuringNormalState =  new Vision.normalVision(profile.vision.visionDuringNormalState.coneAngle, profile.vision.visionDuringNormalState.sightRange);
        vision.visionDuringAlertState =  new Vision.alertVision(profile.vision.visionDuringAlertState.coneAngle, profile.vision.visionDuringAlertState.sightRange);
        vision.visionDuringAttackState =  new Vision.attackVision(profile.vision.visionDuringAttackState.coneAngle, profile.vision.visionDuringAttackState.sightRange);
        vision.sightLevel = profile.vision.sightLevel;
        vision.maxSightLevel = profile.vision.maxSightLevel;

        normalState.moveSpeed = profile.normalState.moveSpeed;
        normalState.rotationSpeed = profile.normalState.rotationSpeed;
        normalState.waitTime = profile.normalState.waitTime;
        normalState.randomizeWaitTime = profile.normalState.randomizeWaitTime;
        normalState.randomizeWaitTimeBetween = profile.normalState.randomizeWaitTimeBetween;
        normalState.useAnimations = profile.normalState.useAnimations;
        normalState.idleAnimationName = profile.normalState.idleAnimationName;
        normalState.idleAnimationTransition = profile.normalState.idleAnimationTransition;
        normalState.moveAnimationName = profile.normalState.moveAnimationName;
        normalState.moveAnimationTransition = profile.normalState.moveAnimationTransition;
        normalState.useRandomAnimationsOnIdle = profile.normalState.useRandomAnimationsOnIdle;
        normalState.randomIdleAnimationNames = profile.normalState.randomIdleAnimationNames;
        normalState.randomIdleAnimationTransition = profile.normalState.randomIdleAnimationTransition;
        normalState.enableScripts = profile.normalState.enableScripts;
        normalState.playAudiosOnPatrol = profile.normalState.playAudiosOnPatrol;
        normalState.playAudioEvery = profile.normalState.playAudioEvery;

        alertState.useAlertStateOnStart = profile.alertState.useAlertStateOnStart;
        alertState.alertOthers = profile.alertState.alertOthers;
        alertState.alertRadius = profile.alertState.alertRadius;
        alertState.tagsToAlert = profile.alertState.tagsToAlert;
        alertState.receiveAlertFromOthers = profile.alertState.receiveAlertFromOthers;
        alertState.moveSpeed = profile.alertState.moveSpeed;
        alertState.rotationSpeed = profile.alertState.rotationSpeed;
        alertState.waitTime = profile.alertState.waitTime;
        alertState.randomizeWaitTime = profile.alertState.randomizeWaitTime;
        alertState.randomizeWaitTimeBetween = profile.alertState.randomizeWaitTimeBetween;
        alertState.useAnimations = profile.alertState.useAnimations;
        alertState.idleAnimationName = profile.alertState.idleAnimationName;
        alertState.idleAnimationTransition = profile.alertState.idleAnimationTransition;
        alertState.moveAnimationName = profile.alertState.moveAnimationName;
        alertState.moveAnimationTransition = profile.alertState.moveAnimationTransition;
        alertState.useRandomAnimationsOnIdle = profile.alertState.useRandomAnimationsOnIdle;
        alertState.randomIdleAnimationNames = profile.alertState.randomIdleAnimationNames;
        alertState.randomIdleAnimationTransition = profile.alertState.randomIdleAnimationTransition;
        alertState.returnToNormalState = profile.alertState.returnToNormalState;
        alertState.timeBeforeReturningNormal = profile.alertState.timeBeforeReturningNormal;
        alertState.useAnimationOnReturn = profile.alertState.useAnimationOnReturn;
        alertState.animationNameOnReturn = profile.alertState.animationNameOnReturn;
        alertState.animationOnReturnTransition = profile.alertState.animationOnReturnTransition;
        alertState.playAudioOnReturn = profile.alertState.playAudioOnReturn;
        alertState.enableScripts = profile.alertState.enableScripts;
        alertState.playAudiosOnPatrol = profile.alertState.playAudiosOnPatrol;
        alertState.playAudioEvery = profile.alertState.playAudioEvery;

        attackState.coverShooterOptions.coverShooter = profile.attackState.coverShooterOptions.coverShooter;
        attackState.coverShooterOptions.coverLayers = profile.attackState.coverShooterOptions.coverLayers;
        attackState.coverShooterOptions.hideSensitivity = profile.attackState.coverShooterOptions.hideSensitivity;
        attackState.coverShooterOptions.searchDistance = profile.attackState.coverShooterOptions.searchDistance;
        attackState.coverShooterOptions.minObstacleHeight = profile.attackState.coverShooterOptions.minObstacleHeight;
        attackState.coverShooterOptions.moveToCoverCenter = profile.attackState.coverShooterOptions.moveToCoverCenter;
        attackState.coverShooterOptions.firstSightChance = (CoverShooterOptions.FirstSightChance)((int)profile.attackState.coverShooterOptions.firstSightChance);
        attackState.coverShooterOptions.coverBlownState = (CoverShooterOptions.CoverBlownState) ((int)profile.attackState.coverShooterOptions.coverBlownState);
        attackState.coverShooterOptions.attackEnemyCover = (CoverShooterOptions.AttackEnemyCover) ((int)profile.attackState.coverShooterOptions.attackEnemyCover);
        attackState.coverShooterOptions.coverAnimations.highCoverHeight = profile.attackState.coverShooterOptions.coverAnimations.highCoverHeight;
        attackState.coverShooterOptions.coverAnimations.highCoverAnimation = profile.attackState.coverShooterOptions.coverAnimations.highCoverAnimation;
        attackState.coverShooterOptions.coverAnimations.lowCoverHeight = profile.attackState.coverShooterOptions.coverAnimations.lowCoverHeight;
        attackState.coverShooterOptions.coverAnimations.lowCoverAnimation = profile.attackState.coverShooterOptions.coverAnimations.lowCoverAnimation;
        attackState.coverShooterOptions.coverAnimations.rotateToNormal = profile.attackState.coverShooterOptions.coverAnimations.rotateToNormal;
        attackState.coverShooterOptions.coverAnimations.useScripts = profile.attackState.coverShooterOptions.coverAnimations.useScripts;
        attackState.coverShooterOptions.coverAnimationTransition = profile.attackState.coverShooterOptions.coverAnimationTransition;
        attackState.distanceFromEnemy = profile.attackState.distanceFromEnemy;
        attackState.attackDistance = profile.attackState.attackDistance;
        attackState.alertOthersTime = profile.attackState.alertOthersTime;
        attackState.layersToIgnore = profile.attackState.layersToIgnore;
        attackState.timeToReturnAlert = profile.attackState.timeToReturnAlert;
        attackState.attackInIntervals = profile.attackState.attackInIntervals;
        attackState.attackInIntervalsTime = profile.attackState.attackInIntervalsTime;
        attackState.randomizeAttackIntervals = profile.attackState.randomizeAttackIntervals;
        attackState.randomizeAttackIntervalsBetween = profile.attackState.randomizeAttackIntervalsBetween;
        attackState.moveBackwards = profile.attackState.moveBackwards;
        attackState.moveBackwardsDist = profile.attackState.moveBackwardsDist;
        attackState.moveBackwardsAttack = profile.attackState.moveBackwardsAttack;
        attackState.moveSpeed = profile.attackState.moveSpeed;
        attackState.moveBackwardsSpeed = profile.attackState.moveBackwardsSpeed;
        attackState.useAnimations = profile.attackState.useAnimations;
        attackState.idleAnimationName = profile.attackState.idleAnimationName;
        attackState.idleAnimationTransition = profile.attackState.idleAnimationTransition;
        attackState.moveForwardAnimationName = profile.attackState.moveForwardAnimationName;
        attackState.moveForwardAnimationTransition = profile.attackState.moveForwardAnimationTransition;
        attackState.moveBackwardsAnimationName = profile.attackState.moveBackwardsAnimationName;
        attackState.moveBackwardsAnimationTransition = profile.attackState.moveBackwardsAnimationTransition;
        attackState.moveBackwardsAttackAnimationName = profile.attackState.moveBackwardsAttackAnimationName;
        attackState.moveBackwardsAttackAnimationTransition = profile.attackState.moveBackwardsAttackAnimationTransition;
        attackState.attackAnimations = profile.attackState.attackAnimations;
        attackState.alwaysLookAtEnemy = profile.attackState.alwaysLookAtEnemy;
        attackState.attackDuration = profile.attackState.attackDuration;
        attackState.attackAnimationTransition = profile.attackState.attackAnimationTransition;
        attackState.useAudio = profile.attackState.useAudio;
        attackState.surprised.useSurprised = profile.attackState.surprised.useSurprised;
        attackState.surprised.surprisedDuration = profile.attackState.surprised.surprisedDuration;
        attackState.surprised.alwaysRotate = profile.attackState.surprised.alwaysRotate;
        attackState.surprised.useAnimations = profile.attackState.surprised.useAnimations;
        attackState.surprised.surprisedAnimationName = profile.attackState.surprised.surprisedAnimationName;
        attackState.surprised.surprisedAnimationTransition = profile.attackState.surprised.surprisedAnimationTransition;
        attackState.surprised.useAudio = profile.attackState.surprised.useAudio;

        distractions.alwaysUse = profile.distractions.alwaysUse;
        distractions.autoTurn = profile.distractions.autoTurn;
        distractions.turnSpeed = profile.distractions.turnSpeed;
        distractions.turnReactionTime = profile.distractions.turnReactionTime;
        distractions.turnAlertOnDistraction = profile.distractions.turnAlertOnDistraction;
        distractions.moveToDistractionLocation = profile.distractions.moveToDistractionLocation;
        distractions.checkDistractionPriorityLevel = profile.distractions.checkDistractionPriorityLevel;
        distractions.moveToDistractionReactTime = profile.distractions.moveToDistractionReactTime;
        distractions.checkingTime = profile.distractions.checkingTime;
        distractions.useTurnAnimations = profile.distractions.useTurnAnimations;
        distractions.rightTurnAnimationNameNormal = profile.distractions.rightTurnAnimationNameNormal;
        distractions.rightTurnTransitionNormal = profile.distractions.rightTurnTransitionNormal;
        distractions.leftTurnAnimationNameNormal = profile.distractions.leftTurnAnimationNameNormal;
        distractions.leftTurnTransitionNormal = profile.distractions.leftTurnTransitionNormal;
        distractions.rightTurnAnimationNameAlert = profile.distractions.rightTurnAnimationNameAlert;
        distractions.rightTurnTransitionAlert = profile.distractions.rightTurnTransitionAlert;
        distractions.leftTurnAnimationNameAlert = profile.distractions.leftTurnAnimationNameAlert;
        distractions.leftTurnTransitionAlert = profile.distractions.leftTurnTransitionAlert;
        distractions.distractionCheckAnimation = profile.distractions.distractionCheckAnimation;
        distractions.distractionCheckAnimationName = profile.distractions.distractionCheckAnimationName;
        distractions.distractionCheckTransition = profile.distractions.distractionCheckTransition;
        distractions.enableScript = profile.distractions.enableScript;
        distractions.playAudios = profile.distractions.playAudios;
        distractions.playDistractionSearchAudio = profile.distractions.playDistractionSearchAudio;
    
        hits.hitDuration = profile.hits.hitDuration;
        hits.cancelAttackIfHit = profile.hits.cancelAttackIfHit;
        hits.useAnimation = profile.hits.useAnimation;
        hits.animationName = profile.hits.animationName;
        hits.animationTransition = profile.hits.animationTransition;
        hits.useAudios = profile.hits.useAudios;

        death.useAnimation = profile.death.useAnimation;
        death.animationName = profile.death.animationName;
        death.animationTransition = profile.death.animationTransition;
        death.useAudio = profile.death.useAudio;
        death.enableScript = profile.death.enableScript;
    }

    // validate properties on start
    void ValidateProperties()
    {
        if (attackState.attackDistance < 0.1f) attackState.attackDistance = 0.1f;
    }
}