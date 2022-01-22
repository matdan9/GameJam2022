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

    //options.GetButton().onClick.AddListener(/*Function name*/);

    void ResumeButton()
    {
        gameMenu.SetActive(false);
    }

    void SettingsButton()
    {

        Debug.Log("SETTINGS HAS BEEN PRESSED");
    }

    void MenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
