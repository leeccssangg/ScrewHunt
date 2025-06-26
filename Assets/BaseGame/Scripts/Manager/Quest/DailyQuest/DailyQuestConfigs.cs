using System.Collections.Generic;
using UnityEngine;
using Sirenix.Utilities;

//[GUIColor("@MyExtension.EditorExtension.GetColor(\"DailyQuest\", (int)$value)")]
public enum QuestType
{
    DAILY,
    ROOKIE,
    ACHIEVEMENT,
    MAINQUEST,
    NONE = -1,
}
//[GUIColor("@MyExtension.EditorExtension.GetColor(\"DailyQuest\", (int)$value)")]
public enum QuestCollectType
{
    FREE,
    ADS,
    NONE = -1,
}
//[GUIColor("@MyExtension.EditorExtension.GetColorById((int)$value,50)")]
public enum MissionTarget
{
    NONE,
    COMPLETE_CUPS,
    COLLECT_CASH,
    COMPLETE_LEVEL,
    USE_REVIVE,
    UNLOCK_SLOT,
    USE_BOOSTERS,
    LOGIN,
}

[CreateAssetMenu(fileName = "DailyQuestConfigs", menuName = "GlobalConfigs/DailyQuestConfigs")]
[GlobalConfig("Assets/Resources/GlobalConfig/")]
[System.Serializable]
public class DailyQuestConfigs : GlobalConfig<DailyQuestConfigs>
{
    public List<QuestConfig> dailyQuestConfigs = new List<QuestConfig>();
    public List<QuestStage> questStages = new List<QuestStage>();   

    public int GetNumDailyQuestConfig()
    {
        return dailyQuestConfigs.Count;
    }
    public QuestConfig GetDailyQuestConfig(int id)
    {
        for (int i = 0; i < dailyQuestConfigs.Count; i++)
        {
            QuestConfig config = dailyQuestConfigs[i];
            if (config.id == id)
            {
                return config;
            }
        }
        return null;
    }
    public QuestConfig GetDailyQuestConfig(MissionTarget missionTarget)
    {
        for (int i = 0; i < dailyQuestConfigs.Count; i++)
        {
            QuestConfig config = dailyQuestConfigs[i];
            if (config.missionTarget == missionTarget)
            {
                return config;
            }
        }
        return null;
    }
    public List<QuestConfig> GetDailyQuestConfigs()
    {
        return new List<QuestConfig>(dailyQuestConfigs);
    }
    public int GetLastStageId()
    {
        return questStages.Count;
    }
    public List<QuestStage> GetStageReward()
    {
        return questStages;
    }
}
[System.Serializable]
public class QuestConfig
{
    public int id;
    public QuestCollectType type;
    public MissionTarget missionTarget;
    public int targetAmount;
    public List<GameResource> reward;
    public string description;
    public int point;

    public string GetDescription()
    {
        return description;
    }
}
[System.Serializable]
public class QuestStage
{
    public int requiredPoint;
    public List<GameResource> rewards;
}

