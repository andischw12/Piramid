using UnityEngine;

namespace BlazeAISpace
{
    [System.Serializable]
    public class VisionScriptable
    {
        [System.Serializable] public struct normalVision {
            [Range(0f, 360f)]
            public float coneAngle;
            public float sightRange;
            
            //constructor
            public normalVision (float angle, float range) {
                coneAngle = angle;
                sightRange = range;
            }
        }

        [System.Serializable] public struct alertVision {
            [Range(0f, 360f)]
            public float coneAngle;
            public float sightRange;

            //constructor
            public alertVision (float angle, float range) {
                coneAngle = angle;
                sightRange = range;
            }
        }

        [System.Serializable] public struct attackVision {
            [Range(0f, 360f)]
            [Tooltip("Always better to have this at 360 in order for the NPC to have 360 view when in attack state.")]
            public float coneAngle;
            [Tooltip("Will be automatically set if cover shooter enabled.")]
            public float sightRange;

            //constructor
            public attackVision (float angle, float range) {
                coneAngle = angle;
                sightRange = range;
            }
        }

        [System.Serializable] public struct AlertOptions {
            [Tooltip("Tag of a certain event you want the NPC to be alarmed about. Also attach a game object with several audio sources to play a random audio that is specific to this event. For example: if the player opens a door and the NPC sees this DOOR OPENED tag he can react with a specific audio source, 'Who left this door open?' and becomes in alert state")]
            public string tag;

            [Tooltip("After the NPC sees and reacts to the tag, the object's tag will be changed to this fall back tag. The reason for this is to avoid this NPC and others to react on the same situation, while also giving you control over the tag for your functionality. Make sure this tag isn't listed in the alert tags or NPCs will react to it again")]
            public string fallBackTag;
            
            [Tooltip("play animation state of this name. If move to location is set to true the animation will be played after reaching the location. If field is empty it will be ignored")]
            public string animationName;

            [Tooltip("Amount of time in seconds to pass before returning to patrol in alert state. Will start countdown when NPC reaches location if 'Move To Location' is set to true OR if it is set to false then will start countdown when the animation starts (if that too is set) if neither is set then this will be ignored")]
            public float time;
            
            [Tooltip("Move the NPC to an approximate location of the alert tag then plays animation if 'Animation Name' not empty. Will continue patrol in alert state when the alert state idle time finishes.")]
            public bool moveToLocation;

            [Tooltip("If enabled, this will call other npcs to alert location")]
            public bool callOthersToLocation;
        }

        [Tooltip("Tags that will make the NPC become in alert state such as tags of dead bodies or an open door.")]
        public AlertOptions[] alertTags;

        [Tooltip("The tags of hostile gameobjects (player/s) that this NPC should attack.")]
        public string[] hostileTags;

        [Header("SET THE VISION RANGE AND CONE ANGLE FOR EACH STATE")]
        public normalVision visionDuringNormalState = new normalVision(90f, 10f);
        public alertVision visionDuringAlertState = new alertVision(100f, 15f);
        public attackVision visionDuringAttackState = new attackVision(360f, 15f);

        [Space(5)]
        [Tooltip("The level of sight (eyes) of the NPC, anything below this level will be seen, anything above it won't.")]
        public float sightLevel = 1f;
        [Tooltip("The maximum level of sight of the NPC to detect objects, shown as a purple rectangle in the scene view. Any object above the max level won't be seen but anything between the rectangle and below the actual vision cone will be seen.")]
        public float maxSightLevel = 2f;
    }
}
