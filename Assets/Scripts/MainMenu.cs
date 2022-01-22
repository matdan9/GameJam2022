using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    //Play settings credits exit
    public GameObject play, settings, credits, exit;
    public GameObject settingsMenu, creditsMenu, title;

    void Start()
    {
        play.GetComponent<Button>().onClick.AddListener(PlayButton);
        settings.GetComponent<Button>().onClick.AddListener(SettingsButton);
        credits.GetComponent<Button>().onClick.AddListener(CreditsButton);
        exit.GetComponent<Button>().onClick.AddListener(ExitButton);
    }

    void PlayButton()
    {
        //SceneManager.LoadScene("SceneName");
    }

    void SettingsButton()
    {
        settingsMenu.SetActive(true);
        creditsMenu.SetActive(false);
        title.SetActive(false);
    }

    void CreditsButton()
    {
        creditsMenu.SetActive(true);
        settingsMenu.SetActive(false);
        title.SetActive(false);
    }

    void ExitButton()
    {
        Application.Quit();
    }

    //Settings
    public Slider FOV, sensitivityX, sensitivityY, mainVolume, musicVolume, sfxVolume;
    public Toggle inverseX, inverseY;
    public Text FOVText, sensitivityXText, sensitivityYText, mainVolumeText, musicVolumeText, sfxVolumeText;

    void Update()
    {
        SetTextValuesToSlider();
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            settingsMenu.SetActive(false);
            creditsMenu.SetActive(false);
            title.SetActive(true);
        }
    }

    void SetTextValuesToSlider()
    {
        FOVText.text = FOV.value.ToString();
        sensitivityXText.text = sensitivityX.value.ToString();
        sensitivityYText.text = sensitivityY.value.ToString();
        mainVolumeText.text = mainVolume.value.ToString();
        musicVolumeText.text = musicVolume.value.ToString();
        sfxVolumeText.text = sfxVolume.value.ToString();
    }
}
