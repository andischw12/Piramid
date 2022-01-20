﻿using UnityEngine;
using System.Collections.Generic;

namespace BlazeAISpace
{
    [System.Serializable]
    public class Vision
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
            [Tooltip("Will be automatically set if cover shooter enabled based on Distance From Enemy property.")]
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

            [Tooltip("Set to a game object with several audio source components. One will be randomly chosen and played. If only one audio source component is added then that only one will be played. If no audios added, process will be ignored")]
            public GameObject audiosToPlay;

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
        [Tooltip("OPTIONAL: add the head object, this will be used for updating both the rotation of the vision according to the head and the sight level automatically. If empty, the rotation will be according to the body, projecting forwards.")]
        public Transform head;
        
        [Header("DEBUG")]
        [Tooltip("Show the vision cone of normal state in edit mode for easier debugging.")]
        public bool showNormalVision = true;
        [Tooltip("Show the vision cone of alert state in edit mode for easier debugging.")]
        public bool showAlertVision = false;
        [Tooltip("Show the vision cone of attack state in edit mode for easier debugging.")]
        public bool showAttackVision = false;
        [Tooltip("Shows the maximum sight level as a purple rectangle.")]
        public bool showMaxSightLevel = false;
        
        public struct TagAlertOptions {

            public GameObject audiosToPlay;
            public string fallBackTag;
            public string animationName;
            public bool moveToLocation;
            public float time;
            public bool callOthersToLocation;

            //constructor
            public TagAlertOptions(GameObject audiosToPlay, string fallBackTag, string animationName, bool moveToLocation, float time, bool callOthersToLocation)
            {
                this.audiosToPlay = audiosToPlay;
                this.fallBackTag = fallBackTag;
                this.animationName = animationName;
                this.moveToLocation = moveToLocation;
                this.time = time;
                this.callOthersToLocation = callOthersToLocation;
            }
        }

        public Dictionary<string, TagAlertOptions> alertTagsDict;
        //call from the main script to build the dictionary
        public void buildDictionaryOfAlert()
        {
            alertTagsDict = new Dictionary<string, TagAlertOptions>();
            foreach (var item in alertTags)
            {   
                TagAlertOptions optionsStruct = new TagAlertOptions(item.audiosToPlay, item.fallBackTag, item.animationName, item.moveToLocation, item.time, item.callOthersToLocation);
                alertTagsDict.Add(item.tag, optionsStruct);
            }
        }

        //save the current audio playing within the vision sub-system
        AudioSource currentAudio;
        //play random audio
        public void AlertTagsPlayRandomAudio(string tag)
        {
            if (alertTagsDict[tag].audiosToPlay == null) return;

            AudioSource[] audios = alertTagsDict[tag].audiosToPlay.GetComponents<AudioSource>();
            if (audios.Length > 0) {
                currentAudio = audios[Random.Range(0, audios.Length)];
                currentAudio.Play();
            }else{
                if (audios.Length == 1) {
                    currentAudio = audios[0];
                    currentAudio.Play();
                }
            }
        }

        //stop current audio
        public void StopCurrentAudio()
        {
            if (currentAudio != null && currentAudio.isPlaying) currentAudio.Stop();
        }

        //show the vision spheres in level-editor screen
        public void ShowVisionSpheres(Transform transform) 
        {
            if (showNormalVision) {
                //normal state vision
                DrawVisionCone(transform, visionDuringNormalState.coneAngle, visionDuringNormalState.sightRange, Color.white);
            }
            
            if (showAlertVision) {
                //alert state vision
                DrawVisionCone(transform, visionDuringAlertState.coneAngle, visionDuringAlertState.sightRange, Color.white);
            }

            if (showAttackVision) {
                //attack state vision
                DrawVisionCone(transform, visionDuringAttackState.coneAngle, visionDuringAttackState.sightRange, Color.red);
            }

            if (showMaxSightLevel) {
                //all the passed arguments are being ignored
                DrawVisionCone(transform, visionDuringAttackState.coneAngle, visionDuringAttackState.sightRange, Color.red, true);
            }
        }

        //draw vision cone
        void DrawVisionCone(Transform transform, float angle, float rayRange, Color color, bool ignore = false)
        {
            if (ignore) 
            {
                if (showMaxSightLevel && maxSightLevel > 0f) {
                    Gizmos.color = new Color(139,0,139);
                    Gizmos.DrawCube(transform.position + new Vector3(0f, sightLevel + maxSightLevel, 0.5f), new Vector3(0.5f, 0.05f, 0.5f));
                }

                return;
            }

            if (angle >= 360f)
            {   
                Gizmos.color = color;
                Gizmos.DrawWireSphere(transform.position, rayRange);
                return;
            }

            float halfFOV = angle / 2.0f;

            Quaternion leftRayRotation1 = Quaternion.AngleAxis(-halfFOV, Vector3.up);
            Quaternion rightRayRotation1 = Quaternion.AngleAxis(halfFOV, Vector3.up);

            leftRayRotation1.eulerAngles = new Vector3(0f, leftRayRotation1.eulerAngles.y, 0f);
            rightRayRotation1.eulerAngles = new Vector3(0f, rightRayRotation1.eulerAngles.y, 0f);

            Vector3 leftRayDirection1 = leftRayRotation1 * transform.forward;
            Vector3 rightRayDirection1 = rightRayRotation1 * transform.forward;

            Vector3 npcSight = new Vector3(transform.position.x, transform.position.y + sightLevel, transform.position.z);

            Gizmos.color = Color.white;
            Gizmos.DrawRay(npcSight, leftRayDirection1 * rayRange);
            Gizmos.DrawRay(npcSight, rightRayDirection1 * rayRange);

            Gizmos.DrawLine(npcSight + rightRayDirection1 * rayRange, npcSight + leftRayDirection1 * rayRange);
        }
    }
}

