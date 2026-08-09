using UnityEngine;

public class GraphicsSettingsPanel : MonoBehaviour
{
    [Header("默认设置")]
    public int defaultWidth = 1280;
    public int defaultHeight = 720;
    public FullScreenMode defaultFullScreenMode = FullScreenMode.Windowed;
    public int defaultRefreshRate = 60;

    private void Awake()
    {
        ApplySettings(defaultWidth, defaultHeight, defaultFullScreenMode, defaultRefreshRate);
    }

    public void ApplySettings(int width, int height, FullScreenMode fullScreenMode, int refreshRate = 60)
    {
        Screen.SetResolution(width, height, fullScreenMode, refreshRate);
    }

    public void ToggleFullscreen()
    {
        bool isFullscreen = Screen.fullScreen;
        FullScreenMode mode = isFullscreen ? FullScreenMode.Windowed : FullScreenMode.FullScreenWindow;
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, mode, Screen.currentResolution.refreshRate);
    }

    public void SetResolution(int width, int height)
    {
        ApplySettings(width, height, Screen.fullScreenMode, Screen.currentResolution.refreshRate);
    }

    public void SetWindowedMode()
    {
        ApplySettings(defaultWidth, defaultHeight, FullScreenMode.Windowed, defaultRefreshRate);
    }

    public void SetFullscreenMode()
    {
        ApplySettings(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.FullScreenWindow, Screen.currentResolution.refreshRate);
    }
}
