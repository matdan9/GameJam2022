using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    [SerializeField]
    GameObject player, gameMenu, settingsMenu, resume, settings, menu, UI;
    [SerializeField]
    Slider FOV, sensitivityX, sensitivityY;
    Toggle inverseX, inverseY;
    [SerializeField]
    Text fovText, sensitivityXText, sensitivityYText;
    Camera cam;

    PlayerController playerController;
    private AudioListener audioListener;

    void Awake()
    {
        FindGameObject();
        FindSlider();
        FindToggle();
        FindText();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        GetPrefs();
    }

    void Start()
    {
        resume.GetComponent<Button>().onClick.AddListener(ResumeButton);
        settings.GetComponent<Button>().onClick.AddListener(SettingsButton);
        menu.GetComponent<Button>().onClick.AddListener(MenuButton);
        playerController = player.GetComponent<PlayerController>();
        audioListener = GameObject.FindObjectOfType<AudioListener>();
    }

    void FindGameObject()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameMenu = GameObject.Find("GameMenu");
        settingsMenu = GameObject.Find("SettingsMenu");
        resume = GameObject.Find("Resume");
        settings = GameObject.Find("Settings");
        menu = GameObject.Find("Menu");
        UI = GameObject.Find("Slots");
    }

    void FindSlider()
    {
        FOV = GameObject.Find("FOV").GetComponent<Slider>();
        sensitivityX = GameObject.Find("Sensitivity X").GetComponent<Slider>();
        sensitivityY = GameObject.Find("Sensitivity Y").GetComponent<Slider>();
    }

    void FindToggle()
    {
        inverseX = GameObject.Find("Inverse X").GetComponent<Toggle>();
        inverseY = GameObject.Find("Inverse Y").GetComponent<Toggle>();
    }

    void FindText()
    {
        fovText = GameObject.Find("FOV Value").GetComponent<Text>();
        sensitivityXText = GameObject.Find("SensX Value").GetComponent<Text>();
        sensitivityYText = GameObject.Find("SensY Value").GetComponent<Text>();
    }

    void GetPrefs()
    {
        FOV.value = PlayerPrefs.GetFloat("fov");
        sensitivityX.value = PlayerPrefs.GetFloat("sensX");
        sensitivityY.value = PlayerPrefs.GetFloat("sensY");
    }

    void ResumeButton()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.lockState = CursorLockMode.Locked;
        gameMenu.SetActive(false);
        UI.SetActive(true);
        Time.timeScale = 1;
        audioListener.enabled = true;
        player.GetComponent<PlayerController>().EnableMouseLook(true);
        if(player.GetComponent<PlayerController>().isPickedUp()) player.GetComponent<PlayerController>().EnableShooting(true);
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
        SceneManager.LoadScene("MainMenu");
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
        fovText.text = FOV.value.ToString();
        sensitivityXText.text = sensitivityX.value.ToString();
        sensitivityYText.text = sensitivityY.value.ToString();
    }
}
