using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{
    public Frost frost;
    void Awake()
    {
        frost = GameObject.Find("frostOverlay").GetComponent<Frost>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player") frost.SetCold(false);
        Debug.Log("Enter");
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player") frost.SetCold(true);
        Debug.Log("Exit");
    }
}
