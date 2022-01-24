using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EventListener : MonoBehaviour
{
    [SerializeField]
    GameObject player, gameMenu, settingsMenu, slots;

    void Awake()
    {
        player = GameObject.Find("Player");
        gameMenu = GameObject.Find("GameMenu");
        settingsMenu = GameObject.Find("SettingsMenu");
        slots = GameObject.Find("Slots");
    }

    void Start()
    {
        settingsMenu.SetActive(false);
        gameMenu.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame && gameMenu.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.lockState = CursorLockMode.Locked;
            gameMenu.SetActive(false);
            settingsMenu.SetActive(false);
            slots.SetActive(true);
            player.GetComponent<PlayerController>().EnableMouseLook(true);
        }
        else if (Keyboard.current.escapeKey.wasPressedThisFrame && !gameMenu.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.lockState = CursorLockMode.Confined;
            gameMenu.SetActive(true);
            slots.SetActive(false);
            player.GetComponent<PlayerController>().EnableMouseLook(false);
        }
    }
}
