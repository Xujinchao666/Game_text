using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }
    public float minTransitionSeconds = 0.2f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName, LoadSceneMode.Single, null));
    }

    public void LoadScene(int buildIndex)
    {
        if (buildIndex < 0 || buildIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogWarning($"Build index {buildIndex} is out of range.");
            return;
        }

        StartCoroutine(LoadSceneRoutine(buildIndex, LoadSceneMode.Single, null));
    }

    public void LoadSceneAsync(string sceneName, System.Action<float> onProgress = null, System.Action onComplete = null)
    {
        StartCoroutine(LoadSceneRoutine(sceneName, LoadSceneMode.Single, new SceneLoadCallbacks(onProgress, onComplete)));
    }

    public void LoadSceneAdditive(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName, LoadSceneMode.Additive, null));
    }

    public void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        LoadScene(currentScene.name);
    }

    private IEnumerator LoadSceneRoutine(string sceneName, LoadSceneMode mode, SceneLoadCallbacks callbacks)
    {
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneName, mode);
        if (asyncOp == null)
        {
            Debug.LogWarning($"Unable to load scene: {sceneName}");
            yield break;
        }

        asyncOp.allowSceneActivation = true;
        float startTime = Time.realtimeSinceStartup;

        while (!asyncOp.isDone)
        {
            callbacks?.OnProgress?.Invoke(asyncOp.progress);
            yield return null;
        }

        yield return new WaitForSeconds(Mathf.Max(0f, minTransitionSeconds - (Time.realtimeSinceStartup - startTime)));
        callbacks?.OnComplete?.Invoke();
    }

    private IEnumerator LoadSceneRoutine(int buildIndex, LoadSceneMode mode, SceneLoadCallbacks callbacks)
    {
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(buildIndex, mode);
        if (asyncOp == null)
        {
            Debug.LogWarning($"Unable to load scene index: {buildIndex}");
            yield break;
        }

        asyncOp.allowSceneActivation = true;
        float startTime = Time.realtimeSinceStartup;

        while (!asyncOp.isDone)
        {
            callbacks?.OnProgress?.Invoke(asyncOp.progress);
            yield return null;
        }

        yield return new WaitForSeconds(Mathf.Max(0f, minTransitionSeconds - (Time.realtimeSinceStartup - startTime)));
        callbacks?.OnComplete?.Invoke();
    }

    private class SceneLoadCallbacks
    {
        public System.Action<float> OnProgress { get; }
        public System.Action OnComplete { get; }

        public SceneLoadCallbacks(System.Action<float> onProgress, System.Action onComplete)
        {
            OnProgress = onProgress;
            OnComplete = onComplete;
        }
    }
}
