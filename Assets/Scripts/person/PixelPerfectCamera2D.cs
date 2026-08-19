using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PixelPerfectCamera2D : MonoBehaviour
{
    [Tooltip("像素每单位，确保贴图的Pixels Per Unit与此值一致。")]
    public int pixelsPerUnit = 32;

    [Tooltip("基准分辨率，用于计算缩放比。")]
    public Vector2Int referenceResolution = new Vector2Int(320, 180);

    [Tooltip("是否固定正交大小并锁定到像素栅格。")]
    public bool enforcePixelPerfect = true;

    [Tooltip("是否在屏幕尺寸变化时自动计算最接近的整数缩放级别。")]
    public bool autoScaleToScreen = true;

    [Tooltip("最小缩放倍数，避免出现低于参考分辨率的模糊。")]
    public int minScale = 1;

    [Tooltip("最大缩放倍数。")]
    public int maxScale = 8;

    private Camera _camera;
    private float _unitsPerPixel;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _camera.orthographic = true;
        UpdateCameraSettings();
    }

    private void LateUpdate()
    {
        if (enforcePixelPerfect)
        {
            SnapToPixelGrid();
        }
    }

    private void OnValidate()
    {
        _camera = GetComponent<Camera>();
        if (_camera != null)
        {
            _camera.orthographic = true;
        }
        UpdateCameraSettings();
    }

    public void UpdateCameraSettings()
    {
        if (_camera == null)
            _camera = GetComponent<Camera>();

        if (_camera == null)
            return;

        _camera.orthographic = true;
        int scale = CalculateRenderScale();
        _unitsPerPixel = 1f / pixelsPerUnit;
        _camera.orthographicSize = (Screen.height / (float)scale) / (2f * pixelsPerUnit);
    }

    public int CalculateRenderScale()
    {
        if (!autoScaleToScreen || referenceResolution.x <= 0 || referenceResolution.y <= 0)
            return 1;

        int scaleX = Mathf.FloorToInt((float)Screen.width / referenceResolution.x);
        int scaleY = Mathf.FloorToInt((float)Screen.height / referenceResolution.y);
        int scale = Mathf.Max(minScale, Mathf.Min(maxScale, Mathf.Min(scaleX, scaleY)));
        return Mathf.Max(scale, 1);
    }

    private void SnapToPixelGrid()
    {
        if (_camera == null)
            _camera = GetComponent<Camera>();

        if (_camera == null)
            return;

        _unitsPerPixel = 1f / pixelsPerUnit;
        Vector3 position = transform.position;
        position.x = Mathf.Round(position.x / _unitsPerPixel) * _unitsPerPixel;
        position.y = Mathf.Round(position.y / _unitsPerPixel) * _unitsPerPixel;
        transform.position = position;
    }
}
