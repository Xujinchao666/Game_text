using UnityEngine;
public class UIIndicatorManager : MonoBehaviour
{
    public static UIIndicatorManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ShowInteractTip(Vector2 worldPos)
    {
        // 2D世界坐标转UI画布，显示【按E交互】图标
    }
    public void HideInteractTip()
    {
        // 隐藏交互提示UI
    }
}