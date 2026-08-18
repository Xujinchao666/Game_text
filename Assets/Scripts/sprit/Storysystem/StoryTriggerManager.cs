using UnityEngine;
public class StoryTriggerManager : MonoBehaviour
{
    public static StoryTriggerManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void TriggerStory(string yarnNode, string recallAnimName, Transform focusTarget, StoryChoice choice)
    {
        // 调度2D像素镜头演出、播放回忆动画、启动对话、保存分支选择
    }
}