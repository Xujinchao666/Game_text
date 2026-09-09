using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuPanel : MonoBehaviour
{
    public Button startButton;
    public Button settingsButton;
    public Button exitButton;


    private void Start()
    {
        startButton.onClick.AddListener(() => 
        {
            SceneManager.LoadScene("MainScene");
        });
        settingsButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("SettingsScene");
        });
        exitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
