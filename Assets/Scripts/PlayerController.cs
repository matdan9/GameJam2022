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
    private float mSensX = 5f;
    [SerializeField]
    private float mSensY = 5f;
    [SerializeField]
    private float _jumpSensorLength = 0.1f;
    [SerializeField]
    private float jumpForce = 4;
    [SerializeField]
    private float jogSpeed = 10;
    [SerializeField]
    private float runSpeed = 15f;
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
    private AudioSource _audio;
    private Vector2 _movementInputs;
    private InputSystem inputSystem;

    private float x = 0;
    private float y = 0;

    private void Awake(){
        SetupControls();
    }

    private void SetupControls(){
        inputSystem = new InputSystem();
        inputSystem.PlayerMovements.Movements.performed += UpdateMovementInputs;
        inputSystem.PlayerMovements.Movements.canceled += UpdateMovementInputs;
        inputSystem.PlayerMovements.Look.performed += UpdateMouseInput;
    }

    private void Start()
    {
        _movingSpeed = jogSpeed;
        Cursor.lockState = CursorLockMode.Locked;
        SetupCam();
        _audio = this.GetComponent<AudioSource>();
        SetupCollider();
        RbSetup();
        SetupControls();
    }

    private void UpdateMovementInputs(InputAction.CallbackContext c){
        _movementInputs = c.ReadValue<Vector2>();
    }

    private void Update()
    {
        //if(_rbPlayer.velocity.magnitude >= 2 && !_audio.isPlaying) PlayWalkingSound();
        UpdateMovement();
        _movement *= Time.deltaTime * 10f;
    }

    private void FixedUpdate()
    {
        UpdateGroundSensors();
        MouseLook();
        UpdatePlayerPosition();
    }
    
    private void UpdatePlayerPosition()
    {
        if (!_touchedGround) _movement /= 3;
        _movement.y = _rbPlayer.velocity.y;
        _extraPhisicsVec *= _extraDrag;
        _movement += _extraPhisicsVec;
        _rbPlayer.velocity = _movement;
    }

    private void UpdateMouseInput(InputAction.CallbackContext c)
    {
        Vector2 vec = c.ReadValue<Vector2>();
        x += vec.x * mSensX * Time.deltaTime;
        y += vec.y * mSensY * -1 * Time.deltaTime;
        _mouseMovement.x += vec.x * mSensX * Time.deltaTime;
        _mouseMovement.y += vec.y * mSensY * -1 * Time.deltaTime;
    }

    private void MouseLook()
    {
        _rbPlayer.MoveRotation(Quaternion.Euler(0f, this.transform.rotation.eulerAngles.y + x, 0f));
        _camPlayer.transform.localRotation = Quaternion.Euler(_camPlayer.transform.localRotation.eulerAngles.x + y, 0f, 0f);
        x = 0;
        y = 0;
        _mouseMovement.x = 0;
        _mouseMovement.y = 0;
    }

    private void UpdateMovement()
    {
        Strafe(_movementInputs.x);
        Move(_movementInputs.y);
        /*
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 pos = new Vector3(_camPlayer.transform.position.x, _camPlayer.transform.position.y, _camPlayer.transform.position.z);
            Quaternion rot = new Quaternion(_camPlayer.transform.rotation.x, _camPlayer.transform.rotation.y, _camPlayer.transform.rotation.z, _camPlayer.transform.rotation.w);
            ShootLaser(pos, rot);
        }
        */
    }

    private void ShootGun (Vector3 pos, Quaternion rot)
    {
        long currentTime = GetCurrentTime();
        if (_nextShot > currentTime) return;
        _nextShot = currentTime + _weaponCoolDown;
        GameObject bullet = Instantiate(_bullet, pos + Vector3.down * 0.1f, rot);
        RpcPlayGunSound(new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z));
        Destroy(bullet, 1f);
    }

    private void CtrlKey()
    {
        //TODO crouch less sound smaller less visible
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
        _movingSpeed = jogSpeed;
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
        _rbPlayer.mass = 1f;
    }

    private void UpdateGroundSensors()
    {
        Vector3 pos = this.transform.position;
        pos.y -= _collider.bounds.size.y / 2.1f;
        _touchedGround = Physics.Raycast(pos, Vector3.down, out _jumpRay, _jumpSensorLength);
    }

    public void SetSens(int sens)
    {
        this.mSensX = sens;
        this.mSensY = sens;
    }

    public float GetSens()
    {
        return this.mSensX;
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
        _audio.volume = Random.Range(0.5f, 8f);
        _audio.pitch = Random.Range(0.7f, 1f);
        _audio.Play();
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
