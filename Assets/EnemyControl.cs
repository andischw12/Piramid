using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyControl : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;               //  Nav mesh agent component
    public float startWaitTime = 4;                 //  Wait time of every action
    public float timeToRotate = 2;                  //  Wait time when the enemy detect near the player without seeing
    public float speedWalk = 6;                     //  Walking speed, speed in the nav mesh agent
    public float speedRun = 9;                      //  Running speed
    public int life;
    public float viewRadius = 15;                   //  Radius of the enemy view
    public float viewAngle = 90;                    //  Angle of the enemy view
    public LayerMask playerMask;                    //  To detect the player with the raycast
    public LayerMask obstacleMask;                  //  To detect the obstacules with the raycast
    public float meshResolution = 1.0f;             //  How many rays will cast per degree
    public int edgeIterations = 4;                  //  Number of iterations to get a better performance of the mesh filter when the raycast hit an obstacule
    public float edgeDistance = 0.5f;               //  Max distance to calcule the a minumun and a maximum raycast when hits something
    public float distanceToAttack;
    public bool GurdFirstPoint;
    public Transform[] waypoints;                   //  All the waypoints where the enemy patrols
    int m_CurrentWaypointIndex;                     //  Current waypoint where the enemy is going to

    Vector3 playerLastPosition = Vector3.zero;      //  Last position of the player when was near the enemy
    Vector3 m_PlayerPosition;                       //  Last position of the player when the player is seen by the enemy

    float m_WaitTime;                               //  Variable of the wait time that makes the delay
    float m_TimeToRotate;                           //  Variable of the wait time to rotate when the player is near that makes the delay
    bool m_playerInRange;                           //  If the player is in range of vision, state of chasing
    bool m_PlayerNear;                              //  If the player is near, state of hearing
    public bool m_IsPatrol;                                //  If the enemy is patrol, state of patroling
    bool m_CaughtPlayer;                            //  if the enemy has caught the player
    public GameObject Player;
    Vector3 previousPosition;
    float curSpeed = 0;
    public float BlendAnimationMult = 1.75f;
    public bool IsCollidingWithPlayer;
    public int DamageVal ;
    public bool CanEnemyAttack;
    public float LookAtSpeed;

    void Start()
    {
        print("start");
        m_PlayerPosition = Vector3.zero;
        m_IsPatrol = true;
        m_CaughtPlayer = false;
        m_playerInRange = false;
        m_PlayerNear = false;
        m_WaitTime = startWaitTime;                 //  Set the wait time variable that will change
        m_TimeToRotate = timeToRotate;
        previousPosition = transform.position;
        m_CurrentWaypointIndex = 0;                 //  Set the initial waypoint
        navMeshAgent = GetComponent<NavMeshAgent>();
        
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speedWalk;             //  Set the navemesh speed with the normal speed of the enemy
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);    //  Set the destination to the first waypoint
        Move(speedWalk);
        Player = FindObjectOfType<PlayerManager>().gameObject;

    }




    private void Update()
    {
        EnviromentView();                       //  Check whether or not the player is in the enemy's field of vision

        if (!m_IsPatrol)
        {
            Chasing();
        }
        else
        {
            Patroling();
        }

         
        if ( Vector3.Distance(Player.transform.position, transform.position) < distanceToAttack) 
        {
            
            Quaternion lookOnLook = Quaternion.LookRotation(Player.transform.position - transform.position);
             

            transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, Time.deltaTime);


           // transform.LookAt(Player.transform);
            if (CanEnemyAttack)
                StartCoroutine(Attack());
            else
                Stop();
        }

       


        SetAnimation();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerManager>())
            IsCollidingWithPlayer = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerManager>())
            IsCollidingWithPlayer = false;
    }

    IEnumerator Attack()
    {
        CanEnemyAttack = false;
        navMeshAgent.isStopped = true;
        GetComponent<Animator>().SetTrigger("Attack");
        
        yield return new WaitForSeconds(2);
        navMeshAgent.isStopped = false;
        
        
        CanEnemyAttack = true;

    }

    public void TakeDamage(int d) 
    {
        life -=d;
        if (life <= 0)
            Die();
    }

    void Die() 
    {
        Destroy(this.gameObject);
    }

    public void OnHit()
    {
        if (IsCollidingWithPlayer)
        {
            PlayerManager.instance.ChangeHealth(-DamageVal);
          
            print("attacking");
        }
    }

    private void Chasing()
    {
        
        //  The enemy is chasing the player
        m_PlayerNear = false;                       //  Set false that hte player is near beacause the enemy already sees the player
        playerLastPosition = Vector3.zero;          //  Reset the player near position

        if (!m_CaughtPlayer)
        {
            Move(speedRun);
            navMeshAgent.SetDestination(m_PlayerPosition);          //  set the destination of the enemy to the player location
        }
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance ||
            Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 3f )    //  Control if the enemy arrive to the player location
        {
            
            if (m_WaitTime <= 0 && !m_CaughtPlayer && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 3f)
            {
                //  Check if the enemy is not near to the player, returns to patrol after the wait time delay
                returnPatrol();
            }

            //else if (CheckFirstPoint()) { }
            //else
            // {
            //if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f)
            //  Wait if the current position is not the player position
            // Stop();
            // m_WaitTime -= Time.deltaTime;
            else 
            {  
                returnPatrol(); 
            //}
             }
        }
    }
 

    void returnPatrol() 
    {
        m_IsPatrol = true;
        m_PlayerNear = false;
        Move(speedWalk);
        m_TimeToRotate = timeToRotate;
        m_WaitTime = startWaitTime;
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    }

    private void Patroling()
    {
        if (m_PlayerNear)
        {
            //  Check if the enemy detect near the player, so the enemy will move to that position
            if (m_TimeToRotate <= 0)
            {
                Move(speedWalk);
                LookingPlayer(playerLastPosition);
            }
            else
            {
                //  The enemy wait for a moment and then go to the last player position
                Stop();
                //m_TimeToRotate -= Time.deltaTime;
            }
        }
        else
        {
            m_PlayerNear = false;           //  The player is no near when the enemy is platroling
            playerLastPosition = Vector3.zero;
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);    //  Set the enemy destination to the next waypoint
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                //  If the enemy arrives to the waypoint position then wait for a moment and go to the next
                if (m_WaitTime <= 0)
                {
                    NextPoint();
                    Move(speedWalk);
                    m_WaitTime = startWaitTime;
                }
                else
                {
                    Stop();
                     
                    m_WaitTime -= Time.deltaTime;
                }
            }
        }


        
    }

     

    public void NextPoint()
    {
        m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    }

    void Stop()
    {
        StartCoroutine(StopProcess());
        /*
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;

        GetComponent<Animator>().SetBool("Walk", false);
        */
    }
    IEnumerator StopProcess() 
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
        //yield return new WaitUntil(()=>navMeshAgent.remainingDistance < 0.5);
        GetComponent<Animator>().SetBool("Walk", false);
        yield break ;

    }

    void Move(float speed)
    {
       
        navMeshAgent.isStopped = false;
         
        navMeshAgent.speed = speed;
    }
    // Blend Walking and idle animation by current object speed
    void SetAnimation() 
    {
        Vector3 curMove = transform.position - previousPosition;
        curSpeed = curMove.magnitude / Time.deltaTime;
        previousPosition = transform.position;
        GetComponent<Animator>().SetFloat("Blend",curSpeed * BlendAnimationMult);
    }

    void CaughtPlayer()
    {
        m_CaughtPlayer = true;
        print("Caught player");
    }

    void LookingPlayer(Vector3 player)
    {
        navMeshAgent.SetDestination(player);
        if (Vector3.Distance(transform.position, player) <= 0.3)
        {
            if (m_WaitTime <= 0)
            {
                m_PlayerNear = false;
                Move(speedWalk);
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
                m_WaitTime = startWaitTime;
                m_TimeToRotate = timeToRotate;
            }
            else
            {
                Stop();
                m_WaitTime -= Time.deltaTime;
            }
        }
    }

    void EnviromentView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);   //  Make an overlap sphere around the enemy to detect the playermask in the view radius

        for (int i = 0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);          //  Distance of the enmy and the player
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    m_playerInRange = true;             //  The player has been seeing by the enemy and then the nemy starts to chasing the player
                    m_IsPatrol = false;                 //  Change the state to chasing the player
                }
                else
                {
                    /*
                     *  If the player is behind a obstacle the player position will not be registered
                     * */
                    m_playerInRange = false;
                }
            }
            if (Vector3.Distance(transform.position, player.position) > viewRadius)
            {
                /*
                 *  If the player is further than the view radius, then the enemy will no longer keep the player's current position.
                 *  Or the enemy is a safe zone, the enemy will no chase
                 * */
                m_playerInRange = false;                //  Change the sate of chasing
            }
            if (m_playerInRange)
            {
                /*
                 *  If the enemy no longer sees the player, then the enemy will go to the last position that has been registered
                 * */
                m_PlayerPosition = player.transform.position;       //  Save the player's current position if the player is in range of vision
            }
        }
    }
}