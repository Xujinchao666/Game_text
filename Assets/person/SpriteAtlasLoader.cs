using System.Collections.Generic;
using UnityEngine;

public static class SpriteAtlasLoader
{
    private static readonly Dictionary<string, Sprite[]> SpriteCache = new Dictionary<string, Sprite[]>();

    public static Sprite[] LoadAtlasSprites(string atlasPath)
    {
        if (string.IsNullOrEmpty(atlasPath))
        {
            Debug.LogWarning("Atlas path cannot be null or empty.");
            return new Sprite[0];
        }

        if (SpriteCache.TryGetValue(atlasPath, out Sprite[] cachedSprites))
        {
            return cachedSprites;
        }

        Sprite[] sprites = Resources.LoadAll<Sprite>(atlasPath);
        if (sprites == null)
        {
            Debug.LogWarning($"Unable to load sprites from atlas: {atlasPath}");
            sprites = new Sprite[0];
        }

        SpriteCache[atlasPath] = sprites;
        return sprites;
    }

    public static Sprite GetSprite(string atlasPath, string spriteName)
    {
        Sprite[] sprites = LoadAtlasSprites(atlasPath);
        foreach (Sprite sprite in sprites)
        {
            if (sprite != null && sprite.name == spriteName)
                return sprite;
        }

        Debug.LogWarning($"Sprite '{spriteName}' not found in atlas '{atlasPath}'.");
        return null;
    }

    public static void ClearCache()
    {
        SpriteCache.Clear();
    }
}
