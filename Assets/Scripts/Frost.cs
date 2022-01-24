using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Frost : MonoBehaviour
{
    [SerializeField]
    GameObject frost, deathUi;
    [SerializeField]
    Collider sphereCollider;
    [SerializeField]
    bool atFirecamp;
    [SerializeField]
    public float multiplier;
    [SerializeField]
    float frostValue;
    [SerializeField]
    bool cold = false;
    LightMecanic l;

    private AudioManager audioManager;
    private AudioSource _audioBreathing;
    private AudioListener audioListener;
    
    public void Awake()
    {
        deathUi = GameObject.Find("DeathScreenFrost");
    }

    public void Start()
    {
        frost = GameObject.FindGameObjectWithTag("Overlay");
        sphereCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<SphereCollider>();
        l = GameObject.FindGameObjectWithTag("Player").GetComponent<LightMecanic>();
        SetFrost(0);
        SetMultiplier(3);
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        _audioBreathing = this.gameObject.AddComponent<AudioSource>() as AudioSource;
        _audioBreathing.loop = true;
        _audioBreathing.clip = audioManager.heroFreezing;
        _audioBreathing.volume = 0.5f;


        audioListener = GameObject.FindObjectOfType<AudioListener>();
        deathUi.SetActive(false);
    }

    public void FixedUpdate()
    {
        frostValue = Mathf.Clamp(frostValue, 0, 255);
        Color32 temp = frost.GetComponent<RawImage>().color;
        temp.a = (byte)frostValue; //color.a is transparency
        frost.GetComponent<RawImage>().color = temp;

        if (l.torchValue <= 0 && !AtFirecamp()) SetCold(true);
        if (l.torchValue > 0) SetCold(false);
        if (cold) AddFrost(0.1f * multiplier);
        else RemoveFrost(0.1f * multiplier);

        PlayBreathingSound();
        DeathFrost();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Campfire")
        {
            SetCold(false);
            atFirecamp = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Campfire")
        {
            if(l.torchValue <= 0) SetCold(true);
            atFirecamp = false;
        }
    }

    public void SetFrost(float alpha)
    {
        frostValue = alpha;
    }

    public void AddFrost(float alpha)
    {
        frostValue += alpha;
    }

    public void RemoveFrost(float alpha)
    {
        frostValue -= alpha;
    }

    public void SetCold(bool status)
    {
        cold = status;
    }

    public void SetMultiplier(float value)
    {
        multiplier = value;
    }

    public bool AtFirecamp()
    {
        return atFirecamp;
    }

    private void DeathFrost(){
        if(frostValue >= 255.3f){
            deathUi.SetActive(true);
            Time.timeScale = 0;
            audioListener.enabled = false;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    private void PlayBreathingSound()
    {
        
        if(cold && !_audioBreathing.isPlaying)
        {
            _audioBreathing.Play();
        }
        else if(!cold)
        {
            _audioBreathing.Pause();
        }
    }
}
