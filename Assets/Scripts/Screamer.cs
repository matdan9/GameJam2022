using UnityEngine;
using UnityEngine.AI;

public class Screamer : MonoBehaviour
{
    
    private NavMeshAgent agent;
    private Rigidbody rb;

    //Patroling
    private Vector3 walkPoint;
    private bool isWalkPointSet = false;
    private float walkPointRange = 10f;
    private LayerMask ground;
    private bool isScreaming = false;
    private Animator animScreamer;
    private int navMeshArea;
    private AudioManager audioManager;
    private AudioSource _audioIdle;
    private AudioSource _audioFootstep;

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
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        rb = gameObject.GetComponent<Rigidbody>();

        _audioFootstep = this.gameObject.AddComponent<AudioSource>() as AudioSource;
        _audioIdle = this.gameObject.AddComponent<AudioSource>() as AudioSource;
        AudioManager.setAudio(_audioFootstep);
        AudioManager.setAudio(_audioIdle);
        _audioIdle.clip = audioManager.spiderIdle;
    }

    // Update is called once per frame
    void Update()
    {
        if(rb.velocity.magnitude >= 1 && !_audioFootstep.isPlaying)
        {
            _audioFootstep.clip = audioManager.spiderWalk;
            _audioFootstep.Play();
        }
        if (transform.tag != "ScreamerFix"){
          EnnemyPatroling();  
          animScreamer.SetBool("walk", true);
        }
    }

    private void EnnemyPatroling()
    {
        //_audioFootstep.Play();
        if (!isWalkPointSet && !isScreaming) {
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
        _audioFootstep.clip = audioManager.spiderAlert;

        if (transform.tag == "ScreamerFix")
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
