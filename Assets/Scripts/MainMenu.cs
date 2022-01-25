using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //Play settings credits exit
    public GameObject play, settings, credits, exit;
    public GameObject settingsMenu, creditsMenu;

    void Awake()
    {
        FOV.value = PlayerPrefs.GetFloat("fov");
        sensitivityX.value = PlayerPrefs.GetFloat("sensX");
        sensitivityY.value = PlayerPrefs.GetFloat("sensY");
    }

    void Start()
    {
        play.GetComponent<Button>().onClick.AddListener(PlayButton);
        settings.GetComponent<Button>().onClick.AddListener(SettingsButton);
        credits.GetComponent<Button>().onClick.AddListener(CreditsButton);
        exit.GetComponent<Button>().onClick.AddListener(ExitButton);
    }

    void PlayButton()
    {
        PlayerPrefs.SetFloat("fov", FOV.value);
        PlayerPrefs.SetFloat("sensX", sensitivityX.value);
        PlayerPrefs.SetFloat("sensY", sensitivityY.value);
        SceneManager.LoadScene("Niveau_Foret");
        Time.timeScale = 1;
    }

    void SettingsButton()
    {
        settingsMenu.SetActive(true);
        creditsMenu.SetActive(false);
    }

    void CreditsButton()
    {
        creditsMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    void ExitButton()
    {
        Application.Quit();
    }

    //Settings
    public Slider FOV, sensitivityX, sensitivityY;
    public Toggle inverseX, inverseY;
    public Text FOVText, sensitivityXText, sensitivityYText;

    void Update()
    {
        SetTextValuesToSlider();
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            settingsMenu.SetActive(false);
            creditsMenu.SetActive(false);
        }
    }

    void SetTextValuesToSlider()
    {
        FOVText.text = FOV.value.ToString();
        sensitivityXText.text = sensitivityX.value.ToString();
        sensitivityYText.text = sensitivityY.value.ToString();
    }
}
