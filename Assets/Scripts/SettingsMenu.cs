using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider FOV, sensitivityX, sensitivityY, mainVolume, musicVolume, sfxVolume;
    public Toggle inverseX, inverseY;
    public Text FOVText, sensitivityXText, sensitivityYText, mainVolumeText, musicVolumeText, sfxVolumeText;
    public Camera cam;
    public GameObject player;
    PlayerController playerController;

    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
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