using UnityEngine;

namespace BlazeAISpace
{
    [System.Serializable]
    public class AttackState
    {
        [Header("Attacking Options")]
        [Tooltip("Options for using cover shooter behaviour.")]
        public CoverShooterOptions coverShooterOptions;

        [Space(5)]
        [Tooltip("It's the safe distance between this NPC and the enemy. If cover shooter is enabled, it's the safe distance if there's no cover to hide.")]
        public float distanceFromEnemy = 30;
        [Min(0.1f), Tooltip("The distance between the NPC and the enemy to deliver the actual attack. So for example: if this NPC is to punch the enemy then it needs to get pretty close. This can't be bigger or equal to [Distance From Enemy].")]
        public float attackDistance = 1f;
        [Tooltip("The amount of time in seconds to pass before alerting others when this NPC is in attack state. It's always best to leave a gap of about 2 seconds to give the player a chance to kill the enemy before being compromised by others.")]
        public float alertOthersTime = 2f;
        [Tooltip("Select the layers that will not block the view from this npc when in attack state. This should be the layers of other Blaze AI npcs as you dont want other attacking npcs to block the target from view when they're attacking. Make sure all the child gameobjects of the npcs have these ignorable layers to completely ignore them.")]
        public LayerMask layersToIgnore;
        [Tooltip("The amount of time to transition to alert state from attack state when there is no longer any enemies.")]
        public float timeToReturnAlert = 2f;

        [Space(5)]
        [Tooltip("If set to true, this NPC will not be added to the enemy scheduler, but rather will attack every certain amount of seconds. Best for ranged enemies where you want an NPC to shoot or throw arrows at the player every certain amount of time repeatedly. This will be forced on if cover shooter is enabled.")]
        public bool attackInIntervals = false;
        [Tooltip("The amount of time in seconds for this NPC to attack ~ ONLY READ IF [Attack In Intervals] IS TRUE.")]
        public float attackInIntervalsTime = 0f;
        [Tooltip("If set to true, the [Attack In Intervals Time] will be randomized after each attack ~ ONLY WORKS IF [Attack In Intervals] IS SET TO TRUE.")]
        public bool randomizeAttackIntervals = false;
        [Tooltip("Randomize the attack intervals between these two values (minimum of X is 0.5) ~ ONLY WORKS IF [Randomize Attack Intervals] IS SET TO TRUE.")]
        public Vector2 randomizeAttackIntervalsBetween;
        
        [Space(5)]
        [Tooltip("If the enemy gets too near, this NPC will backup.")]
        public bool moveBackwards;
        [Tooltip("Move backwards if distance is less than this")]
        public float moveBackwardsDist = 3f;
        [Tooltip("When the NPC is backing up it can continue enabling the attack script, like a shooter where getting too close makes it backup while shooting you.")]
        public bool moveBackwardsAttack;

        [Header("Movement")]
        [Tooltip("Movement speed of the NPC when in attack state, the rotation speed is that of alert state.")]
        public float moveSpeed = 5f;
        [Tooltip("Set the speed of moving backwards. When the targeted enemy is too close this NPC will back up.")]
        public float moveBackwardsSpeed = 2f;
        
        [Header("Animations")]
        [Tooltip("Set whether you want to use animations when in attack state or not. If not, the animations used will be those of the alert state.")]
        public bool useAnimations;
        [Space(3)]
        [Tooltip("Animation name to play when this NPC is in idle-attack state (when waiting for it's turn to attack) will ignore if empty!")]
        public string idleAnimationName;
        [Tooltip("The amount of time to transition to the idle-attack animation.")]
        public float idleAnimationTransition = 0.25f;

        [Space(3)]
        [Tooltip("Move forward animation name when this NPC is in attack state and needs to move (when running after player, getting to attack position, running to cover, etc..) will ignore if empty!")]
        public string moveForwardAnimationName;
        [Tooltip("The amount of time to transition to this animation.")]
        public float moveForwardAnimationTransition = 0.25f;

        [Space(3)]
        [Tooltip("Move backwards animation name when this NPC is in attack state and needs to move (backing up when the player gets closer.")]
        public string moveBackwardsAnimationName;
        [Tooltip("The amount of time to transition to this animation.")]
        public float moveBackwardsAnimationTransition = 0.25f;

        [Space(3)]
        [Tooltip("The animation name to play when NPC is backing up and set to attack while moving backwards.")]
        public string moveBackwardsAttackAnimationName;
        [Tooltip("The transition time from the current animation and this one.")]
        public float moveBackwardsAttackAnimationTransition = 0.25f;

        [Header("Attacking")]
        [Tooltip("Animation names to play on attack. One will be chosen at random to play in each attack. If only one is set. Then that one will always be chosen. If empty, will be ignored.")]
        public string[] attackAnimations;
        [Tooltip("Set the attack duration for each attack animation before backing up (attack finishes). This is automatically set according to attack animations array amount.")]
        public float[] attackDuration;
        [Tooltip("Transition time from current animation to the attack animation.")]
        public float attackAnimationTransition = 0.25f;
        [Tooltip("Check whether you always want the npc to keep looking at the enemy on attack. If disabled, npc won't change rotation to enemy direction when attack starts. Will always be enabled on cover shooter.")]
        public bool alwaysLookAtEnemy;
        [Tooltip("Enable script when goes in for the attack in melee and ranged attack types and for cover shooter when about to shoot.")]
        public MonoBehaviour attackScript;

