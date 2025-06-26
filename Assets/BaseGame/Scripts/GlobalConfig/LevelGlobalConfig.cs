using UnityEngine;
using Sirenix.Utilities;
using System.Collections.Generic;
using UnityEditor;

[CreateAssetMenu(fileName = "LevelGlobalConfig", menuName = "GlobalConfigs/LevelGlobalConfig")]
[GlobalConfig("Assets/Resources/GlobalConfig/")]
public class LevelGlobalConfig : GlobalConfig<LevelGlobalConfig>
{
    public List<LevelConfigData> LevelConfigs = new();

    public LevelConfigData GetLevelConfigData(int level)
    {
        if (level < 0 || level > LevelConfigs.Count)
        {
            return LevelConfigs[^1];
        }
        for(int i = 0; i < LevelConfigs.Count; i++)
        {
            if(LevelConfigs[i].level == level)
            {
                return LevelConfigs[i];
            }
        }
        return null;
    }
    public Level GetLevelPrefab(int level)
    {
        LevelConfigData config = GetLevelConfigData(level);
        if (config != null)
        {
            return config.levelPrefab;
        }
        return null;
    }
}
[System.Serializable]
public class LevelConfigData
{
    public int level;
    public Level levelPrefab;
    
}