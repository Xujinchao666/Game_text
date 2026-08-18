using UnityEngine;
public class YarnDialogueManager : MonoBehaviour
{
    public static YarnDialogueManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartDialogue(string nodeName)
    {
        // 对接Yarn Spinner对话框、打字机效果
    }
}