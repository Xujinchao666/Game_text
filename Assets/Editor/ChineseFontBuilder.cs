#if UNITY_EDITOR
using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using TMPro;
using UnityEngine.TextCore.LowLevel;

public static class ChineseFontBuilder
{
    private const string FontsFolder = "Assets/Fonts";
    private const string MainFontTtf = "Assets/Fonts/LXGWWenKaiLite-Regular.ttf";
    private const string FallbackFontOtf = "Assets/Fonts/NotoSansSC-Regular.otf";
    private const string CharsetFile = "Assets/Fonts/Characters/ChineseCharset_Common3500.txt";

    private const int AtlasWidth = 2048;
    private const int AtlasHeight = 2048;
    private const int PointSize = 90;
    private const int Padding = 9;

    // Atlas 放不下全部 7000 字时，使用多图集自动扩展。
    private const bool EnableMultiAtlas = true;

    public static void Build()
    {
        try
        {
            Debug.Log("[ChineseFontBuilder] === Starting Chinese font build ===");

            EnsureFolders();
            CleanupOldAssets();

            var mainAsset = BuildMainFont();
            var fallbackAsset = BuildFallbackFont();

            if (mainAsset != null && fallbackAsset != null)
            {
                // 设置 Fallback：主字体缺字时回退到 Noto Sans SC
                mainAsset.fallbackFontAssetTable = new System.Collections.Generic.List<TMP_FontAsset> { fallbackAsset };
                EditorUtility.SetDirty(mainAsset);

                Debug.Log("[ChineseFontBuilder] Fallback configured on main font asset.");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("[ChineseFontBuilder] === Chinese font build completed successfully ===");
        }
        catch (Exception e)
        {
            Debug.LogError("[ChineseFontBuilder] Build FAILED: " + e);
            throw;
        }
    }

    private static void CleanupOldAssets()
    {
        string[] paths =
        {
            FontsFolder + "/LXGWWenKaiLite - SDF.asset",
            FontsFolder + "/NotoSansSC - SDF.asset"
        };

        foreach (string path in paths)
        {
            if (AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path) != null)
            {
                AssetDatabase.DeleteAsset(path);
                Debug.Log("[ChineseFontBuilder] Removed old asset: " + path);
            }
        }

        AssetDatabase.SaveAssets();
    }

    private static void EnsureFolders()
    {
        if (!AssetDatabase.IsValidFolder(FontsFolder))
            AssetDatabase.CreateFolder("Assets", "Fonts");

        string charFolder = FontsFolder + "/Characters";
        if (!AssetDatabase.IsValidFolder(charFolder))
            AssetDatabase.CreateFolder(FontsFolder, "Characters");
    }

    private static string ReadCharset()
    {
        if (!File.Exists(CharsetFile))
        {
            Debug.LogError("[ChineseFontBuilder] Charset file not found: " + CharsetFile);
            return string.Empty;
        }

        // 字符集文件为 UTF-8 无 BOM。
        string content = File.ReadAllText(CharsetFile, new UTF8Encoding(false));

        // 去重，保持顺序。
        var seen = new System.Collections.Generic.HashSet<char>();
        var sb = new StringBuilder();
        foreach (char c in content)
        {
            if (char.IsWhiteSpace(c)) continue;
            if (seen.Add(c)) sb.Append(c);
        }

        string result = sb.ToString();
        Debug.Log("[ChineseFontBuilder] Loaded charset with " + result.Length + " unique characters.");
        return result;
    }

