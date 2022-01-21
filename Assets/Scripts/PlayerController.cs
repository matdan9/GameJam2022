using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
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
    private float jogSpeed = 10000;
    [SerializeField]
    private float runSpeed = 15f;
    [SerializeField]
    private float _sideRayLength = 0.5f;
    [SerializeField]
    private float _sideJumpForce = 0.5f;
    [SerializeField]
    private float _extraDrag = 0.3f;
    [FormerlySerializedAs("_gripStrenght")] [SerializeField]
    private float _gripStrength = 100f;
    [SerializeField]
    private LayerMask _groundLayer;
    [SerializeField]
    private GameObject _laser;
    [SerializeField]
    private long _weaponCoolDown;
    [SerializeField] 
    private AudioClip laserAudio;
    [SerializeField] 
    private AudioClip movingSound;
    [SerializeField] 
    private AudioClip dieSound;
    [SerializeField]
    private AudioClip _jumpAudio;

    private bool _touchedGround = false;
    private bool _touchedLeft = false;
    private bool _touchedRight = false;
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
    private GameObject[] _spawnPoints;
    private long _nextShot = 0;
    private AudioSource _audio;

    private float x = 0;
    private float y = 0;

    public void OnStartClient()
    {
        Debug.Log("ok");
        _audio = this.GetComponent<AudioSource>();
        SetupCollider();
        RbSetup();
    }

    public void OnStartLocalPlayer()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _spawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
        Debug.Log("toto africa");
        SetupCam();
    }


    public void Kill()
    {
        if(_spawnPoints == null) _spawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
        this.transform.position = _spawnPoints[Random.Range(0, _spawnPoints.Length)].transform.position;
        AudioSource.PlayClipAtPoint(dieSound, transform.position);
    }

    private void Update()
    {
        if(_rbPlayer.velocity.magnitude >= 2 && (_touchedLeft||_touchedRight||_touchedGround) && !_audio.isPlaying) PlayWalkingSound();
        UpdateMovingInputs();
        UpdateMouseInput();
        _movement *= Time.deltaTime * 10f;
    }

    private void PlayWalkingSound()
    {
        _audio.volume = Random.Range(0.5f, 8f);
        _audio.pitch = Random.Range(0.7f, 1f);
        _audio.Play();
    }

    private void FixedUpdate()
    {
        UpdateGroundSensors();
        MouseLook();
        UpdatePlayerPosition();
    }
    

    private void UpdatePlayerPosition()
    {
        if (!_touchedGround && (!_touchedLeft && !_touchedRight)) _movement /= 3;
        _movement.y = _rbPlayer.velocity.y;
        _extraPhisicsVec *= _extraDrag;
        _movement += _extraPhisicsVec;
        if((_touchedLeft || _touchedRight) && !_touchedGround && _movement.y<0)
        {
            _movement.y /= _gripStrength;
        }
        _rbPlayer.velocity = _movement;
    }

    private void UpdateMouseInput()
    {
         x += Input.GetAxis("Mouse X") * mSensX * Time.deltaTime;
         y += Input.GetAxis("Mouse Y") * mSensY * -1 * Time.deltaTime;
        _mouseMovement.x += Input.GetAxis("Mouse X") * mSensX * Time.deltaTime;
        _mouseMovement.y += Input.GetAxis("Mouse Y") * mSensY * -1 * Time.deltaTime;
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

    private void UpdateMovingInputs()
    {
        _movingDirection = transform.eulerAngles.y * Mathf.Deg2Rad;
        if (Input.GetKey(KeyCode.A))
        {
            AKey();
        }
        if (Input.GetKey(KeyCode.D))
        {
            DKey();
        }
        if (Input.GetKey(KeyCode.W))
        {
            WKey();
        }
        if (Input.GetKey(KeyCode.S))
        {
            SKey();
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpaceKey();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 pos = new Vector3(_camPlayer.transform.position.x, _camPlayer.transform.position.y, _camPlayer.transform.position.z);
            Quaternion rot = new Quaternion(_camPlayer.transform.rotation.x, _camPlayer.transform.rotation.y, _camPlayer.transform.rotation.z, _camPlayer.transform.rotation.w);
            ShootLaser(pos, rot);
        }
        
        if (Input.GetKeyDown(KeyCode.LeftControl) && _touchedGround)
        {
            CtrlKey();
        }
        
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x, 1, this.transform.localScale.z);
        }
    }

    private void ShootLaser(Vector3 pos, Quaternion rot)
    {
        long currentTime = GetCurrentTime();
        if (_nextShot > currentTime) return;
        _nextShot = currentTime + _weaponCoolDown;
        GameObject laser = Instantiate(_laser, pos + Vector3.down * 0.1f, rot);
        RpcPlayGunSound(new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z));
        Destroy(laser, 1f);
        
    }


    private void RpcPlayGunSound(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(laserAudio, position);
    }
    
    private void CtrlKey()
    {
        _movingDirection = transform.eulerAngles.y * Mathf.Deg2Rad;
        _extraPhisicsVec += _movement / 4;
        _movement = Vector3.zero;
        this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y / 1.5f, this.transform.localScale.z);
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
        if (_touchedLeft)
        {
            var velocity = _rbPlayer.velocity;
            velocity = new Vector3(velocity.x, 0f, velocity.y);
            _rbPlayer.velocity = velocity;
            Vector3 p = new Vector3();
            float direction = transform.eulerAngles.y * Mathf.Deg2Rad + Mathf.PI / 4;
            p.z += Mathf.Cos(direction) * _sideJumpForce;
            p.x += Mathf.Sin(direction) * _sideJumpForce;
            _extraPhisicsVec += p;
            _rbPlayer.AddForce(Vector3.up * (jumpForce) , ForceMode.VelocityChange);
            AudioSource.PlayClipAtPoint(_jumpAudio, this.transform.position);
            return;
        }
        if (_touchedRight)
        {
            var velocity = _rbPlayer.velocity;
            velocity = new Vector3(velocity.x, 0f, velocity.y);
            _rbPlayer.velocity = velocity;
            Vector3 p = new Vector3();
            float direction = transform.eulerAngles.y * Mathf.Deg2Rad - Mathf.PI / 4;
            p.z += Mathf.Cos(direction) * _sideJumpForce;
            p.x += Mathf.Sin(direction) * _sideJumpForce;
            _extraPhisicsVec += p;
            _rbPlayer.AddForce(Vector3.up * (jumpForce), ForceMode.VelocityChange);
            AudioSource.PlayClipAtPoint(_jumpAudio, this.transform.position);
            return;
        }
    }
    
    private void AKey()
    {
        _movingDirection = transform.eulerAngles.y * Mathf.Deg2Rad - Mathf.PI / 2;
        _movement.z += Mathf.Cos(_movingDirection) * _movingSpeed;
        _movement.x += Mathf.Sin(_movingDirection) * _movingSpeed;
    }
    
    private void DKey()
    {
        _movingDirection = transform.eulerAngles.y * Mathf.Deg2Rad + Mathf.PI / 2;
        _movement.z += Mathf.Cos(_movingDirection) * _movingSpeed;
        _movement.x += Mathf.Sin(_movingDirection) * _movingSpeed;
    }
    
    private void WKey()
    {
        if (Input.GetKey(KeyCode.LeftShift)){
            _movingSpeed = runSpeed;
        }
        else if (_iswalking == false){
            _movingSpeed = jogSpeed;
        }
        _movingDirection = transform.eulerAngles.y * Mathf.Deg2Rad;
        _movement.z += Mathf.Cos(_movingDirection) * _movingSpeed;
        _movement.x += Mathf.Sin(_movingDirection) * _movingSpeed;
    }
    
    private void SKey()
    {
        _movingDirection = transform.eulerAngles.y * Mathf.Deg2Rad + Mathf.PI;
        _movement.z += Mathf.Cos(_movingDirection) * _movingSpeed;
        _movement.x += Mathf.Sin(_movingDirection) * _movingSpeed;
    } 

    private void SetupCam()
    {
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(0f, 0.34f, 0f);
        //Debug.Log(Camera.main.transform.localPosition);
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
        Vector3 xPos = new Vector3(0f, 0f, 0f);
        xPos.x += _collider.bounds.size.x / 2.1f;
        _touchedRight = Physics.Raycast(transform.TransformPoint(xPos), this.transform.right, out _jumpRay, _sideRayLength);
        xPos = new Vector3(0f, 0f, 0f);
        xPos.x -= _collider.bounds.size.x / 2.1f;
        _touchedLeft = Physics.Raycast(transform.TransformPoint(xPos), this.transform.right * -1, out _jumpRay, _sideRayLength);
    }

    public void AddJumpBoost(float boost)
    {
        _rbPlayer.AddForce(Vector3.up * boost, ForceMode.VelocityChange);
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
}
