using UnityEngine;
using UnityEngine.AI;

public class BigBoy : MonoBehaviour
{
    private NavMeshAgent agent;
    
    //Patroling
    public Vector3 walkPoint;
    private bool isWalkPointSet = false;
    private float walkPointRange = 10f;
    private int navMeshArea;
    private LayerMask ground;
    private bool isRaging = false;
    private Animator animBigBoy;

    // Start is called before the first frame update
    void Start()
    {
        ground = LayerMask.GetMask("Ground");
        agent = GetComponent<NavMeshAgent>();
        navMeshArea = NavMesh.GetAreaFromName("BigBoy");
        animBigBoy = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        EnnemyPatroling();
        if(isRaging){
          OnRageMode();  
        }else{
            animBigBoy.SetBool("run", false);
        }
    }


    private void EnnemyPatroling(){
        if(!isWalkPointSet && !isRaging){
            SearchWalkingPoint();
        }
        else if(isWalkPointSet && !isRaging) {
            agent.SetDestination(walkPoint);
        }
        else if(!isWalkPointSet && isRaging || isWalkPointSet && isRaging) {
             
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if(distanceToWalkPoint.magnitude < 2f){
            isRaging = false; 
            isWalkPointSet = false;
            agent.speed = 3.5f;
        }
    }

    private void SearchWalkingPoint(){
        walkPoint = NavMeshHelper.RandomCoordinateInRange(agent.transform.position, 40f, navMeshArea);
        if(Physics.Raycast(walkPoint, -transform.up, 3f, ground)){
            isWalkPointSet = true;
        }
    }


    public void OnScreamerCall(Vector3 screamerPosition){
        walkPoint = NavMeshHelper.RandomCoordinateInRange(screamerPosition, 10f, navMeshArea);
        agent.SetDestination(walkPoint);
        
        

        isRaging = true; 
    }

    private void OnRageMode(){

        agent.speed = 10f;
        animBigBoy.SetBool("run", true);
    }



}
