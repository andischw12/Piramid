using UnityEngine;
using UnityEditor;

namespace BlazeAISpace
{
    [CreateAssetMenu(fileName="Blaze Profile", menuName="Blaze AI/Blaze Profile")]
    public class BlazeProfile : ScriptableObject
    {
        [Space(20), Header("Set scripts & audio objects from Blaze component.")]

        public LayerMask groundLayers;
        public float pathRecalculationRate = 0.3f;
        public float proxyOffset = 0.7f;
        public bool enableGravity = true;
        public bool useRootMotion = false;
        
        [Space(10)]

        public bool avoidFacingObstacles = true;
        public float obstacleRayDistance = 3f;
        public Vector3 obstacleRayOffset;
        public LayerMask obstacleLayers;

        [Space(10)]
        public string[] tagsToAvoid;
        
        [Space(10)]
        public WaypointsScriptable waypoints;
        
        [Space(10)]
        public VisionScriptable vision;

        [Space(10)]
        public NormalStateScriptable normalState;

        [Space(10)]
        public AlertStateScriptable alertState;

        [Space(10)]
        public AttackStateScriptable attackState;

        [Space(10)]
        public DistractionsScriptable distractions;

        [Space(10)]
        public HitsScriptable hits;

        [Space(10)]
        public DeathScriptable death;

        
        void OnValidate() 
        {
            if (vision.maxSightLevel < 0f) vision.maxSightLevel = 0f;
            if (vision.sightLevel < 0f) vision.sightLevel = 0f;

            #if UNITY_EDITOR
            foreach (var tag in tagsToAvoid) {
                if (System.Array.IndexOf(vision.hostileTags, tag) >= 0) {
                    EditorUtility.DisplayDialog("Error",$"The tag name = {tag} is set in both [Tags To Avoid] and [Hostile Tags] in vision. You can't have the same tag in both.", "OK");
                }
            }
            #endif

            if (attackState.coverShooterOptions.coverShooter) attackState.alwaysLookAtEnemy = true;
            if (attackState.coverShooterOptions.coverShooter) vision.visionDuringAttackState.sightRange = attackState.distanceFromEnemy + attackState.coverShooterOptions.searchDistance;

            if (!alertState.useAlertStateOnStart && !normalState.useNormalStateOnStart) {
                normalState.useNormalStateOnStart = true;
            }

            waypoints.Validate();
            attackState.Validate();
        }
    }
}

