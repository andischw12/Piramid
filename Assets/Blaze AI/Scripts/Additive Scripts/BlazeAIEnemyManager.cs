using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlazeAISpace 
{
    public class BlazeAIEnemyManager : MonoBehaviour
    {
        [HideInInspector] public List<BlazeAI> enemiesScheduled  = new List<BlazeAI>();

        [Tooltip("The amount of time in seconds to let an enemy attack")]
        public float attackTimer = 3f;
        [Tooltip("Setting this to false won't let enemies attack instead they'll just be in attack idle state")]
        public bool callEnemies = true;
        
        bool calledRoutine;
        
        BlazeAI lastEnemy;
        BlazeAI newEnemy;
        

        void Update ()
        {
            //if list is greater than 1, run coroutine to choose an enemy
            if (enemiesScheduled.Count > 0 && callEnemies) {
                StartCoroutine(ChooseEnemy());
            }
        }

        //choose a random enemy from the list to attack
        IEnumerator ChooseEnemy()
        {
            if (calledRoutine) yield break;
            calledRoutine = true;

            yield return new WaitForSeconds(attackTimer);

            if (enemiesScheduled.Count > 1) {
                newEnemy = enemiesScheduled[Random.Range(0, enemiesScheduled.Count)];

                //if new enemy is the same as last one - increment
                if (lastEnemy == newEnemy || newEnemy.attackBackUp || ((int)newEnemy.state != 2) || !newEnemy.enemyInSight) {

                    int max = enemiesScheduled.Count;
                    int currentIndex = enemiesScheduled.IndexOf(newEnemy);

                    if ((currentIndex + 1) == max) {
                        newEnemy = enemiesScheduled[0];
                    }else{
                        newEnemy = enemiesScheduled[currentIndex+1];
                    }
                }

                lastEnemy = newEnemy;
                newEnemy.GoForAttack();
            }else{
                if (enemiesScheduled.Count == 1) {
                    lastEnemy = enemiesScheduled[0];
                    lastEnemy.GoForAttack();
                }
            }

            yield return StartCoroutine(Reset());
        }

        IEnumerator Reset() 
        {
            yield return new WaitForSeconds(0.5f);    
            calledRoutine = false;
        }

        //remove a specific enemy from the list
        public void RemoveEnemy(BlazeAI enemy)
        {
            enemiesScheduled.Remove(enemy);
        }
    }
}

