using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;
using Random = UnityEngine.Random;

public class PlayerController: MonoBehaviour
{
    [SerializeField]
    private float mSensX = 10f;
    [SerializeField]
    private float xMul = 1f;
    [SerializeField]
    private float mSensY = 5f;
    [SerializeField]
    private float yMul = 1f;
    [SerializeField]
    private float _jumpSensorLength = 0.1f;
    [SerializeField]
    private float jumpForce = 4;
    [SerializeField]
    private float jogSpeed = 50;
    [SerializeField]
    private float crouchSpeed = 2;
    [SerializeField]
    private float runSpeed = 8f;
    [SerializeField]
    private float _extraDrag = 0.3f;
    [SerializeField]
    private LayerMask _groundLayer;
    [SerializeField]
    private GameObject _bullet;
    [SerializeField]
    private long _weaponCoolDown;
    [SerializeField] 
    private AudioClip gunAudio;
    [SerializeField] 
    private AudioClip movingSound;
    [SerializeField] 
    private AudioClip dieSound;
    [SerializeField]
    private AudioClip _jumpAudio;

    private bool _touchedGround = false;
    private float _movingSpeed;
    private bool _iswalking = false;
    private float _movingDirection = 0;
    public LayerMask ground;
    private Quaternion _viewAngle;
    private Rigidbody _rbPlayer;
    private Vector3 _movement = new Vector3();
    private RaycastHit _lookingRay;
    private RaycastHit _jumpRay;
    private Camera _camPlayer;
    private Ray _look;
    private CapsuleCollider _collider;
    private Vector2 _mouseMovement = new Vector2();
    private Vector3 _extraPhisicsVec = new Vector3();
    private long _nextShot = 0;
    private int bulletCount = 0;
    private Vector2 _movementInputs;
    private InputSystem inputSystem;
    private SoundEmitter soundEmitter;
    private float totalSound;
    private bool enableMouseLook = true;
    private bool enableShooting = false;
    private bool gunPickedUp = false;
    private AudioSource _audioFootstep;
    private AudioSource _audioSFX;
    private AudioManager audioManager;

    [SerializeField]
    GameObject light, smoke;

    private void Awake(){
        SetupControls();
        light = GameObject.Find("GunLight");
        smoke = GameObject.Find("Smoke");
    }

    private void SetupControls(){
        inputSystem = new InputSystem();
        inputSystem.PlayerMovements.Movements.performed += UpdateMovementInputs;
        inputSystem.PlayerMovements.Movements.canceled += UpdateMovementInputs;
        inputSystem.PlayerMovements.Look.performed += UpdateMouseInput;
        inputSystem.PlayerMovements.Crouch.performed += Crouch;
        inputSystem.PlayerMovements.Crouch.canceled += Crouch;
        inputSystem.PlayerMovements.Shoot.performed += ShootGun;
    }

