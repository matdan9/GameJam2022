using UnityEngine;
using UnityEngine.AI;

public class BigBoy : MonoBehaviour
{
    
    
    private NavMeshAgent agent;
    

    //Patroling
    private Vector3 walkPoint;
    private bool isWalkPointSet = false;
    private float walkPointRange = 10f;

    private LayerMask ground;

    private bool isRaging = false;

    public static Vector3 RandomNavSphere (Vector3 origin, float distance, int layermask) {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;
           
            randomDirection += origin;
           
            NavMeshHit navHit;
           
            bool a = NavMesh.SamplePosition (randomDirection, out navHit, distance, layermask);
            //bool a = NavMesh.Raycast(randomDirection, distance, out navHit, NavMesh.AllAreas);

            Debug.Log(a);
           
            return navHit.position;
        }



    // Start is called before the first frame update
    void Start()
    {
        ground = LayerMask.GetMask("GroundBigBoy");
        agent = GetComponent<NavMeshAgent>();
        
    }

    // Update is called once per frame
    void Update()
    {
        EnnemyPatroling();
    }


    private void EnnemyPatroling(){

        if(!isWalkPointSet && !isRaging){
            SearchWalkingPoint();
        }
        else if(isWalkPointSet && !isRaging)
        {
            agent.SetDestination(walkPoint);
        }
        else if(!isWalkPointSet && isRaging || isWalkPointSet && isRaging)
        {
            agent.SetDestination(agent.transform.position);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if(distanceToWalkPoint.magnitude < 1f){
            isWalkPointSet = false;
        }

    }

    private void SearchWalkingPoint(){

        Vector3 randomPosition = RandomNavSphere(agent.transform.position, 20f, 10);
        Debug.Log(randomPosition);

        walkPoint = randomPosition;

        if(Physics.Raycast(walkPoint, -transform.up, 3f, ground)){
            isWalkPointSet = true;
        }
    }

    

}
