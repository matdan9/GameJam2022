using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 20f;
    private MeshCollider collider;
    private Vector3 movement;
    private Rigidbody rb;

    private void Start()
    {
        collider = GetComponent<MeshCollider>();
        collider.convex = true;
        collider.isTrigger = true;
        rb = this.gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        movement = new Vector3(speed, 0, 0);
    }

    private void Update()
    {
        this.transform.position += this.transform.forward * speed * Time.deltaTime;
    }
    
    private void OnTriggerEnter(Collider other){
        if(other.tag == "Enemy"){
            Debug.Log("hit enemy");
        }
        if(other.tag != "Player"){
            Destroy(this.gameObject);
        }
    }

}
