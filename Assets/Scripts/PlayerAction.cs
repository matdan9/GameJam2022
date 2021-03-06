using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerAction: MonoBehaviour
{
    //Variables pour le nouveau input system
    private InputSystem inputActions;

    private bool canInteract = false;

    private GameObject item;

    private int bulletCounter = 0;
    private int maxBulletCount = 10;

    private GameObject bulletText;
    private GameObject pickupText;
    private GameObject interactText;
    private GameObject shotgunIcon;
    private GameObject shotgun;
    private GameObject player;

    private void Awake()
    {
        //Go get the new input system
        inputActions = new InputSystem();
        inputActions.PlayerActions.Pickup.performed += PickUp;
        inputActions.PlayerActions.Interact.performed += Interact;

        player = GameObject.FindGameObjectWithTag("Player");
        bulletText = GameObject.Find("bulletText");
        shotgunIcon = GameObject.Find("shotgunIcon");
        pickupText = GameObject.Find("pickupText");
        interactText = GameObject.Find("interactText");
        shotgun = GameObject.Find("HeldShotgun");
    }

    

    void Start(){
        pickupText.SetActive(false);
        interactText.SetActive(false);
        shotgun.SetActive(false);
        shotgunIcon.SetActive(false);
    }
    
    void Update(){
        bulletText.GetComponent<Text>().text = bulletCounter.ToString() + "/" + maxBulletCount.ToString();
    }
    

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.transform.tag == "Bullet" || collision.transform.tag == "Shotgun"){
            canInteract = true;
            item = collision.transform.gameObject;
            pickupText.SetActive(true);
            
        }
        else if(collision.transform.tag == "Campfire")
        {
            canInteract = true;
            item = collision.transform.gameObject;
            interactText.SetActive(true);
        }
    }



    void OnTriggerExit(Collider collision)
    {
        if(collision.transform.tag == "Campfire" || collision.transform.tag == "Bullet" || collision.transform.tag == "Shotgun"){
            canInteract = false;
            pickupText.SetActive(false);
            interactText.SetActive(false);
        }
    }

    //------- this fonction detect if the pickup(E) bouton is pressed -------//
     private void PickUp(InputAction.CallbackContext context)
    {   
        if(canInteract && item.transform.tag == "Bullet"){
            Destroy(item);
            item = null;
            bulletCounter ++;  
            pickupText.SetActive(false);
            canInteract = false;

        }
        else if(canInteract && item.transform.tag == "Shotgun" )
        {
            Destroy(item);
            item = null;
            pickupText.SetActive(false);
            canInteract = false;
            shotgunIcon.SetActive(true);
            shotgun.SetActive(true);
            player.GetComponent<PlayerController>().EnableShooting(true);
            player.GetComponent<PlayerController>().SetGunPickedUp(true);
        }
     }

    private void Interact(InputAction.CallbackContext context)
    {
        if(canInteract && item.transform.tag == "Campfire")
        {
            GetComponent<LightMechanic>().RestartTorch();
            
        }  
    }

    public int GetBulletCount()
    {
        return bulletCounter;
    }

    public void RemoveBullet(int val)
    {
        bulletCounter -= val;
    }

    //------- This fonction activate the input system -------//
    private void OnEnable()
    {
        inputActions.Enable();
    }

    //------- This fonction deactivate the input system -------//
    private void OnDisable()
    {
        inputActions.Disable();
    }
}