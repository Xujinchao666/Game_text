using UnityEngine;
public class StoryBranchManager : MonoBehaviour
{
    public static StoryBranchManager Instance;
    private void Awake()
    {
        // 2D单例，防止多副本
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool HasMadeChoice(StoryChoice choice)
    {
        // 后续接入EasySave存档读取分支记录
        return false;
    }
}