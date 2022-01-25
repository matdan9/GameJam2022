using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class BigBoy : MonoBehaviour
{

    public LayerMask playerMask;
    private NavMeshAgent agent;

    [SerializeField]
    private int health = 5;
    [SerializeField]
    private float view = 3;
    [SerializeField]
    private float grabRange = 5;
    [SerializeField]
    private float viewAngle = 100;
    [SerializeField]
    GameObject deathUi, winUi;
    
    //Patroling
    public Vector3 walkPoint;
    private bool isWalkPointSet = false;
    private float walkPointRange = 10f;
    private int navMeshArea;
    private LayerMask ground;
    private bool isRaging = false;
    private Animator animBigBoy;
    private bool isPlayerDead = false;

    //Sounds
    private AudioManager audioManager;
    private AudioSource _audioVoice;
    private AudioSource _audioSFX;
    private AudioSource _audioFootstep;
    private AudioListener audioListener;

    public void Awake()
    {
        deathUi = GameObject.Find("DeathScreenBigBoi");
        winUi = GameObject.Find("WinScreen");
    }
    

    // Start is called before the first frame update
    void Start()
    {
        ground = LayerMask.GetMask("Ground");
        agent = GetComponent<NavMeshAgent>();
        navMeshArea = NavMesh.GetAreaFromName("BigBoy");
        animBigBoy = GetComponent<Animator>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        _audioVoice = this.gameObject.AddComponent<AudioSource>() as AudioSource;
        _audioFootstep = this.gameObject.AddComponent<AudioSource>() as AudioSource;
        _audioSFX = this.gameObject.AddComponent<AudioSource>() as AudioSource;
        AudioManager.setAudio(_audioVoice);
        AudioManager.setAudio(_audioFootstep);
        _audioFootstep.maxDistance = 30.0f;
        _audioVoice.maxDistance = 20.0f;
        
        audioListener = GameObject.FindObjectOfType<AudioListener>();
        deathUi.SetActive(false);
        winUi.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        EnnemyPatroling();
        if (!_audioFootstep.isPlaying && !_audioVoice.isPlaying)
        {
            _audioFootstep.Play();
            _audioVoice.Play();
        }
           

        if (isRaging){
          OnRageMode();
            _audioFootstep.clip = audioManager.bigboyRun;
            _audioVoice.clip = audioManager.bigboyRage;

        }
        else{
            animBigBoy.SetBool("run", false);
            _audioFootstep.clip = audioManager.bigboyWalk;
            _audioVoice.clip = audioManager.bigboyVoiceIdle;

        }
        if(isPlayerDead) agent.speed = 0;
    }

    private void FixedUpdate(){
        UpdateGrab();
    }

    private void EnnemyPatroling(){
        if(isDead()) return;
        if(!isWalkPointSet && !isRaging){
            SearchWalkingPoint();
        }
        else if(isWalkPointSet && !isRaging) {
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(walkPoint, path);
            if(path.status == NavMeshPathStatus.PathComplete){
                agent.SetDestination(walkPoint);
            }
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

        AudioListener.volume = 0;

        Invoke("WinScreen", 3f);
    }

    private bool isDead(){
        return health <=0;
    }

    private void WinScreen(){
        Debug.Log("Win");
        winUi.SetActive(true);
        Time.timeScale = 0;
        audioListener.enabled = false;
        Cursor.lockState = CursorLockMode.Confined;
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
        if (!isRaging)
        {
            _audioSFX.PlayOneShot(audioManager.bigboyRageFar);
        }
        walkPoint = NavMeshHelper.RandomCoordinateInRange(screamerPosition, 10f, navMeshArea);
         NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(walkPoint, path);
            if(path.status == NavMeshPathStatus.PathComplete){
                agent.SetDestination(walkPoint);
            }
        isRaging = true;

    }

    private void OnRageMode(){
        agent.speed = 10f;
        animBigBoy.SetBool("run", true);
    }

    private void UpdateGrab(){
        if(isDead()) return;
        Collider[] colliders = Physics.OverlapSphere(transform.position, view, playerMask);
        foreach(Collider col in colliders){
            Transform target = col.transform;
            Vector3 targetDirection = (target.position - transform.position).normalized;
            if(Vector3.Angle(transform.forward, targetDirection) < viewAngle / 2){
                RaycastHit hit;
                if(Physics.Raycast(transform.position, targetDirection, out hit, view)){
                    if(hit.collider.tag == "Player") {
                        ChasePlayer(hit.collider.gameObject);
                        return;
                    }
                    if(!isWalkPointSet) StopPlayerChase();
                }
            }
        }
    }

    private void StopPlayerChase(){
        SearchWalkingPoint();
        isRaging = false;
    }

    private void ChasePlayer(GameObject player){
        walkPoint = NavMeshHelper.GetCloseCoordinate(player.transform.position, navMeshArea);
        isWalkPointSet = true;
         NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(walkPoint, path);
            if(path.status == NavMeshPathStatus.PathComplete){
                agent.SetDestination(walkPoint);
            }
        if(Vector3.Distance(player.transform.position, transform.position) <= grabRange){
            KillPlayer(player);
            return;
        }
        isRaging = true;
    }

    private void KillPlayer(GameObject player)
    {
        GameObject cam = player.transform.GetChild(0).transform.gameObject;
        cam.GetComponent<CameraShake>().SetShake(false);
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

        Invoke("DeathScreen", 3f);
    }

    private void DeathScreen(){
        deathUi.SetActive(true);
        Time.timeScale = 0;
        audioListener.enabled = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, view);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, grabRange);
    }

}
