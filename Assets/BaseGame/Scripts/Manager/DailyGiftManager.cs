using System;
using UnityEngine;
using System.Collections.Generic;
using TW.Utility.DesignPattern;
using MemoryPack;
using Sirenix.OdinInspector;
using TW.Reactive.CustomComponent;

public class DailyGiftManager : Singleton<DailyGiftManager>
{
    [SerializeField] public DailyGiftData m_DailyGiftData;
    [SerializeField] private ReactiveValue<int> m_CurrentDayID;
    [SerializeField] private DateTime m_NextClaimDay;
    [SerializeField] private DateTime m_NextDay;

    private void Start()
    {
        LoadData();
    }
    public void LoadData()
    {
        // m_DailyGiftData = InGameDataManager.Instance.InGameData.DailyGiftData;
        // ImportDataString(m_DailyGiftData.Id, m_DailyGiftData.Date);
        // UpdateNextDay();
    }
    public void ResetDataNewDay()
    {
        LoadData();
        Save();
    }
    public void Save()
    {
        //DailyGiftData data = new DailyGiftData
        //{
        //    Id = m_CurrentDayID,
        //    Date = TimeUtil.DateTimeToString(m_NextClaimDay),
        //};
        m_DailyGiftData.Id = m_CurrentDayID;
        m_DailyGiftData.Date = m_NextClaimDay;
        InGameDataManager.Instance.SaveData();
    }
    public void ImportDataString(ReactiveValue<int> id, DateTime data)
    {
        m_CurrentDayID = id;
        m_NextClaimDay = data;
    }
    public string ToDataString()
    {
        string s = "";
        s += "" + m_CurrentDayID + "|" + m_NextClaimDay;
        return s;
    }
    public bool IsGoodToClaim()
    {
        TimeSpan ts = DateTime.Now - m_NextClaimDay;
        return ts.TotalSeconds >= 0;
    }
    public List<GameResource> GetListDailyGift()
    {
        //List<GameResource> gift = DailyGiftGlobalConfig.Instance.GetListDailyGiftByDayNum(m_CurrentDayID % 7);
        //return gift;
        return new();
    }
    public void OnClaimComplete()
    {
        UpdateNextDay();
        m_NextClaimDay = m_NextDay;
        m_CurrentDayID.Value++;
        //EventManager.TriggerEvent("ClaimDailyGift");
        //UICMainMenu.Events.NotiDailyGift?.Invoke();
        NotiDailyGift();
        Save();
        //UICMainMenu.Events.NotiDailyGift?.Invoke();
    }
    //public void ClaimBonusGift()
    //{
    //    ItemReward gift = m_DailyGiftConfigs.fiveDayGift;
    //    ClaimGift(gift);
    //}
    //private void ClaimGift(List<GameResource> gift)
    //{
    //    List<GameResource> rewards = new List<GameResource>(gift);
    //    UIManager.Instance.OpenUI<UIReward>().Setup(rewards,OnClaimComplete);
    //    Debug.Log("ClaimGift");
    //}
    private void UpdateNextDay()
    {
        m_NextDay = TimeUtil.GetNextDate();
        //m_NextDay = DateTime.Now;
    }
    public string GetTimeToNextDay()
    {
        DateTime now = DateTime.Now;
        TimeSpan timeSpan = m_NextDay - now;
        string time = TimeUtil.GetTimeStringFromSecond(timeSpan.TotalSeconds);
        return time;
    }

    public int GetCurrentDayId()
    {
        return m_CurrentDayID;
    }
    public bool IsGoodToCollectBonusGift()
    {
        return m_CurrentDayID % 7 == 5;
    }
    public List<DailyReward> GetListDailyGifts()
    {
        //return DailyGiftGlobalConfig.Instance.GetListDailyGifts();
        return new();
    }
    public void NotiDailyGift()
    {
        ScreenMainMenuContext.Events.NotiDailyGift?.Invoke(IsGoodToClaim());
    }
}
[System.Serializable]
public class DailyReward
{
    public int dayID;
    public List<GameResource> gifts;
}
[System.Serializable]
[MemoryPackable]
public partial class DailyGiftData
{
    [field: SerializeField] public ReactiveValue<int> Id { get; set; } = new(0);
    [field: SerializeField] public DateTime Date { get; set; } = DateTime.Now;

    [MemoryPackConstructor]
    public DailyGiftData()
    {
        //Id = new(0);
        //Date = DateTime.Now;
    }
}
// public partial class InGameData
// {
//     [MemoryPackOrder(4)]
//     [field: SerializeField, PropertyOrder(4)] public DailyGiftData DailyGiftData { get; set; } = new();
// }
