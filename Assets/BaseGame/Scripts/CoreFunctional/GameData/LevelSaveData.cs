using System.Collections.Generic;
using MemoryPack;
using Sirenix.OdinInspector;
using TW.Utility.CustomType;
using UnityEngine;
using R3;
using TW.Reactive.CustomComponent;

[System.Serializable]
[MemoryPackable]

public partial class LevelSaveData
{
    public static LevelSaveData Instance => InGameDataManager.Instance.InGameData.LevelSaveData;
    [field: SerializeField] public ReactiveValue<int> Level { get; private set; }
    //[field: SerializeField] public List<BoosterType> BoostersUnlocked { get; private set; } = new();
    [MemoryPackConstructor] 
    public LevelSaveData()
    {
        Level = new(1);
    }
}
public partial class InGameData
{
    [MemoryPackOrder(0)]
    [field: SerializeField, PropertyOrder(0)] public LevelSaveData LevelSaveData { get; set; } = new();
}