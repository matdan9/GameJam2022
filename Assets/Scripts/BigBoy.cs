using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class BigBoy : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField]
    private int health = 5;
    private Animator animBigBoy;
    
    //Patroling
    public Vector3 walkPoint;
    private bool isWalkPointSet = false;
    private float walkPointRange = 10f;
    private int navMeshArea;
    private LayerMask ground;
    private bool isRaging = false;
    private Animator animBigBoy;
    private bool isPlayerDead = false;

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

        if(isPlayerDead) agent.speed = 0;
    }


    private void EnnemyPatroling(){
        if(isDead()) return;
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

    public void TakeDamage(int damage){
        if(isDead()) return;
        health -= damage;
        if(isDead()){
            killBigBoy();
            return;
        }
        animBigBoy.SetBool("take damage", true);
    }

    private void killBigBoy(){
        stopAnimations();
        animBigBoy.SetBool("die", true);
        agent.isStopped = true;
    }

    private bool isDead(){
        return health <=0;
    }

    private void stopAnimations(){
        foreach(AnimatorControllerParameter parameter in animBigBoy.parameters) {
            animBigBoy.SetBool(parameter.name, false);
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

     private void OnTriggerEnter(Collider collision)
    {
        if(collision.transform.tag == "Player"){
            GameObject player = collision.transform.gameObject;
            GameObject cam = player.transform.GetChild(0).transform.gameObject;
            GameObject rightHand = GameObject.Find("forearm.R.002_end");

            cam.transform.SetParent(rightHand.transform); 
            cam.transform.localPosition = new Vector3(0.000152f, 0.000369f, -0.00019f);
            cam.transform.localEulerAngles = new Vector3(26f, 0, 0);

            walkPoint = transform.position;
            isRaging = false;
            animBigBoy.SetTrigger("kill"); 
            isPlayerDead = true;

            
            player.GetComponent<PlayerController>().EnableMouseLook(false);
            player.GetComponent<CapsuleCollider>().enabled = false;
            player.GetComponent<PlayerController>().enabled = false;
            
        }
    }



}
