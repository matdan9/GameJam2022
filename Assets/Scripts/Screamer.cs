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
    private bool isScreaming = false;
    private Animator animScreamer;
    private int navMeshArea;
    
    public Transform patrolArea;
    public Vector3 currentScreamPos;
    [SerializeField]private GameObject bigBoy;


    // Start is called before the first frame update
    void Start()
    {
        ground = LayerMask.GetMask("Ground");
        agent = GetComponent<NavMeshAgent>();
        animScreamer = GetComponent<Animator>();
        navMeshArea = NavMesh.GetAreaFromName("IMP");
        if(patrolArea == null) patrolArea = this.transform;

        bigBoy = GameObject.Find("bigBoy");
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.tag != "ScreamerFix"){
          EnnemyPatroling();  
          animScreamer.SetBool("walk", true);
        }
    }

    private void EnnemyPatroling()
    {
        if(!isWalkPointSet && !isScreaming) {
            SearchWalkingPoint();
        }
        else if(isWalkPointSet && !isScreaming) {
            agent.SetDestination(walkPoint);
        }
        else if(!isWalkPointSet && isScreaming || isWalkPointSet && isScreaming) {
            agent.SetDestination(agent.transform.position);
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if(distanceToWalkPoint.magnitude < 1f){
            isWalkPointSet = false;
        }
    }

    private void SearchWalkingPoint(){
        walkPoint = NavMeshHelper.RandomCoordinateInRange(patrolArea.position, 20f, navMeshArea);
        if(Physics.Raycast(walkPoint, -transform.up, 3f, ground)){
            isWalkPointSet = true;
        }
    }

    public void EnnemyScream(){
        isScreaming = true;
        animScreamer.SetBool("alert", true);

        if(transform.tag == "ScreamerFix")
        {
            Invoke("EnnemyFixCalmDown", 3f);
            currentScreamPos = transform.position;
            bigBoy.GetComponent<BigBoy>().OnScreamerCall(currentScreamPos);

        }
        else
        {
            Invoke("EnnemyCalmDown", 3f);
            currentScreamPos = transform.position;
            bigBoy.GetComponent<BigBoy>().OnScreamerCall(currentScreamPos);
            
        }

        transform.tag = "EnemyTouched";
        Debug.Log("Scream sound !");
    }

    private void EnnemyCalmDown(){
         isScreaming = false;
         transform.tag = "Enemy";
         animScreamer.SetBool("alert", false);
    }

    private void EnnemyFixCalmDown(){
         isScreaming = false;
         transform.tag = "ScreamerFix";
         animScreamer.SetBool("alert", false);
         animScreamer.SetBool("walk", false);
    }
}
