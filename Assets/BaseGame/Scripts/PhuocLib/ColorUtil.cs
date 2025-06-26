using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public enum ColorId
{
    None = -1,
    Color_blue,
    Color_cyan,
    Color_darkGreen,
    Color_green,
    Color_grey,
    Color_orange,
    Color_pink,
    Color_purple,
    Color_red,
    Color_yellow,
    Color10,
    Color_LightBlue
}

public static class ColorUtil
{
    #region Materials

    public static Color GetColorById(ColorId colorId)
    {
        switch (colorId)
        {
            case ColorId.Color_blue: return Color.blue;
            case ColorId.Color_cyan: return Color.cyan;
            case ColorId.Color_darkGreen: return Color.green;
            case ColorId.Color_green: return Color.green;
            case ColorId.Color_grey: return Color.gray;
            case ColorId.Color_orange: return Color.yellow;
            case ColorId.Color_pink: return Color.magenta;
            case ColorId.Color_purple: return Color.magenta;
            case ColorId.Color_red: return Color.red;
            case ColorId.Color_yellow: return Color.yellow;
            case ColorId.Color10: return Color.white;
            default: return Color.black;
        }
    }

#if UNITY_EDITOR
    public static void ApplyColorMaterialSkinedMeshRenderer(SkinnedMeshRenderer meshRenderer, ColorId colorId)
    {
        switch (colorId)
        {
            case ColorId.Color_blue:
                SetSkinedMeshRendererMaterialByName(meshRenderer, "jelly_blue");
                break;
            case ColorId.Color_cyan:
                SetSkinedMeshRendererMaterialByName(meshRenderer, "jelly_cyan");
                break;
            case ColorId.Color_darkGreen:
                SetSkinedMeshRendererMaterialByName(meshRenderer, "jelly_darkgreen");
                break;
            case ColorId.Color_green:
                SetSkinedMeshRendererMaterialByName(meshRenderer, "jelly_green");
                break;
            case ColorId.Color_grey:
                SetSkinedMeshRendererMaterialByName(meshRenderer, "jelly_grey");
                break;
            case ColorId.Color_orange:
                SetSkinedMeshRendererMaterialByName(meshRenderer, "jelly_orange");
                break;
            case ColorId.Color_pink:
                SetSkinedMeshRendererMaterialByName(meshRenderer, "jelly_pink");
                break;
            case ColorId.Color_purple:
                SetSkinedMeshRendererMaterialByName(meshRenderer, "jelly_purple");
                break;
            case ColorId.Color_red:
                SetSkinedMeshRendererMaterialByName(meshRenderer, "jelly_red");
                break;
            case ColorId.Color_yellow:
                SetSkinedMeshRendererMaterialByName(meshRenderer, "jelly_yellow");
                break;
            case ColorId.Color10:
            default:
                SetSkinedMeshRendererMaterialByName(meshRenderer, "jelly_grey");
                break;
        }
    }
    public static void ApplyColorMaterialMeshRenderer(MeshRenderer meshRenderer, ColorId colorId)
    {
        switch (colorId)
        {
            case ColorId.Color_blue:
                SetMeshRendererMaterialByName(meshRenderer, "jelly_blue");
                break;
            case ColorId.Color_cyan:
                SetMeshRendererMaterialByName(meshRenderer, "jelly_cyan");
                break;
            case ColorId.Color_darkGreen:
                SetMeshRendererMaterialByName(meshRenderer, "jelly_darkgreen");
                break;
            case ColorId.Color_green:
                SetMeshRendererMaterialByName(meshRenderer, "jelly_green");
                break;
            case ColorId.Color_grey:
                SetMeshRendererMaterialByName(meshRenderer, "jelly_grey");
                break;
            case ColorId.Color_orange:
                SetMeshRendererMaterialByName(meshRenderer, "jelly_orange");
                break;
            case ColorId.Color_pink:
                SetMeshRendererMaterialByName(meshRenderer, "jelly_pink");
                break;
            case ColorId.Color_purple:
                SetMeshRendererMaterialByName(meshRenderer, "jelly_purple");
                break;
            case ColorId.Color_red:
                SetMeshRendererMaterialByName(meshRenderer, "jelly_red");
                break;
            case ColorId.Color_yellow:
                SetMeshRendererMaterialByName(meshRenderer, "jelly_yellow");
                break;
            case ColorId.Color10:
            default:
                SetMeshRendererMaterialByName(meshRenderer, "jelly_grey");
                break;
        }
    }
    public static void ApplyColorParticalSystemMeshRenderer(ParticleSystemRenderer particleSystemRenderer,ColorId colorId)
    {
        switch (colorId)
        {
            case ColorId.Color_blue:
                SetParticalSystemMaterialByName(particleSystemRenderer, "jelly_blue");
                break;
            case ColorId.Color_cyan:
                SetParticalSystemMaterialByName(particleSystemRenderer, "jelly_cyan");
                break;
            case ColorId.Color_darkGreen:
                SetParticalSystemMaterialByName(particleSystemRenderer, "jelly_darkgreen");
                break;
            case ColorId.Color_green:
                SetParticalSystemMaterialByName(particleSystemRenderer, "jelly_green");
                break;
            case ColorId.Color_grey:
                SetParticalSystemMaterialByName(particleSystemRenderer, "jelly_grey");
                break;
            case ColorId.Color_orange:
                SetParticalSystemMaterialByName(particleSystemRenderer, "jelly_orange");
                break;
            case ColorId.Color_pink:
                SetParticalSystemMaterialByName(particleSystemRenderer, "jelly_pink");
                break;
            case ColorId.Color_purple:
                SetParticalSystemMaterialByName(particleSystemRenderer, "jelly_purple");
                break;
            case ColorId.Color_red:
                SetParticalSystemMaterialByName(particleSystemRenderer, "jelly_red");
                break;
            case ColorId.Color_yellow:
                SetParticalSystemMaterialByName(particleSystemRenderer, "jelly_yellow");
                break;
            case ColorId.Color10:
            default:
                SetParticalSystemMaterialByName(particleSystemRenderer, "jelly_grey");
                break;
        }
    }
    private static void SetSkinedMeshRendererMaterialByName(SkinnedMeshRenderer meshRenderer, string materialName)
    {
        string[] guids = AssetDatabase.FindAssets(materialName);
        var mats = new List<Material>();

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var material = AssetDatabase.LoadAssetAtPath<Material>(assetPath);
            if (material == null) continue;
            mats.Add(material);
        }

