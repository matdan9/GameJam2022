using UnityEngine;
using UnityEngine.AI;

public class BigBoy : MonoBehaviour
{
    private NavMeshAgent agent;
    
    //Patroling
    private Vector3 walkPoint;
    private bool isWalkPointSet = false;
    private float walkPointRange = 10f;
    private int navMeshArea;
    private LayerMask ground;
    private bool isRaging = false;

    // Start is called before the first frame update
    void Start()
    {
        ground = LayerMask.GetMask("Ground");
        agent = GetComponent<NavMeshAgent>();
        navMeshArea = NavMesh.GetAreaFromName("BigBoy");
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
        else if(isWalkPointSet && !isRaging) {
            agent.SetDestination(walkPoint);
        }
        else if(!isWalkPointSet && isRaging || isWalkPointSet && isRaging) {
            agent.SetDestination(agent.transform.position);
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if(distanceToWalkPoint.magnitude < 2f){
            isWalkPointSet = false;
        }
    }

    private void SearchWalkingPoint(){
        walkPoint = NavMeshHelper.RandomCoordinateInRange(agent.transform.position, 40f, navMeshArea);
        if(Physics.Raycast(walkPoint, -transform.up, 3f, ground)){
            isWalkPointSet = true;
        }
    }
}
