using System.Collections.Generic;
using UnityEngine;
using Sirenix.Utilities;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "SpritesGlobalConfig", menuName = "GlobalConfigs/SpritesGlobalConfig")]
[GlobalConfig("Assets/Resources/GlobalConfig/")]
public class SpritesGlobalConfig : GlobalConfig<SpritesGlobalConfig>
{
    public List<ScrewSpriteData> ScrewSprites = new();
    public List<BoxSpriteData> BoxSprites = new();
    public List<ScrewEffectSpriteData> ScrewEffectSprites = new();
    
    public Sprite GetScrewSprite(ColorId colorId)
    {
        foreach (var screwSprite in ScrewSprites)
        {
            if (screwSprite.ColorId == colorId)
            {
                return screwSprite.ScrewSprite;
            }
        }
        return null;
    }
    public Sprite GetBoxMainSprite(ColorId colorId)
    {
        foreach (var boxSprite in BoxSprites)
        {
            if (boxSprite.ColorId == colorId)
            {
                return boxSprite.BoxMainSprite;
            }
        }
        return null;
    }
    public Sprite GetBoxCoverSprite(ColorId colorId)
    {
        foreach (var boxSprite in BoxSprites)
        {
            if (boxSprite.ColorId == colorId)
            {
                return boxSprite.BoxCoverSprite;
            }
        }
        return null;
    }
    public Sprite GetScrewEffectSprite(ColorId colorId)
    {
        foreach (var screwEffectSprite in ScrewEffectSprites)
        {
            if (screwEffectSprite.ColorId == colorId)
            {
                return screwEffectSprite.ScrewEffectSprite;
            }
        }
        return null;
    }
}
[System.Serializable]
public class ScrewSpriteData
{
    public ColorId ColorId;
    public Sprite ScrewSprite;
}
[System.Serializable]
public class BoxSpriteData
{
    public ColorId ColorId;
    public Sprite BoxMainSprite;
    public Sprite BoxCoverSprite;
}
[System.Serializable]
public class ScrewEffectSpriteData
{
    public ColorId ColorId;
    public Sprite ScrewEffectSprite;
}
