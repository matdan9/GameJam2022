using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour
{
    private AudioListener audioListener;

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(Retry);
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
}
