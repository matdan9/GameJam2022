using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class SoundEmitter : MonoBehaviour
{
    [SerializeField]
    private float triggerRange = 10;
    [SerializeField]
    private LayerMask seekerMask;
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

    public void AddNoise(Sound d){
        sounds.Add(d);
    }

    public void AddPermanentNoise(float noise){
        this.permanentNoise += noise;
    }

    private void Awake(){
        SetMask();
        sounds = new List<Sound>();
    }

    private void FixedUpdate(){
        seekerMask = LayerMask.GetMask("Ennemis");
        Collider[] colliders = Physics.OverlapSphere(transform.position, triggerRange, 1 << 6);
        foreach(Collider other in colliders){
            float dist = Vector3.Distance(transform.position, other.transform.position);
            //other.gameObject.GetComponent<SoundSeeker>().OnSoundTrigger(gameObject, dist, GetNoiseLevel());
        }
    }

    private void SetMask(){
        if(seekerMask == null){
            seekerMask = LayerMask.GetMask("Ennemis");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, triggerRange);
    }
}
