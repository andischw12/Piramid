using UnityEngine;
using System.Collections.Generic;

public class BlazeAIDistraction : MonoBehaviour {
    
    [Tooltip("The radius of the distraction")]
    public float distractionRadius;
    [Tooltip("Do you want the distraction to pass through obstacles with colliders?")]
    public bool passThroughColliders;
    BlazeAI noMovingEnemy;


    //public method for triggering the distractions
    public void TriggerDistraction() {

        //make the actual distraction using an overlap sphere physics
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, distractionRadius);
        List<BlazeAI> uniqueScripts = new List<BlazeAI>();
        List<BlazeAI> enemiesList = new List<BlazeAI>();

        //npcs have two colliders so each one returns the same script
        //add only unique ones
        foreach (var hit in hitColliders)
        {
            var script = hit.GetComponent<BlazeAI>();
            if (script != null) {
                if (!uniqueScripts.Contains(script)) uniqueScripts.Add(script);
            }
        }

        //loop the unique scripts and add them to enemies list
        //enemies who are not set to move to distraction, distract them
        foreach(var script in uniqueScripts) {
            if (script.distractions.moveToDistractionLocation) {
                enemiesList.Add(script);
            }else{
                if (CheckIfReaches(script.transform)) script.Distract(transform);
                if (noMovingEnemy == null) noMovingEnemy = script;
            }   
        }

        //keep track of highest priority and enemy index
        float highestValue = 0f;
        int i = 0;

        //now loop the enemy list and move to location the one with the highest priority level
        if(enemiesList.Count > 0){
            
            //order the enemies according to priortiy values
            enemiesList.Sort((a, b) => { return a.distractions.checkDistractionPriorityLevel.CompareTo(b.distractions.checkDistractionPriorityLevel); });
            
            //get the highest value
            highestValue = enemiesList[enemiesList.Count - 1].distractions.checkDistractionPriorityLevel;
            
            for(var x=0; x<enemiesList.Count; x++){
                if (enemiesList[x].distractions.checkDistractionPriorityLevel < highestValue) {
                    if (CheckIfReaches(enemiesList[x].transform)) enemiesList[x].Distract(transform, true);
                    enemiesList.Remove(enemiesList[x]);
                }
            }
            
            //loop and move to location the enemy with smallest distance
            float smallestDistance = Mathf.Infinity;
            int currentLowestIndex = 0;
            
            foreach (var item in enemiesList) {
                
                float newDistance = (item.transform.position - transform.position).sqrMagnitude;
                if (newDistance < smallestDistance) {
                    smallestDistance = newDistance;
                    currentLowestIndex = i;
                }else{
                    if (CheckIfReaches(item.transform)) item.Distract(transform, true);
                }

                i++;
            }

            //play distraction audio only if not already distracted
            if (!enemiesList[currentLowestIndex].distracted) {
                if (CheckIfReaches(enemiesList[currentLowestIndex].transform)) enemiesList[currentLowestIndex].distractions.PlayDistractedAudios();
            }

            //distract the highest priority enemy with lowest distance
            if (CheckIfReaches(enemiesList[currentLowestIndex].transform)) enemiesList[currentLowestIndex].Distract(transform);
            
        }else{
            if (noMovingEnemy == null) return;
            
            noMovingEnemy.StopSystemsAudios("distraction");
            if (CheckIfReaches(noMovingEnemy.transform)) noMovingEnemy.distractions.PlayDistractedAudios();
        }
    }

    //checks if distraction will reach NPC through colliders
    bool CheckIfReaches(Transform enemyPos)
    {
        if (passThroughColliders) return true;

        RaycastHit hit;
        Vector3 dir = (new Vector3(enemyPos.position.x, enemyPos.position.y + enemyPos.GetComponent<CharacterController>().height / 2f, enemyPos.position.z) - transform.position);
        
        if (Physics.Raycast(transform.position, dir, out hit, Mathf.Infinity)) {
            if (hit.transform == enemyPos) {
                return true;
            }else{
                return false;
            }
        }else{
            return false;
        }
    }

    //show distraction radius
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distractionRadius);
    }
}

