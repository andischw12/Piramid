using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum EnemyMode {Idle,Patrol,Attack}
public class EnemyControl : MonoBehaviour
{
    public EnemyMode mode;
    public Transform[] points;
    private int destPoint = 0;
    public NavMeshAgent agent;
    bool AgentPatrolFlagWaiting;
    public Transform GurdObject;
    Transform player;


    void Start()
    {
         
        player = GameObject.FindGameObjectWithTag("Player").transform;
         
        agent.autoBraking = false;
        GoToNextPoint();

    }



    private void Update()
    {
 
            
 

        if (Input.GetMouseButtonDown(0))
        {
            if (!AgentPatrolFlagWaiting) 
            {
                
                StartCoroutine(GoToNextPoint());
            }
              
        }

    }

 


    IEnumerator GoToNextPoint() 
    {
        AgentPatrolFlagWaiting = true;
        agent.isStopped = false;
        agent.destination = points[GetRandomPoint()].position;
        GetComponent<Animator>().SetTrigger("Walk");
        yield return new WaitUntil(() => agent.remainingDistance < 0.5f);
        GetComponent<Animator>().SetTrigger("Idle");
        agent.isStopped = true;
        yield return new WaitForSeconds(3);
       
        AgentPatrolFlagWaiting = false;



    }


     

 

    int GetRandomPoint() 
    {
        destPoint++;
        if (destPoint == points.Length)
            destPoint = 0;
        return destPoint;
    }

    


}
