using System;
using MemoryPack;
using R3;
using Sirenix.OdinInspector;
using TW.Reactive.CustomComponent;
using TW.Utility.CustomType;
using UnityEngine;

[MemoryPackable]
[Serializable]
public partial class GameResource
{
    public enum Type
    {
        Coin = 0,
        Gem = 1,
        Booster_SortCup = 2,
        Booster_ShuffleBox = 3,
        Booster_SortBox = 4,
        Booster_Sort3Box = 5,

    }
    [MemoryPackIgnore]
    [field: HideLabel, HorizontalGroup("GameResource", 100)]
    [field: SerializeField] public Type ResourceType { get; private set; }
    
    [MemoryPackIgnore]
    [field: SerializeField, HideLabel, HorizontalGroup("GameResource")]
    public ReactiveValue<BigNumber> ReactiveAmount { get; private set; } = new();
    
    [MemoryPackIgnore]
    public BigNumber Amount
    {
        get => ReactiveAmount.Value;
        set => ReactiveAmount.Value = value;
    }
    [MemoryPackIgnore]
    public ReactiveProperty<BigNumber> ReactiveProperty => ReactiveAmount.ReactiveProperty;
    
    
    public GameResourceData GameResourceData { get; set; } = new();

    [MemoryPackConstructor]
    public GameResource()
    {
        
    }
    
    [MemoryPackOnSerializing]
    private void OnSerializing()
    {
        GameResourceData.ResourceType = ResourceType;
        GameResourceData.V = Amount.coefficient;
        GameResourceData.M = Amount.exponent;
    }
    
    [MemoryPackOnDeserialized]
    private void OnDeserialized()
    {
        ResourceType = GameResourceData.ResourceType;
        Amount = new BigNumber(GameResourceData.V, GameResourceData.M);
    }
    
    public GameResource(Type resourceType, BigNumber amount)
    {
        ResourceType = resourceType;
        Amount = amount;
    }
    
    public GameResourceData ToGameResourceData()
    {
        return new GameResourceData
        {
            ResourceType = ResourceType,
            V = Amount.coefficient,
            M = Amount.exponent
        };
    }
    public GameResource FromGameResourceData(GameResourceData gameResourceData)
    {
        ResourceType = gameResourceData.ResourceType;
        Amount = new BigNumber(gameResourceData.V, gameResourceData.M);
        return this;
    }

    public void Add(BigNumber value)
    {
        Amount += value;
    }
    public void Consume(BigNumber value)
    {
        Amount -= value;
    }
    public bool IsEnough(BigNumber value, float threshold = 0.001f)
    {
        return value <= Amount + threshold;
    }

}

[MemoryPackable]
[Serializable]
public partial class GameResourceData
{
    [field: SerializeField] public GameResource.Type ResourceType { get; set; }
    [field: SerializeField] public double V { get; set; }
    [field: SerializeField] public int M { get; set; }
}