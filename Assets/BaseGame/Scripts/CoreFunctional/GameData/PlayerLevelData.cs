using MemoryPack;
using TW.Reactive.CustomComponent;
using TW.Utility.CustomType;
using UnityEngine;

[System.Serializable]
[MemoryPackable]
public partial class PlayerLevelData
{
    //public static PlayerLevelData Instance => InGameDataManager.Instance.InGameData.PlayerLevelData;
    [field: SerializeField] public ReactiveValue<int> CurrentLevel { get; set; } = new(1);
    [field: SerializeField] public ReactiveValue<BigNumber> CurrentExp { get; set; } = new(0);

    public void AddExp()
    {
        //BigNumber maxExp = LevelProcessGlobalConfig.Instance.GetLevelConfig(CurrentLevel.Value).MaxExp;
        //BigNumber expPerGame = LevelProcessGlobalConfig.Instance.GetLevelConfig(CurrentLevel.Value).ExpPerGame;
        //CurrentExp.Value += expPerGame;
        //if (CurrentLevel.Value >= LevelProcessGlobalConfig.Instance.LevelConfigs[^1].Level
        //    && CurrentExp.Value >= maxExp)
        //{
        //    CurrentExp.Value = maxExp;
        //    InGameDataManager.Instance.SaveData();
        //    return;
        //}
        //if (CurrentExp.Value >= maxExp)
        //{
        //    if (CurrentLevel.Value < LevelProcessGlobalConfig.Instance.LevelConfigs[^1].Level)
        //    {
        //        CurrentExp.Value = 0;
        //        CurrentLevel.Value++;
        //    }
        //}
        InGameDataManager.Instance.SaveData();
    }
}

//public partial class InGameData
//{
//    [MemoryPackOrder(0)]
//    [field: SerializeField,PropertyOrder(0)] public PlayerLevelData PlayerLevelData { get; set; } = new();
//}
