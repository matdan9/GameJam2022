using UnityEngine;
using UnityEngine.AI;

public class Screamer : MonoBehaviour
{
    
    private AI ai = new AI();
    

    //Patroling
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
    private AudioSource _audioAlert;
    private CapsuleCollider collider;

    void Awake()
    {
        bigBoy = GameObject.Find("bigBoy");
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        setupAi();
        collider = GetComponent<CapsuleCollider>();
        animScreamer = GetComponent<Animator>();
        animScreamer.SetBool("walk", true);
        _audioFootstep = this.gameObject.AddComponent<AudioSource>() as AudioSource;
        _audioIdle = this.gameObject.AddComponent<AudioSource>() as AudioSource;
        _audioAlert = this.gameObject.AddComponent<AudioSource>() as AudioSource;
        AudioManager.setAudio(_audioFootstep);
        AudioManager.setAudio(_audioIdle);
        AudioManager.setAudio(_audioAlert);
        _audioIdle.clip = audioManager.spiderIdle;
        _audioIdle.maxDistance = 12;
        _audioIdle.Play();
        _audioAlert.maxDistance = 20.0f;
        _audioAlert.clip = audioManager.spiderAlert;
        _audioAlert.volume = 0.75f;
        _audioFootstep.clip = audioManager.spiderWalk;
        _audioFootstep.volume = 1;
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
        if (ai.Update(transform)) Scream();
    }

    public void Scream(){
        if (_audioAlert.isPlaying) return;
        animScreamer.SetBool("alert", true);
        _audioAlert.Play();
        currentScreamPos = transform.position;
        bigBoy.GetComponent<BigBoy>().OnScreamerCall(currentScreamPos);
        collider.isTrigger = true;
        Invoke("EnnemyCalmDown", 2.5f);
    }

    private void EnnemyCalmDown(){
         animScreamer.SetBool("alert", false);
         _audioAlert.Stop();
         _audioFootstep.Play();
        collider.isTrigger = false;
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

    public void TakeDamage(int val)
    {
        return;
    }
}
