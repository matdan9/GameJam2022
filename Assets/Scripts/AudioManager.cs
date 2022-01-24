using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("SFX")]
    [SerializeField]
    public AudioClip bigboyRage;
    [SerializeField]
    public AudioClip bigboyRun;
    [SerializeField]
    public AudioClip bigboyVoiceIdle;
    [SerializeField]
    public AudioClip bigboyWalk;
    [SerializeField]
    public AudioClip fireIdle;
    [SerializeField]
    public AudioClip ftConcrete;
    [SerializeField]
    public AudioClip ftDirt;
    [SerializeField]
    public AudioClip ftSand;
    [SerializeField]
    public AudioClip heroBreathNormal;
    [SerializeField]
    public AudioClip heroBreathScare;
    [SerializeField]
    public AudioClip heroFreezing;
    [SerializeField]
    public AudioClip heroHeartbeat;
    [SerializeField]
    public AudioClip shotgunFire;
    [SerializeField]
    public AudioClip shotgunReload;
    [SerializeField]
    public AudioClip spiderAlert;
    [SerializeField]
    public AudioClip spiderIdle;
    [SerializeField]
    public AudioClip spiderWalk;
    [SerializeField]
    public AudioClip takeBullets;
    [SerializeField]
    public AudioClip takeShotgun;
    [SerializeField]
    public AudioClip torchIdle;
    [SerializeField]
    public AudioClip torchIgnite;

    [Header("Ambiance")]
    [SerializeField]
    public AudioClip envForestWind;
    [SerializeField]
    public AudioClip envBeachOcean;
    [SerializeField]
    public AudioClip envBeachSeagull;
    [SerializeField]
    public AudioClip envForestBird;
    [SerializeField]
    public AudioClip envForestWolf;
    [SerializeField]
    public AudioClip envShedLight;

    [Header("Music")]
    [SerializeField]
    public AudioClip musicGameIdle;
    [SerializeField]
    public AudioClip musicGettingChase;
    [SerializeField]
    public AudioClip musicMenu;
    [SerializeField]




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void setAudio(AudioSource audio)
    {
        audio.loop = true;
        audio.playOnAwake = false;
        audio.maxDistance = 10;
        audio.spatialBlend = 1.0f;
        audio.rolloffMode = AudioRolloffMode.Linear;
    }
}
