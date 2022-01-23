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

    public GameObject target;

    public Vector3 RandomNavSphere (Vector3 origin, float distance, int layermask) {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;
        randomDirection += origin;
        NavMeshHit navHit;
        var g = NavMesh.GetAreaFromName("BigBoy");
        bool a = NavMesh.SamplePosition(randomDirection, out navHit, distance, 1 << g);
        if(a){
            target.transform.position = navHit.position;
        }
        Debug.Log(g + " :: " + NavMesh.GetAreaFromName("IMP") + " :: " + a);
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
        if(distanceToWalkPoint.magnitude < 2f){
            isWalkPointSet = false;
        }
    }

    private void SearchWalkingPoint(){
        walkPoint = RandomNavSphere(agent.transform.position, 20f, ground);
        if(Physics.Raycast(walkPoint, -transform.up, 3f, ground)){
            isWalkPointSet = true;
        }
    }
}
