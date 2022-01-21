using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAction
{
    //Variables pour le nouveau input system
    private InputSystem inputActions;

    private Vector2 input;
    
    private void Awake()
    {
        //Va chercher le nouveau input system
        inputActions = new InputSystem();

        //Permet d'aller chercher les inputs des touches pour le mouvement
        inputActions.PlayerMovements.Movements.performed += MovementsCharacter;
        //Remet les valeur a 0 lorsqu'on relache la touche
        
    }

    //------- Cette fonction detecte si les boutons WASD sont enfonce -------//
     private void MovementsCharacter(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }



      //------- Cette fonction active l'input system -------//
    private void OnEnable()
    {
        inputActions.Enable();
    }

    //------- Cette fonction desactive l'input system -------//
    private void OnDisable()
    {
        inputActions.Disable();
    }
}