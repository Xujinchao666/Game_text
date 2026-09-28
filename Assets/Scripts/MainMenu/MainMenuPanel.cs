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

    public string startButtonSceneToLoad = "MainScene";
    public string settingsButtonSceneToLoad = "SettingsScene";

    public Animator FadeAnim;
    public float fadeTime = 1f;

    private void Start()
    {
        startButton.onClick.AddListener(() => 
        {
            StartCoroutine(FadeAndLoadScene(startButtonSceneToLoad));
        });
        settingsButton.onClick.AddListener(() =>
        {
            StartCoroutine(FadeAndLoadScene(settingsButtonSceneToLoad));
        });
        exitButton.onClick.AddListener(() =>
        {
            FadeAnim.Play("FadeToWhite");
            Application.Quit();
        });
    }

    IEnumerator FadeAndLoadScene(string sceneName)
    {
        FadeAnim.Play("FadeToWhite");
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(sceneName);
    }
}
