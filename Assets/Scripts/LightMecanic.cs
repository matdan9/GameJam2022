using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMecanic : MonoBehaviour
{

    public float torchValue = 7f;
    private float torchDurationSpeed = 0.0009f;
    private float lightIntensity;
    
    public bool isTorchAlight = false;
    private GameObject Torch;



    void Start(){
        Torch = GameObject.FindGameObjectWithTag("Torch");
        
    }

    // Update is called once per frame
    void Update()
    {
        LightDetection();
        BurningTorch();  
    }

    
    private void LightDetection(){

        Quaternion rotation = Quaternion.AngleAxis(5000 * Time.time, Vector3.up);
        Vector3  direction = transform.forward * 5;
        LayerMask layer = LayerMask.GetMask("Ennemis", "Obstacle"); 
       
        RaycastHit hit;
        Debug.DrawRay (transform.position, rotation * direction,Color.green);
        
        if(Physics.Raycast(transform.position, rotation * direction, out hit, 5f, layer)){
            
            if(hit.transform.tag == "Enemy" || hit.transform.tag == "ScreamerFix") {
                hit.transform.gameObject.GetComponent<Screamer>().EnnemyScream();
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

    public void RestartTorch(){
        torchValue = 7;
    }



}