        meshRenderer.materials = mats.ToArray();
    }
    private static void SetMeshRendererMaterialByName(MeshRenderer meshRenderer, string materialName)
    {
        string[] guids = AssetDatabase.FindAssets(materialName);
        var mats = new List<Material>();

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var material = AssetDatabase.LoadAssetAtPath<Material>(assetPath);
            if (material == null) continue;
            mats.Add(material);
        }

        meshRenderer.materials = mats.ToArray();
    }
    private static void SetParticalSystemMaterialByName(ParticleSystemRenderer particleSystemRenderer, string materialName)
    {
        string[] guids = AssetDatabase.FindAssets(materialName);
        var mats = new List<Material>();

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var material = AssetDatabase.LoadAssetAtPath<Material>(assetPath);
            if (material == null) continue;
            mats.Add(material);
        }

        particleSystemRenderer.materials = mats.ToArray();
    }
#endif

    #endregion

    #region Sprites
    public static void SetColorToImage(UnityEngine.UI.Image image, ColorId colorId)
    {
        if (image == null) return;
        switch (colorId)
        {
            case ColorId.Color_blue:
                image.color = GetColorById(ColorId.Color_blue);
                break;
            case ColorId.Color_cyan:
                image.color = GetColorById(ColorId.Color_cyan);
                break;
            case ColorId.Color_darkGreen:
                image.color = GetColorById(ColorId.Color_darkGreen);
                break;
            case ColorId.Color_green:
                image.color = GetColorById(ColorId.Color_green);
                break;
            case ColorId.Color_grey:
                image.color = GetColorById(ColorId.Color_grey);
                break;
            case ColorId.Color_orange:
                image.color = GetColorById(ColorId.Color_orange);
                break;
            case ColorId.Color_pink:
                image.color = GetColorById(ColorId.Color_pink);
                break;
            case ColorId.Color_purple:
                image.color = GetColorById(ColorId.Color_purple);
                break;
            case ColorId.Color_red:
                image.color = GetColorById(ColorId.Color_red);
                break;
            case ColorId.Color_yellow:
                image.color = GetColorById(ColorId.Color_yellow);
                break;
            case ColorId.Color10:
            default:
                image.color = GetColorById(ColorId.Color10);
                break;
        }
    }
    public static void SetColorToSpriteRenderer(SpriteRenderer spriteRenderer, ColorId colorId)
    {
        if (spriteRenderer == null) return;
        switch (colorId)
        {
            case ColorId.Color_blue:
                spriteRenderer.color = GetColorById(ColorId.Color_blue);
                break;
            case ColorId.Color_cyan:
                spriteRenderer.color = GetColorById(ColorId.Color_cyan);
                break;
            case ColorId.Color_darkGreen:
                spriteRenderer.color = GetColorById(ColorId.Color_darkGreen);
                break;
            case ColorId.Color_green:
                spriteRenderer.color = GetColorById(ColorId.Color_green);
                break;
            case ColorId.Color_grey:
                spriteRenderer.color = GetColorById(ColorId.Color_grey);
                break;
            case ColorId.Color_orange:
                spriteRenderer.color = GetColorById(ColorId.Color_orange);
                break;
            case ColorId.Color_pink:
                spriteRenderer.color = GetColorById(ColorId.Color_pink);
                break;
            case ColorId.Color_purple:
                spriteRenderer.color = GetColorById(ColorId.Color_purple);
                break;
            case ColorId.Color_red:
                spriteRenderer.color = GetColorById(ColorId.Color_red);
                break;
            case ColorId.Color_yellow:
                spriteRenderer.color = GetColorById(ColorId.Color_yellow);
                break;
            case ColorId.Color10:
            default:
                spriteRenderer.color = GetColorById(ColorId.Color10);
                break;
        }
    }
    #endregion
    
}