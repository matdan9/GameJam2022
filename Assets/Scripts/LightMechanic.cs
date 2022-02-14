using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMechanic : MonoBehaviour
{

    public float torchValue = 7f;
    private float torchDurationSpeed = 0.01f;
    private float lightIntensity;
    private float maxIntensity = 7f;
    
    public bool isTorchAlight = false;
    [SerializeField]
    private GameObject torch, torchFlame, torchFireParticle, torchSmoke;

    private AudioManager audioManager;
    private AudioSource _audioFire;

    GameManager gameManager;

    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Start() {
        GetObjects();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        _audioFire = this.gameObject.AddComponent<AudioSource>() as AudioSource;
        _audioFire.loop = true;
        _audioFire.clip = audioManager.fireIdle;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        torchValue = Mathf.Clamp(torchValue, 0f, 7f);
        BurningTorch();
        torchFlame.transform.localScale = flameScale();
    }

    private void GetObjects()
    {
        torch = gameManager.GetTorchObject();
        torchFlame = gameManager.GetTorchFlameObject();
        torchFireParticle = gameManager.GetTorchFireParticleObject();
        torchSmoke = gameManager.GetTorchSmokeObject();
    }

    private void BurningTorch(){
        torchValue -= torchDurationSpeed;
        lightIntensity = torch.GetComponent<Light>().intensity = torchValue;

        if(torchValue <= 0){
            isTorchAlight = false;
            _audioFire.Pause();
        }
        else{
            isTorchAlight = true;
            if(isTorchAlight && !_audioFire.isPlaying)
            {
                _audioFire.Play();
            }
        }
    }

    private Vector3 flameScale()
    {
        float size = ((3f * torchValue) / 7f);
        return new Vector3(size, size, size);
    }

    public float GetIntensity()
    {
        return lightIntensity;
    }


    public float GetMaxIntensity()
    {
        return maxIntensity;
    }

    public float IntensityRatio()
    {
        if (torchValue <= 0) return 0.01f;
        return torchValue / maxIntensity;
    }

    public void RestartTorch(){
        torchValue = 7;
        torchFlame.GetComponent<ParticleSystem>().Play();
        torchFireParticle.GetComponent<ParticleSystem>().Play();
        torchSmoke.GetComponent<ParticleSystem>().Play();
        _audioFire.PlayOneShot(audioManager.torchIgnite);
        _audioFire.volume = 0.15f;
    }
}
