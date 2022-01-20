using UnityEngine;

namespace BlazeAISpace
{
    [System.Serializable]
    public class NormalState
    {
        public bool useNormalStateOnStart = true;

        [Header("Movement")]
        [Tooltip("Speed of walking to waypoints")]
        public float moveSpeed = 4f;
        [Tooltip("Speed of rotating")]
        public float rotationSpeed = 2.5f;

        [Header("Timing")]
        [Tooltip("The amount of time to wait before moving to the next waypoint")]
        public float waitTime = 5f;
        [Tooltip("Randomize the wait time between two values at every waypoint other than having a constant fixed value")]
        public bool randomizeWaitTime = false;
        [Tooltip("Randomize the wait time between two values (only works if the above [Randomize Wait Time] is set to true)")]
        public Vector2 randomizeWaitTimeBetween = new Vector2(0f, 0f);

        [Header("Animations")]
        [Tooltip("Set to true to call animation states for idle and walking")]
        public bool useAnimations = false;
        
        [Space(5)]
        [Tooltip("Animation manager will play the animation state with this name when idle")]
        public string idleAnimationName;
        [Tooltip("Transition time from any state to the idle animation")]
        public float idleAnimationTransition = 0.25f;
        
        [Space(5)]
        [Tooltip("Animation manager will play the animation state with this name when moving")]
        public string moveAnimationName;
        [Tooltip("Transition time from any state to the move animation")]
        public float moveAnimationTransition = 0.25f;

        [Space(5)]
        [Tooltip("If enabled, when this NPC is idle in normal state will play a random animation from the list below. For example: checking his gun or cleaning his boots")]
        public bool useRandomAnimationsOnIdle;
        [Tooltip("A list of all animation names you want to play randomly when idle. One will be randomly chosen and played. There is a 50/50 chance of random animations being played")]
        public string[] randomIdleAnimationNames;
        [Tooltip("Transition time from idle to one of the random animations played")]
        public float randomIdleAnimationTransition = 0.4f;

        [Header("Script Enabling")]
        [Tooltip("Set to true to enable/disable your custom scripts when idle (waypoint reached) and walking")]
        public bool enableScripts = false;
        [Tooltip("Your custom script will get enabled when waypoint is reached and disabled when walking. If property is NULL (empty), script enabling will not occur")]
        public MonoBehaviour enableScriptOnIdle;
        [Tooltip("Your custom script will get enabled when walking and disabled when waypoint is reached. If property is NULL (empty), script enabling will not occur")]
        public MonoBehaviour enableScriptOnMoving;

        [Header("Audios")]
        [Tooltip("Choose and play a random audio during patrol. Good for immersion, you can have a guard saying 'I should've stayed home, this is boring'")]
        public bool playAudiosOnPatrol = false;
        [Tooltip("A game object with multiple audio sources and one will be chosen at random and played during patroling waypoints in normal state. If only one is added then that only one will be played")]
        public GameObject patrolAudios;
        [Tooltip("The amount of time in seconds to play a patrol audio each time. It's highly recommended that it's set to atleast 30 seconds and have a big gap between other NPCs infact, not all npcs should have audios enabled")]
        public float playAudioEvery = 30f;

        AudioSource currentAudio = new AudioSource();
        AudioSource[] patrolAudiosArr;

        bool _randomizeWaitTimeState;
        float _waitTimeValue;

        public bool instantMoveChange { get; set; }
        public bool inspectorStateOfStart { get; set; }
        public float audioPlayTimer { get; set; }
        

        //random audio to be played when patroling in alert state
        public void PlayRandomPatrolAudio()
        {
            if (patrolAudios == null || !playAudiosOnPatrol) return;

            patrolAudiosArr = patrolAudios.GetComponents<AudioSource>();
            if (patrolAudiosArr.Length > 1)
            {
                AudioSource temp = patrolAudiosArr[Random.Range(0, patrolAudiosArr.Length)];
                if (temp == currentAudio) {
                    PlayRandomPatrolAudio();
                }else{
                    currentAudio = temp;
                    currentAudio.Play();
                }
            }else{
                if (patrolAudiosArr.Length == 1) {
                    currentAudio = patrolAudiosArr[0];
                    currentAudio.Play();
                }
            }

            audioPlayTimer = 0f;
        }

        //stop patrol audio to play others
        public void StopCurrentAudio()
        {
            if (patrolAudios != null) {
                if (currentAudio != null && currentAudio.isPlaying) currentAudio.Stop();
            }

            audioPlayTimer = 0f;
        }

        //if the instant move at start value is set to true
        //this function is called from the main in order
        //to save the states of the timings and override them
        //to a low number to make the NPC move instantly
        public void InstantMoveChangeVals()
        {
            _randomizeWaitTimeState = randomizeWaitTime;
            _waitTimeValue = waitTime;

            waitTime = 0.2f;
            randomizeWaitTime = false;
            instantMoveChange = true;
        }

        //return the values as they were
        public void InstantMoveReturnsVals()
        {
            waitTime = _waitTimeValue;
            randomizeWaitTime = _randomizeWaitTimeState;
            instantMoveChange = false;
        }

        //enable/disable scripts of either walking or idle
        public void TriggerScripts(string walkingOrIdle)
        {
            if (enableScripts) {
                if (walkingOrIdle == "walking") {
                    //enable/disable scripts again for walking and idle
                    if (enableScriptOnIdle != null) enableScriptOnIdle.enabled = false;
                    if (enableScriptOnMoving != null) enableScriptOnMoving.enabled = true;
                }else{
                    //enable/disable idle and walking scripts
                    if (enableScriptOnMoving != null) enableScriptOnMoving.enabled = false;
                    if (enableScriptOnIdle != null) enableScriptOnIdle.enabled = true;
                }
            }
        }

        //disable both moving and idle scripts
        public void DisableScripts()
        {
            if (enableScriptOnIdle != null) enableScriptOnIdle.enabled = false;
            if (enableScriptOnMoving != null) enableScriptOnMoving.enabled = false;
        }
    }
}