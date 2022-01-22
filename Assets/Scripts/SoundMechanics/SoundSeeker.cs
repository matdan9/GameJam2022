using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SoundSeeker : MonoBehaviour
{

    //public delegate void SoundCallback(GameObject soundSource, float distance, float noiseLevel);
    //public SoundCallback soundCallback;

    public SoundEvent SoundTriggerEvent;

    void Start()
    {
    }

    void Update()
    {
    }

    public void OnSoundTrigger(GameObject soundSource, float distance, float noiseLevel){
        SoundTriggerEvent.Invoke(soundSource, distance, noiseLevel);
    }

    public void TriggerDemo(GameObject o, float d, float ss){
        Debug.Log(ss);
    }
}

[System.Serializable]
public class SoundEvent : UnityEvent<GameObject, float, float>
{
}
