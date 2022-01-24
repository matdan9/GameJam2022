using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMecanic : MonoBehaviour
{

    public float torchValue = 7f;
    private float torchDurationSpeed = 0.001f;
    private float lightIntensity;
    
    public bool isTorchAlight = false;
    [SerializeField]
    private GameObject Torch, torchFlame, torchFireParticle, torchSmoke;



    void Start(){
        Torch = GameObject.FindGameObjectWithTag("Torch");
        torchFlame = GameObject.Find("TorchFlame");
        torchFireParticle = GameObject.Find("TorchFireParticle");
        torchSmoke = GameObject.Find("TorchSmoke");
    }

    // Update is called once per frame
    void Update()
    {
        torchValue = Mathf.Clamp(torchValue, 0f, 7f);
        LightDetection();
        BurningTorch();
        torchFlame.transform.localScale = flameScale();
    }

    
    private void LightDetection(){

        Vector3 originRay = new Vector3(transform.position.x, transform.position.y -1, transform.position.z);
        Quaternion rotation = Quaternion.AngleAxis(2000 * Time.time, Vector3.up);
        Vector3  direction = transform.forward * 5;
        LayerMask layer = LayerMask.GetMask("Ennemis", "Obstacle"); 
       
        RaycastHit hit;
        Debug.DrawRay (originRay, rotation * direction,Color.green);
        
        
        if(Physics.Raycast(originRay, rotation * direction, out hit, 5f, layer) && isTorchAlight){
            
            if(hit.transform.tag == "Enemy" || hit.transform.tag == "ScreamerFix") {
                hit.collider.gameObject.GetComponent<Screamer>().EnnemyScream();
            }
        }
    }

    private void BurningTorch(){
        torchValue -= torchDurationSpeed;
        lightIntensity = Torch.GetComponent<Light>().intensity = torchValue;


        if(torchValue <= 0){
            isTorchAlight = false;
        }else{
            isTorchAlight = true;
        }
    }

    private Vector3 flameScale()
    {
        float size = ((3f * torchValue) / 7f);
        return new Vector3(size, size, size);
    }

    public void RestartTorch(){
        torchValue = 7;
        torchFlame.GetComponent<ParticleSystem>().Play();
        torchFireParticle.GetComponent<ParticleSystem>().Play();
        torchSmoke.GetComponent<ParticleSystem>().Play();
    }



}
