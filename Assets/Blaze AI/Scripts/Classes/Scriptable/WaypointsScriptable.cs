using UnityEngine;

namespace BlazeAISpace
{
    [System.Serializable]
    public class WaypointsScriptable
    {
        [Tooltip("On game start the NPC will instantly move onto the first waypoint instead of being idle first (works on both manual waypoints and randomize)")]
        public bool instantMoveAtStart = true;
        [Tooltip("Setting this to true will loop the waypoints when patrolling, setting it to false will stop at the waypoint.")]
        public bool loop = false;
        [Tooltip("Enabling randomize will instead generate randomized waypoints within the navmesh in a continuous fashion.")]
        public bool randomize = true;

        //save inspector states
        bool inspectorLoop;
        bool inspectorRandomize;

        public void Validate() 
        {
            if (randomize && loop) {
                randomize = !inspectorRandomize;
                loop = !inspectorLoop;
            }

            inspectorLoop = loop;
            inspectorRandomize = randomize;
        }
    }
}

