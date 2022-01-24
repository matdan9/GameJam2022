using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EventListener : MonoBehaviour
{
    [SerializeField]
    GameObject player, gameMenu, settingsMenu, slots;

    private AudioListener audioListener;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameMenu = GameObject.Find("GameMenu");
        settingsMenu = GameObject.Find("SettingsMenu");
        slots = GameObject.Find("Slots");
    }

    void Start()
    {
        settingsMenu.SetActive(false);
        gameMenu.SetActive(false);

        audioListener = GameObject.FindObjectOfType<AudioListener>();
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
            player.GetComponent<PlayerController>().EnableShooting(true);
            Time.timeScale = 1;
            audioListener.enabled = true;
        }
        else if (Keyboard.current.escapeKey.wasPressedThisFrame && !gameMenu.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.lockState = CursorLockMode.Confined;
            gameMenu.SetActive(true);
            slots.SetActive(false);
            player.GetComponent<PlayerController>().EnableMouseLook(false);
            player.GetComponent<PlayerController>().EnableShooting(false);

            Time.timeScale = 0;
            audioListener.enabled = false;
        }
    }
}
