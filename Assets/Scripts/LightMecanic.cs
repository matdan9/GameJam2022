using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMecanic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion q = Quaternion.AngleAxis(5000 * Time.time, Vector3.up);
        Vector3  d = transform.forward * 5;
        Debug.DrawRay (transform.position, q*d,Color.green);

        LightDetection();
    }


    private void LightDetection(){

        Quaternion rotation = Quaternion.AngleAxis(5000 * Time.time, Vector3.up);
        Vector3  direction = transform.forward * 5;
        LayerMask layer = LayerMask.GetMask("Ennemis", "Obstacle"); 
       
        RaycastHit hit;
        
        if(Physics.Raycast(transform.position, rotation * direction, out hit, 5f, layer)){
            
            Debug.Log(hit.transform.name);
            //Ennemi scream !!!

        }
    }
}
