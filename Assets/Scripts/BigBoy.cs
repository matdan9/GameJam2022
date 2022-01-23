using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class BigBoy : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField]
    private int health = 5;
    private Animator animator;
    
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
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        EnnemyPatroling();
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
            agent.SetDestination(agent.transform.position);
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if(distanceToWalkPoint.magnitude < 2f){
            isWalkPointSet = false;
        }
    }

    public void TakeDamage(int damage){
        if(isDead()) return;
        health -= damage;
        if(isDead()){
            killBigBoy();
            return;
        }
        animator.SetBool("take damage", true);
    }

    private void killBigBoy(){
        stopAnimations();
        animator.SetBool("die", true);
        agent.isStopped = true;
    }

    private bool isDead(){
        return health <=0;
    }

    private void stopAnimations(){
        foreach(AnimatorControllerParameter parameter in animator.parameters) {
            animator.SetBool(parameter.name, false);
        }
    }


    private void SearchWalkingPoint(){
        walkPoint = NavMeshHelper.RandomCoordinateInRange(agent.transform.position, 40f, navMeshArea);
        if(Physics.Raycast(walkPoint, -transform.up, 3f, ground)){
            isWalkPointSet = true;
        }
    }
}
