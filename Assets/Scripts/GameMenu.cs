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
        playerController = player.GetComponent<PlayerController>();
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
        PlayerPrefs.SetFloat("fov", FOV.value);
        PlayerPrefs.SetFloat("sensX", sensitivityX.value);
        PlayerPrefs.SetFloat("sensY", sensitivityY.value);
        PlayerPrefs.SetFloat("mainVol", mainVolume.value);
        PlayerPrefs.SetFloat("musVol", musicVolume.value);
        PlayerPrefs.SetFloat("sfxVol", sfxVolume.value);
        SceneManager.LoadScene("MainMenu");
    }

    //Settings
    public Slider FOV, sensitivityX, sensitivityY, mainVolume, musicVolume, sfxVolume;
    public Toggle inverseX, inverseY;
    public Text FOVText, sensitivityXText, sensitivityYText, mainVolumeText, musicVolumeText, sfxVolumeText;
    public Camera cam;
    public GameObject player;
    PlayerController playerController;

    void Awake()
    {
        FOV.value = PlayerPrefs.GetFloat("fov");
        sensitivityX.value = PlayerPrefs.GetFloat("sensX");
        sensitivityY.value = PlayerPrefs.GetFloat("sensY");
        mainVolume.value = PlayerPrefs.GetFloat("mainVol");
        musicVolume.value = PlayerPrefs.GetFloat("musVol");
        sfxVolume.value = PlayerPrefs.GetFloat("sfxVol");
    }

    void Update()
    {
        SetTextValuesToSlider();
        playerController.SetFov((int)FOV.value);
        playerController.SetXMul(sensitivityX.value);
        if (inverseX.GetComponent<Toggle>().isOn && sensitivityX.value > 0) playerController.SetXMul(playerController.GetXMul() * -1);
        playerController.SetYMul(sensitivityY.value);
        if (inverseY.GetComponent<Toggle>().isOn && sensitivityY.value > 0) playerController.SetYMul(playerController.GetYMul() * -1);
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
