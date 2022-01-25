using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour
{
    private AudioListener audioListener;
    public bool menu = false;

    void Awake()
    {
        if(!menu) GetComponent<Button>().onClick.AddListener(Retry);
        else GetComponent<Button>().onClick.AddListener(MainMenu);
        audioListener = GameObject.FindObjectOfType<AudioListener>();
    }

    void Retry()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.lockState = CursorLockMode.Locked;
        audioListener.enabled = true;
        Time.timeScale = 1;
        SceneManager.LoadScene("Niveau_Foret");
    }

    void MainMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.lockState = CursorLockMode.Confined;
        SceneManager.LoadScene("MainMenu");
    }
}
