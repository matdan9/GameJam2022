using UnityEngine;
using UnityEngine.AI;

public class Screamer : MonoBehaviour
{
    
    private AI ai = new AI();
    

    //Patroling
    private bool isScreaming = false;
    private Animator animScreamer;
    
    public Vector3 currentScreamPos;
    [SerializeField]
    private GameObject bigBoy;

    [SerializeField]
    public Transform patrolArea;
    [SerializeField]
    public float fov = 110;
    [SerializeField]
    public float viewRange = 20;
    [SerializeField]
    public float contactRange = 4;
    [SerializeField]
    public float dangerRange = 2;

    private AudioManager audioManager;
    private AudioSource _audioIdle;
    private AudioSource _audioFootstep;


    // Start is called before the first frame update
    void Start()
    {
        setupAi();
        animScreamer = GetComponent<Animator>();
        bigBoy = GameObject.Find("bigBoy");
        animScreamer.SetBool("walk", true);
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        _audioFootstep = this.gameObject.AddComponent<AudioSource>() as AudioSource;
        _audioIdle = this.gameObject.AddComponent<AudioSource>() as AudioSource;
        AudioManager.setAudio(_audioFootstep);
        AudioManager.setAudio(_audioIdle);
        _audioIdle.clip = audioManager.spiderIdle;
        _audioIdle.maxDistance = 12;
        _audioIdle.Play();
    }

    private void setupAi(){
        AiState state = AiState.Patrol;
        if(patrolArea == null){
            patrolArea = this.transform;
            state = AiState.Roaming;
        }
        ai.SetAiView(viewRange, contactRange, dangerRange, fov);
        ai.SetNavigation(NavMesh.GetAreaFromName("IMP"), GetComponent<NavMeshAgent>());
        ai.SetAiSettings("Player", 8, state, patrolArea);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(ai.Update(transform)){
            Scream();
        }
        if(isScreaming){
            _audioFootstep.clip = audioManager.spiderAlert;
            _audioFootstep.volume = 0.75f;
            _audioFootstep.Play();
        }
    }

    public void Scream(){
        isScreaming = true;
        animScreamer.SetBool("alert", true);
        Invoke("EnnemyCalmDown", 3f);
        currentScreamPos = transform.position;
        bigBoy.GetComponent<BigBoy>().OnScreamerCall(currentScreamPos);
        //transform.tag = "EnemyTouched";
        Debug.Log("Scream sound !");
    }

    private void EnnemyCalmDown(){
         isScreaming = false;
         transform.tag = "Enemy";
         animScreamer.SetBool("alert", false);
         if (!isScreaming)
         {
            _audioFootstep.clip = audioManager.spiderWalk;
            _audioFootstep.volume = 1;
            _audioFootstep.Play();
         }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, dangerRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, contactRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, viewRange);
    }
}
