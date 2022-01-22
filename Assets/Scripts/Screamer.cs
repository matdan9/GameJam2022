using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Screamer : MonoBehaviour
{
    
    private NavMeshAgent agent;
    

    //Patroling
    private Vector3 walkPoint;
    private bool isWalkPointSet = false;
    private float walkPointRange = 10f;

    private LayerMask ground;

    public static Vector3 RandomNavSphere (Vector3 origin, float distance, int layermask) {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;
           
            randomDirection += origin;
           
            NavMeshHit navHit;
           
            NavMesh.SamplePosition (randomDirection, out navHit, distance, layermask);
           
            return navHit.position;
        }



    // Start is called before the first frame update
    void Start()
    {
        ground = LayerMask.GetMask("Ground");
        agent = GetComponent<NavMeshAgent>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.tag != "ScreamerFix")EnnemiPatroling();
    }


    private void EnnemiPatroling(){

        if(!isWalkPointSet){
            SearchWalkingPoint();
        }else{
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if(distanceToWalkPoint.magnitude < 1f){
            isWalkPointSet = false;
        }

    }

    private void SearchWalkingPoint(){

        Vector3 randomPosition = RandomNavSphere(agent.transform.position, 20f, -1);

        walkPoint = randomPosition;

        if(Physics.Raycast(walkPoint, -transform.up, 3f, ground)){
            isWalkPointSet = true;
        }

        
    }
}
