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

    public static Vector3 RandomNavSphere (Vector3 origin, float distance, int layermask) {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;
       
        randomDirection += origin;
       
        NavMeshHit navHit;
       
        int area = NavMesh.GetAreaFromName("IMP");
        NavMesh.SamplePosition (randomDirection, out navHit, distance, 1 << area);
       
        return navHit.position;
    }



    // Start is called before the first frame update
    void Start()
    {
        ground = LayerMask.GetMask("Ground");
        agent = GetComponent<NavMeshAgent>();
        animScreamer = GetComponent<Animator>();
        
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
        if(!isWalkPointSet && !isScreaming){
            SearchWalkingPoint();
        }
        else if(isWalkPointSet && !isScreaming)
        {
            agent.SetDestination(walkPoint);
        }
        else if(!isWalkPointSet && isScreaming || isWalkPointSet && isScreaming)
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

        Vector3 randomPosition = RandomNavSphere(agent.transform.position, 20f, 9);

        walkPoint = randomPosition;

        if(Physics.Raycast(walkPoint, -transform.up, 3f, ground)){
            isWalkPointSet = true;
        }
    }

    public void EnnemyScream(){
        isScreaming = true;
        animScreamer.SetBool("alert", true);

        if(transform.tag == "ScreamerFix"){
            Invoke("EnnemyFixCalmDown", 3f);
        }else{
            Invoke("EnnemyCalmDown", 3f);
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
