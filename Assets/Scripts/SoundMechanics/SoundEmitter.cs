using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class SoundEmitter : MonoBehaviour
{
    [SerializeField]
    private float triggerRange = 5;
    [SerializeField]
    private bool useExistingCollider = true;

    private SphereCollider triggerZone;
    private List<Sound> sounds;
    private float permanentNoise = 0;


    public float GetNoiseLevel(){
        float totalNoise = 0;
        sounds.RemoveAll(sound => sound.UpdateStatus());
        foreach(Sound sound in sounds){
            totalNoise += sound.GetCurrentNoiseLevel();
        }
        return totalNoise + permanentNoise;
    }

    public void addPermanentNoise(float noise){
        this.permanentNoise += noise;
    }

    private void Awake(){
        SetCollider();
        sounds = new List<Sound>();
    }

    private void SetCollider(){
        if(useExistingCollider) SetExistingCollider();
        else CreateCollider();
        triggerZone.isTrigger = true;
    }

    private void CreateCollider(){
        triggerZone = this.gameObject.AddComponent<SphereCollider>();
        triggerZone.radius = triggerRange;
    }

    private void SetExistingCollider(){
        triggerZone = this.gameObject.GetComponent<SphereCollider>();
        this.triggerRange = triggerZone.radius;
    }

    private void OnTriggerEnter(Collider other){
        if(other.tag == "Enemy"){
            float dist = Vector3.Distance(transform.position, other.transform.position);
            other.gameObject.GetComponent<SoundSeeker>().OnSoundTrigger(gameObject, dist, GetNoiseLevel());
            //other.gameObject.SendMessage("OnSoundTrigger", (int)this);
        }
    }
}