        [Header("Attack Audios")]
        [Tooltip("Play an audio when attacking.")]
        public bool useAudio;
        [Tooltip("Gameobject with audio sources. One will be chosen at random to play on each attack. If only one is set. Then that one will always be chosen. If empty, will be ignored.")]
        public GameObject attackAudios;
        
        [Header("Emotions")]
        [Tooltip("Surprised is when the NPC is in NORMAL STATE and sees an enemy (hostile tag) for the first time. You can play animations like being shocked, scared or something like a battlecry or calling for help.")]
        public Surprised surprised;

        AudioSource currentAudio;
        AudioSource currentAttackAudio;

        public float currentAttackTime { get; set; }
        public string currentAttackAnimation { get; set; }

        
        //play a random audio on attacking
        public void PlayAttackAudio()
        {
            if (useAudio) {

                StopCurrentAudio();
                if (attackAudios == null) return;
                AudioSource[] audios = attackAudios.GetComponents<AudioSource>();
                
                if (audios.Length > 1) {
                    currentAttackAudio = audios[Random.Range(0, audios.Length)];
                    currentAttackAudio.Play();
                }else{
                    if (audios.Length == 1) {
                        currentAttackAudio = audios[0];
                        currentAttackAudio.Play();
                    }
                }
            }
        }

        //play a random audio of surprised emotion
        public void PlaySurprisedAudio()
        {
            if (surprised.useAudio) {
                if (surprised.audiosToPlay == null) return;

                AudioSource[] audios = surprised.audiosToPlay.GetComponents<AudioSource>();
                if(audios.Length > 1) {
                    currentAudio = audios[Random.Range(0, audios.Length)];
                    currentAudio.Play();
                }else{
                    if(audios.Length == 1) {
                        currentAudio = audios[0];
                        currentAudio.Play();
                    }
                }
            }
        }

        //stop the current audios
        public void StopCurrentAudio()
        {
            if (currentAudio != null && currentAudio.isPlaying) currentAudio.Stop();
            if (currentAttackAudio != null && currentAttackAudio.isPlaying) currentAttackAudio.Stop();
        }

        //disable the attack script
        public void DisableScript()
        {
            if (attackScript != null) attackScript.enabled = false;
        }

        //return a randomized attack animation string
        public void SetRandomAttackAnimation()
        {
            if (attackAnimations.Length == 1) {
                currentAttackAnimation = attackAnimations[0];
                currentAttackTime = attackDuration[0];
            }else{
                if (attackAnimations.Length > 0) {
                    int temp = Random.Range(0, attackAnimations.Length);
                    currentAttackAnimation = attackAnimations[temp];
                    currentAttackTime = attackDuration[temp];
                }else{
                    currentAttackAnimation = "";
                }
            }
        }

        //check and set if attack in intervals is set to randomized
        public void RandomizeAttackIntervals()
        {
            if (attackInIntervals && randomizeAttackIntervals) {
                attackInIntervalsTime = Random.Range(randomizeAttackIntervalsBetween.x, randomizeAttackIntervalsBetween.y);
            }
        }

        //validate some properties
        public void Validate()
        {
            if (!moveBackwards) moveBackwardsAttack = false;

            //minimum of x in randomizing attack intervals is 0.5f
            if (randomizeAttackIntervalsBetween.x < 0.5f) randomizeAttackIntervalsBetween.x = 0.5f;

            //disable randomizeAttackIntervals if attackInIntervals is false
            if (!attackInIntervals) randomizeAttackIntervals = false;

            if (distanceFromEnemy - attackDistance < 0.5f) {
                if (distanceFromEnemy > 1f) attackDistance = distanceFromEnemy - 1f;
            }

            //attack distance can't be bigger than distance from enemy
            if (distanceFromEnemy <= attackDistance) attackDistance = distanceFromEnemy - 1f;

            if (moveBackwardsDist == distanceFromEnemy) {
                moveBackwardsDist = distanceFromEnemy - 1f;
            }

            if (attackDuration.Length != attackAnimations.Length) SetDurationArray();
            if (coverShooterOptions.coverShooter) CoverShooterProperties();  
        }

        //set the attack duration array to the same number as attack animations array
        void SetDurationArray()
        {
            float[] arrCopy;
            arrCopy = new float[attackDuration.Length];

            attackDuration.CopyTo(arrCopy, 0);
            attackDuration = new float[attackAnimations.Length];

            for (var i=0; i<attackDuration.Length; i+=1) {
                if (i <= arrCopy.Length-1) {
                    attackDuration[i] = arrCopy[i];
                }
            }
        }

        //set the cover shooter properties
        void CoverShooterProperties()
        {
            attackInIntervals = true;
            if (coverShooterOptions.searchDistance >= distanceFromEnemy) coverShooterOptions.searchDistance = distanceFromEnemy - 2f;
        }

        //print gizmos for cover shooter debugging
        public void CoverShooterGizmos(Transform transform)
        {
            if (coverShooterOptions.coverShooter) {
                Gizmos.color = Color.white;
                Gizmos.DrawWireSphere(transform.position, coverShooterOptions.searchDistance);
            }
        }
    }
}