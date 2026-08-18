using UnityEngine;

public static class ResourceLoader
{
    public static T LoadAsset<T>(string resourcePath) where T : Object
    {
        if (string.IsNullOrEmpty(resourcePath))
        {
            Debug.LogWarning("Resource path cannot be null or empty.");
            return null;
        }

        return Resources.Load<T>(resourcePath);
    }

    public static T[] LoadAllAssets<T>(string resourceFolder) where T : Object
    {
        if (string.IsNullOrEmpty(resourceFolder))
        {
            Debug.LogWarning("Resource folder cannot be null or empty.");
            return new T[0];
        }

        return Resources.LoadAll<T>(resourceFolder);
    }

    public static ResourceRequest LoadAssetAsync<T>(string resourcePath) where T : Object
    {
        if (string.IsNullOrEmpty(resourcePath))
        {
            Debug.LogWarning("Resource path cannot be null or empty.");
            return null;
        }

        return Resources.LoadAsync<T>(resourcePath);
    }

    public static bool TryLoadAsset<T>(string resourcePath, out T asset) where T : Object
    {
        asset = LoadAsset<T>(resourcePath);
        return asset != null;
    }
}
