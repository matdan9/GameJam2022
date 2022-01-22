using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EventListener : MonoBehaviour
{
    public GameObject gameMenu, settingsMenu;

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame && gameMenu.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.lockState = CursorLockMode.Locked;
            gameMenu.SetActive(false);
            settingsMenu.SetActive(false);
        }
        else if (Keyboard.current.escapeKey.wasPressedThisFrame && !gameMenu.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.lockState = CursorLockMode.Confined;
            gameMenu.SetActive(true);
        }
    }
}