    private static TMP_FontAsset BuildMainFont()
    {
        Font font = AssetDatabase.LoadAssetAtPath<Font>(MainFontTtf);
        if (font == null)
        {
            Debug.LogError("[ChineseFontBuilder] Main font not found: " + MainFontTtf);
            return null;
        }

        Debug.Log("[ChineseFontBuilder] Creating main font asset from: " + MainFontTtf);

        TMP_FontAsset asset =
            TMP_FontAsset.CreateFontAsset(font, PointSize, Padding, GlyphRenderMode.SDFAA,
                AtlasWidth, AtlasHeight, AtlasPopulationMode.Dynamic, EnableMultiAtlas);

        if (asset == null)
        {
            Debug.LogError("[ChineseFontBuilder] Failed to create main TMP font asset. " +
                           "Check that \"Include Font Data\" is enabled in the font import settings.");
            return null;
        }

        asset.name = "LXGWWenKaiLite - SDF";

        string chars = ReadCharset();
        if (!string.IsNullOrEmpty(chars))
        {
            TryAddBatch(asset, chars, "main");
        }

        AssetDatabase.CreateAsset(asset, FontsFolder + "/LXGWWenKaiLite - SDF.asset");
        Debug.Log("[ChineseFontBuilder] Saved main font asset.");

        // 材质需单独保存为子资产。
        if (asset.material != null)
        {
            AssetDatabase.AddObjectToAsset(asset.material, FontsFolder + "/LXGWWenKaiLite - SDF.asset");
        }
        if (asset.atlasTextures != null && asset.atlasTextures.Length > 0)
        {
            for (int i = 0; i < asset.atlasTextures.Length; i++)
            {
                if (asset.atlasTextures[i] != null)
                    AssetDatabase.AddObjectToAsset(asset.atlasTextures[i], FontsFolder + "/LXGWWenKaiLite - SDF.asset");
            }
        }

        return asset;
    }

    private static TMP_FontAsset BuildFallbackFont()
    {
        Font font = AssetDatabase.LoadAssetAtPath<Font>(FallbackFontOtf);
        if (font == null)
        {
            Debug.LogError("[ChineseFontBuilder] Fallback font not found: " + FallbackFontOtf);
            return null;
        }

        Debug.Log("[ChineseFontBuilder] Creating fallback font asset from: " + FallbackFontOtf);

        TMP_FontAsset asset =
            TMP_FontAsset.CreateFontAsset(font, PointSize, Padding, GlyphRenderMode.SDFAA,
                AtlasWidth, AtlasHeight, AtlasPopulationMode.Dynamic, EnableMultiAtlas);

        if (asset == null)
        {
            Debug.LogError("[ChineseFontBuilder] Failed to create fallback TMP font asset.");
            return null;
        }

        asset.name = "NotoSansSC - SDF";

        // Fallback 字体保持 Dynamic 空：仅为主字体缺失的非常用字提供回退，
        // 运行时按需将缺失字符渲染进图集，避免重复预烘焙导致资产膨胀。
        TryAddBatch(asset, " .,!?，。！？…·—", "fallback");

        AssetDatabase.CreateAsset(asset, FontsFolder + "/NotoSansSC - SDF.asset");
        Debug.Log("[ChineseFontBuilder] Saved fallback font asset.");

        if (asset.material != null)
        {
            AssetDatabase.AddObjectToAsset(asset.material, FontsFolder + "/NotoSansSC - SDF.asset");
        }
        if (asset.atlasTextures != null && asset.atlasTextures.Length > 0)
        {
            for (int i = 0; i < asset.atlasTextures.Length; i++)
            {
                if (asset.atlasTextures[i] != null)
                    AssetDatabase.AddObjectToAsset(asset.atlasTextures[i], FontsFolder + "/NotoSansSC - SDF.asset");
            }
        }

        return asset;
    }

    private static void TryAddBatch(TMP_FontAsset asset, string characters, string label)
    {
        // 分批添加，避免单次重排开销过大；同时便于观察缺失字。
        string missing;
        bool ok = asset.TryAddCharacters(characters, out missing);

        if (!string.IsNullOrEmpty(missing))
        {
            Debug.LogWarning("[ChineseFontBuilder] [" + label + "] Missing " + missing.Length +
                             " characters (e.g. first 20): " + (missing.Length > 20 ? missing.Substring(0, 20) : missing));
        }

        Debug.Log("[ChineseFontBuilder] [" + label + "] Added " + (characters.Length - missing.Length) +
                  " / " + characters.Length + " characters. Missing: " + missing.Length +
                  ". Atlas count after: " + (asset.atlasTextures != null ? asset.atlasTextures.Length : 0));
    }
}
#endif
