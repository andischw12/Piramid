using UnityEngine;
using System.Collections.Generic;

namespace BlazeAISpace
{
    [System.Serializable]
    public class Waypoints
    {
        //properties for enemy patrols and waypoints
        [Tooltip("On game start the NPC will instantly move onto the first waypoint instead of being idle first (works on both manual waypoints and randomize)")]
        public bool instantMoveAtStart = true;
        [Tooltip("Locations of the waypoints in world space.")]
        public Vector3[] waypoints;
        
        [Space(5)]
        [Tooltip("Set the idle rotation in each waypoint. Set the turning animations in the DISTRACTIONS & TURNING section. The rotation direction is shown in the debug view as red squares along the waypoints. If both the x and y are 0 then no rotation will occur and so the red squares disappear.")]
        public Vector2[] waypointsRotation;
        [Tooltip("The amount of time in seconds to pass before turning to waypoint rotation.")]
        public float timeBeforeTurning = 0.5f;
        [Tooltip("Turning speed of waypoints rotations.")]
        public float turnSpeed = 2f;

        [Space(5)]
        [Tooltip("Setting this to true will loop the waypoints when patrolling, setting it to false will stop at the waypoint.")]
        public bool loop = false;
        [Tooltip("Enabling randomize will instead generate randomized waypoints within the navmesh in a continuous fashion.")]
        public bool randomize = true;

        //save inspector states
        bool inspectorLoop;
        bool inspectorRandomize;


        //GUI validation for the waypoint system
        public void WaypointsSystemValidation() 
        {
            if (randomize && loop) {
                randomize = !inspectorRandomize;
                loop = !inspectorLoop;
            }
            
            inspectorLoop = loop;
            inspectorRandomize = randomize;

            if (waypointsRotation != null) {
                Vector2[] arrCopy;
                arrCopy = new Vector2[waypointsRotation.Length];

                waypointsRotation.CopyTo(arrCopy, 0);
                waypointsRotation = new Vector2[waypoints.Length];

                for (var i=0; i<waypointsRotation.Length; i+=1) {
                    if (i <= arrCopy.Length-1) {
                        waypointsRotation[i] = arrCopy[i];
                        if (waypointsRotation[i].x > 0.5f) waypointsRotation[i].x = 0.5f;
                        if (waypointsRotation[i].y > 0.5f) waypointsRotation[i].y = 0.5f;
                        if (waypointsRotation[i].x < -0.5f) waypointsRotation[i].x = -0.5f;
                        if (waypointsRotation[i].y < -0.5f) waypointsRotation[i].y = -0.5f;
                    }
                }
            }
        }

        //Mark the waypoints in editor-view
        public void ShowWayPoints()
        {
            if (randomize) return;
            
            for (int i = 0; i < waypoints.Length; i++){
                
                if (i == 0) {
                    Gizmos.color = new Color(0, 0.4f, 0);
                }else{
                    Gizmos.color = new Color(0.6f, 1, 0.6f);
                }
                
                Gizmos.DrawCube(waypoints[i], new Vector3(0.5f, 0.5f, 0.5f));
                
                // Draws the waypoint rotation lines
                if (waypointsRotation[i].x != 0 || waypointsRotation[i].y != 0) {
                    Gizmos.color = Color.red;
                    Gizmos.DrawCube(new Vector3(waypoints[i].x + waypointsRotation[i].x, waypoints[i].y, waypoints[i].z + waypointsRotation[i].y), new Vector3(0.3f, 0.3f, 0.3f));
                }

                if (waypoints.Length > 1)
                {
                    Gizmos.color = Color.blue;
                    if (i == 0)
                    {
                        Gizmos.DrawLine(waypoints[0], waypoints[1]);

                    }
                    else if (i == waypoints.Length - 1)
                    {
                        Gizmos.DrawLine(waypoints[i - 1], waypoints[i]);
                        Gizmos.color = Color.grey;
                        Gizmos.DrawLine(waypoints[waypoints.Length - 1], waypoints[0]);
                    }
                    else
                    {
                        Gizmos.color = Color.blue;
                        Gizmos.DrawLine(waypoints[i - 1], waypoints[i]);
                    } 
                }
            }
        }
    }
}
