using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public GameObject gameMenu, settingsMenu, resume, settings, menu;

    void Start()
    {
        resume.GetComponent<Button>().onClick.AddListener(ResumeButton);
        settings.GetComponent<Button>().onClick.AddListener(SettingsButton);
        menu.GetComponent<Button>().onClick.AddListener(MenuButton);
    }

    void ResumeButton()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.lockState = CursorLockMode.Locked;
        gameMenu.SetActive(false);
    }

    void SettingsButton()
    {
        settingsMenu.SetActive(true);
    }

    void MenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