    private void Start()
    {
        _movingSpeed = jogSpeed;
        Cursor.lockState = CursorLockMode.Locked;
        SetupCam();
        SetupCollider();
        RbSetup();
        SetupControls();
        _audioFootstep = this.gameObject.AddComponent<AudioSource>() as AudioSource;
        _audioSFX = this.gameObject.AddComponent<AudioSource>() as AudioSource;
        soundEmitter = gameObject.GetComponent<SoundEmitter>();
        soundEmitter.AddPermanentNoise(5);
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    private void UpdateMovementInputs(InputAction.CallbackContext c){
        _movementInputs = c.ReadValue<Vector2>();
        if(_movementInputs.x == 0 || _movementInputs.y == 0){
            return;
        }
    }

    private void Update()
    {
        ChangeFootstepsSound();
        if(_rbPlayer.velocity.magnitude >= 1 && !_audioFootstep.isPlaying) {
            //PlayWalkingSound();
            _audioFootstep.Play();
        }
        else if(_rbPlayer.velocity.magnitude <= 1)
        {
            _audioFootstep.Pause();
        }
        totalSound += 5 * _rbPlayer.velocity.magnitude;
        soundEmitter.AddNoise(new Sound(0.1f, totalSound, 1));
        totalSound = 0;
        bulletCount = this.GetComponent<PlayerAction>().GetBulletCount();
    }

    private void FixedUpdate()
    {
        UpdateGroundSensors();
        UpdateMovement();
        UpdatePlayerPosition();
    }

    private void UpdatePlayerPosition()
    {
        if (!_touchedGround) _movement /= 3;
        _movement.y = _rbPlayer.velocity.y;
        _extraPhisicsVec *= _extraDrag;
        _movement += _extraPhisicsVec;
        _rbPlayer.velocity = _movement;
        _movement.x -= _movement.x;
        _movement.z -= _movement.z;
    }

    private void UpdateMouseInput(InputAction.CallbackContext c)
    {
        Vector2 vec = c.ReadValue<Vector2>();
        _mouseMovement.x += vec.x * mSensX * 0.01f * xMul;
        _mouseMovement.y += vec.y * mSensY * -1 * 0.01f * yMul;
        MouseLook();
    }

    private void MouseLook()
    {
        if(!enableMouseLook) return;
        _rbPlayer.MoveRotation(Quaternion.Euler(0f, this.transform.rotation.eulerAngles.y + _mouseMovement.x, 0f));
        _camPlayer.transform.localRotation = Quaternion.Euler(_camPlayer.transform.localRotation.eulerAngles.x + _mouseMovement.y, 0f, 0f);
        _mouseMovement.x = 0;
        _mouseMovement.y = 0;
    }

    private void UpdateMovement()
    {
        Strafe(_movementInputs.x);
        Move(_movementInputs.y);
    }

    private void ShootGun (InputAction.CallbackContext c)
    {
        Vector3 pos = new Vector3(_camPlayer.transform.position.x, _camPlayer.transform.position.y, _camPlayer.transform.position.z);
        Quaternion rot = new Quaternion(_camPlayer.transform.rotation.x, _camPlayer.transform.rotation.y, _camPlayer.transform.rotation.z, _camPlayer.transform.rotation.w);
        long currentTime = GetCurrentTime();
        if (!enableShooting) return;
        if (bulletCount == 0) return;
        if (_nextShot > currentTime) return;
        this.GetComponent<PlayerAction>().RemoveBullet(1);
        _nextShot = currentTime + _weaponCoolDown;
        GameObject bullet = Instantiate(_bullet, pos, rot);
        light.GetComponent<ParticleSystem>().Play();
        smoke.GetComponent<ParticleSystem>().Play();
        //RpcPlayGunSound(new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z));
        Destroy(bullet, 10f);

        _audioSFX.PlayOneShot(audioManager.shotgunFire);
    }

    private void Crouch(InputAction.CallbackContext c)
    {
        if(c.canceled){
            this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y * 1.5f, this.transform.localScale.z);
            _movingSpeed = jogSpeed;
            return;
        }
        _movingSpeed = crouchSpeed;
        this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y / 1.5f, this.transform.localScale.z);
    }

    public void Kill()
    {
        //TODO kill the player
    }

    private void SpaceKey()
    {
        if(_touchedGround){
            _rbPlayer.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            _extraPhisicsVec += _movement/5;
            _movement = Vector3.zero;
            AudioSource.PlayClipAtPoint(_jumpAudio, this.transform.position);
            return;
        }
    }
    
    private void Strafe(float direction)
    {
        if(direction == 0) return;
        _movingDirection = transform.eulerAngles.y * Mathf.Deg2Rad;
        _movingDirection += ((direction > 0)? (float) Math.PI / 2 : (float) Math.PI / -2);
        _movement.z += Mathf.Cos(_movingDirection) * _movingSpeed;
        _movement.x += Mathf.Sin(_movingDirection) * _movingSpeed;
    }
    
    private void Move(float direction)
    {
        if(direction == 0) return;
        /*
        if (Input.GetKey(KeyCode.LeftShift)){
            _movingSpeed = runSpeed;
        }
        else if (_iswalking == false){
            _movingSpeed = jogSpeed;
        }
        */
        //_movingSpeed = jogSpeed;
        _movingDirection = transform.eulerAngles.y * Mathf.Deg2Rad;
        if(direction < 0) _movingDirection += Mathf.PI;
        _movement.z += Mathf.Cos(_movingDirection) * _movingSpeed;
        _movement.x += Mathf.Sin(_movingDirection) * _movingSpeed;
    }

    private void SetupCam()
    {
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(0f, 0.34f, 0f);
        _camPlayer = gameObject.GetComponentInChildren<Camera>();
    }
    
    private void SetupCollider()
    {
        _collider = gameObject.GetComponent<CapsuleCollider>();
        PhysicMaterial mat = new PhysicMaterial 
        {
            staticFriction = 0, frictionCombine = 0, 
            dynamicFriction = 0, name = "noFrictionMat"
            
        };
        _collider.material = mat;
    }

    private void RbSetup()
    {
        _rbPlayer = gameObject.AddComponent<Rigidbody>();
        _rbPlayer.freezeRotation = true;
        _rbPlayer.interpolation = RigidbodyInterpolation.Interpolate;
        _rbPlayer.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        _rbPlayer.mass = 3f;
    }

    private void UpdateGroundSensors()
    {
        Vector3 pos = this.transform.position;
        pos.y -= _collider.bounds.size.y / 2.1f;
        _touchedGround = Physics.Raycast(pos, Vector3.down, out _jumpRay, _jumpSensorLength);
    }

    private void ChangeFootstepsSound()
    {
        if (gameObject.transform.position.z > -235)
        {
            _audioFootstep.Pause();
            _audioFootstep.clip = audioManager.ftDirt;
        }
        else
        {
            _audioFootstep.Pause();
            _audioFootstep.clip = audioManager.ftSand;
        }
    }

    public void EnableMouseLook(bool b)
    {
        enableMouseLook = b;
    }

    public void EnableShooting(bool b)
    {
        enableShooting = b;
    }

    public bool isPickedUp()
    {
        return gunPickedUp;
    }

    public void SetGunPickedUp(bool b)
    {
        gunPickedUp = b;
    }

    public void SetSens(float sens)
    {
        this.mSensX = sens * xMul;
        this.mSensY = sens * yMul;
    }

    public void SetXMul(float mul)
    {
        this.xMul = mul;
    }

    public float GetXMul()
    {
        return xMul;
    }

    public void SetYMul(float mul)
    {
        this.yMul = mul;
    }

    public float GetYMul()
    {
        return yMul;
    }

    public float GetXSens()
    {
        return this.mSensX;
    }

    public float GetYSens()
    {
        return this.mSensY;
    }

    public void SetFov(int fov)
    {
        _camPlayer.fieldOfView = fov;
    }

    public int GetFov()
    {
        return (int) _camPlayer.fieldOfView;
    }
    
    public static long GetCurrentTime()
    {
        return (long)(Time.time * 1000f);
    }

    private void PlayWalkingSound()
    {
        /*_audio.volume = Random.Range(0.5f, 8f);
        _audio.pitch = Random.Range(0.7f, 1f);
        _audio.Play();*/
    }

    private void RpcPlayGunSound(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(gunAudio, position);
    }

    private void OnEnable()
    {
        inputSystem.Enable();
    }

    private void OnDisable()
    {
        inputSystem.Disable();
    }
}
