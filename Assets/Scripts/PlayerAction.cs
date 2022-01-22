using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAction: MonoBehaviour
{
    //Variables pour le nouveau input system
    private InputSystem inputActions;

    [SerializeField]private Vector2 input;
    private bool canInteract = false;

    private GameObject item;

    private int bulletCounter = 0;

    private GameObject pickupText;
    private GameObject interactText;
    
    private void Awake()
    {
        //Go get the new input system
        inputActions = new InputSystem();

        //Can reach the inputs for movements
        inputActions.PlayerMovements.Movements.performed += MovementsCharacter;
        //Put the inputs for movements values back to zero
        inputActions.PlayerMovements.Movements.canceled += MovementsCharacter;


        inputActions.PlayerActions.Pickup.performed += PickUp;
        
      

        inputActions.PlayerActions.Interact.performed += Interact;  
        
    
        
    }

    

    void Start(){

        pickupText = GameObject.Find("pickupText");
        interactText = GameObject.Find("interactText");
        pickupText.SetActive(false);
        interactText.SetActive(false);
        
    }
    
    void Update(){
        
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



    //------- this fonction detect if the WASD boutons are pressed -------//
    private void MovementsCharacter(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }



    //------- this fonction detect if the pickup(E) bouton is pressed -------//
     private void PickUp(InputAction.CallbackContext context)
    {   
        if(canInteract && item.transform.tag == "Bullet"){
            Destroy(item);
            item = null;
            bulletCounter ++;  
            pickupText.SetActive(false);

        }
        else if(canInteract && item.transform.tag == "Shotgun" )
        {
            Destroy(item);
            item = null;
        }
    }
    


    private void Interact(InputAction.CallbackContext context)
    {
        if(canInteract && item.transform.tag == "Campfire")
        {
            interactText.SetActive(false);
            GetComponent<LightMecanic>().RestartTorch();
            
        }  
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