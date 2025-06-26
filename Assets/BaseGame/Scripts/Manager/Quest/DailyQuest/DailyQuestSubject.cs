using Newtonsoft.Json;
//using R3;
using System.Collections.Generic;
using TW.Reactive.CustomComponent;
using UnityEngine;

[System.Serializable]
public class DailyQuestSubject : Subject<MissionTarget>
{
    [field : SerializeField] public List<DailyQuest> DailyQuests { get; private set; } = new();

    #region Load & Save Data
    public void LoadData(List<ReactiveValue<QuestData>> quests = null)
    {
        if (quests != null)
            LoadOldData(quests);
        else
            LoadNewData();
    }
    private void LoadNewData()
    {
        DailyQuests.Clear();
        Debug.Log("Load New Daily Quest Data");
        for (int i = 0; i < AllQuestManager.Instance.GetNumDailyQuestConfig(); i++)
        {
            QuestConfig questConfig = AllQuestManager.Instance.GetDailyQuestConfig(i);
            DailyQuest dailyQuest = GenerateDailyQuest(questConfig.missionTarget);
            dailyQuest.Init(questConfig.id);
            DailyQuests.Add(dailyQuest);
            AddObserver(dailyQuest);
            //ReactiveValue<QuestData> questData = new(new(DailyQuests[i]));
            //QuestsData.Add(questData);
        }
        //AllQuestManager.Instance.SaveData();
    }
    private void LoadOldData(List<ReactiveValue<QuestData>> quests)
    {
        //Debug.Log("Load Old Daily Quest Data");
        //List<DailyQuest> rawList = JsonConvert.DeserializeObject<List<DailyQuest>>(jsonData);
        //QuestsData = quests;
        for (int i = 0; i < quests.Count; i++)
        {
            QuestData dailyQuestData = quests[i];
            DailyQuest dailyQuest = new();
            dailyQuest.Init(dailyQuestData.Id);
            dailyQuest.icd = dailyQuestData.IsClaimed;
            dailyQuest.cl = dailyQuestData.Collect;
            string rawString = JsonConvert.SerializeObject(dailyQuest);
            DailyQuest newQuest = DeserializeQuest(rawString, dailyQuest.GetMissionTarget());
            newQuest.InitQuestConfig();
            DailyQuests.Add(newQuest);
            AddObserver(newQuest);
        }
    }
    public DailyQuest GenerateDailyQuest(MissionTarget missionTarget)
    {
        DailyQuest newDailyQuest = null;
        switch (missionTarget)
        {
            case MissionTarget.COMPLETE_CUPS:
                {
                    DailyQuest_CompleteCups newQuest = new();
                    newDailyQuest = newQuest;
                }
                break;
            case MissionTarget.COLLECT_CASH:
                {
                    DailyQuest_CollectCash newQuest = new();
                    newDailyQuest = newQuest;
                }
                break;
            case MissionTarget.COMPLETE_LEVEL:
                {
                    DailyQuest_CompleteLevel newQuest = new();
                    newDailyQuest = newQuest;
                }
                break;
            case MissionTarget.USE_REVIVE:
                {
                    DailyQuest_UseRevive newQuest = new();
                    newDailyQuest = newQuest;
                }
                break;
            case MissionTarget.UNLOCK_SLOT:
                {
                    DailyQuest_UnlockSlot newQuest = new();
                    newDailyQuest = newQuest;
                }
                break;
            case MissionTarget.USE_BOOSTERS:
                {
                    DailyQuest_UseBooster newQuest = new();
                    newDailyQuest = newQuest;
                }
                break;
        }
        return newDailyQuest;
    }
    public DailyQuest DeserializeQuest(string json, MissionTarget missionTarget)
    {
        DailyQuest dailyQuest = null;
        switch (missionTarget)
        {
            case MissionTarget.COMPLETE_CUPS:
                {
                    dailyQuest = JsonConvert.DeserializeObject<DailyQuest_CompleteCups>(json);
                }
                break;
            case MissionTarget.COLLECT_CASH:
                {
                    dailyQuest = JsonConvert.DeserializeObject<DailyQuest_CollectCash>(json);
                }
                break;
            case MissionTarget.COMPLETE_LEVEL:
                {
                    dailyQuest = JsonConvert.DeserializeObject<DailyQuest_CompleteLevel>(json);
                }
                break;
            case MissionTarget.USE_REVIVE:
                {
                    dailyQuest = JsonConvert.DeserializeObject<DailyQuest_UseRevive>(json);
                }
                break;
            case MissionTarget.UNLOCK_SLOT:
                {
                    dailyQuest = JsonConvert.DeserializeObject<DailyQuest_UnlockSlot>(json);
                }
                break;
            case MissionTarget.USE_BOOSTERS:
                {
                    dailyQuest = JsonConvert.DeserializeObject<DailyQuest_UseBooster>(json);
                }
                break;
        }
        return dailyQuest;
    }
    #endregion
    #region Daily Quest Function
    public override void Notify(MissionTarget id, string info)
    {
        base.Notify(id, info);
        AllQuestManager.Instance.SaveData();
    }
    public void ClaimQuest(DailyQuest dailyQuest)
    {
        dailyQuest.OnClaim();
    }
    public void ClaimQuest(int questId)
    {
        for (int i = 0; i < DailyQuests.Count; i++)
        {
            if (DailyQuests[i].id == questId)
            {
                DailyQuests[i].OnClaim();
            }
        }
    }
    public List<DailyQuest> GetListDailyQuests()
    {
        return new List<DailyQuest>(DailyQuests);
    }
    public DailyQuest GetQuest(int questId)
    {
        for (int i = 0; i < DailyQuests.Count; i++)
        {
            if (DailyQuests[i].id == questId)
            {
                return DailyQuests[i];
            }
        }
        return null;
    }
    public int GetMaxPoint()
    {
        int maxPoint = 0;
        for (int i = 0; i < DailyQuests.Count; i++)
        {
            maxPoint += DailyQuests[i].GetPoint();
        }
        return maxPoint;
    }
    public int GetCurrentPoint()
    {
        int currentPoint = 0;
        for (int i = 0; i < DailyQuests.Count; i++)
        {
            if (DailyQuests[i].IsClaimed())
                currentPoint += DailyQuests[i].GetPoint();
        }
        return currentPoint;
    }
    public float GetDailyProcess()
    {
        float process = (float)GetCurrentPoint() / (float)GetMaxPoint();
        if (process >= 1) process = 1;
        return process;
    }
    public int GetNumDailyQuest()
    {
        return DailyQuests.Count;
    }
    public bool IsGoodToClaimQuest()
    {
        for(int i = 0;i<DailyQuests.Count;i++)
        {
            if (!DailyQuests[i].IsClaimed() && DailyQuests[i].GetProgress() >= 1)
                return true;
        }
        return false;
    } 
    public void DoRandomQuest()
    {
        int questId = Random.Range(0, DailyQuests.Count);
        if (DailyQuests[questId].IsClaimed())
            return;
        DailyQuests[questId].OnNotify((MissionTarget)DailyQuests[questId].mt,"1");
    }
    #endregion

}
