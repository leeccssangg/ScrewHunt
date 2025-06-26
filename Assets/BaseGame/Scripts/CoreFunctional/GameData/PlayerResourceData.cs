using System.Collections.Generic;
using MemoryPack;
using Sirenix.OdinInspector;
using TW.Utility.CustomType;
using UnityEngine;

[System.Serializable]
[MemoryPackable]
public partial class PlayerResourceData
{
    public static PlayerResourceData Instance => InGameDataManager.Instance.InGameData.PlayerResourceData;
    [field: SerializeField] public List<GameResource> GameResourceList { get; set; }

    [MemoryPackConstructor]
    public PlayerResourceData()
    {
        // TODO: Create default player resource value
        GameResourceList = new List<GameResource>
        {
            new GameResource(GameResource.Type.Coin, 0),
            new GameResource(GameResource.Type.Gem, 0),
            new GameResource(GameResource.Type.Booster_ShuffleBox, 0),
            new GameResource(GameResource.Type.Booster_SortBox, 0),
            new GameResource(GameResource.Type.Booster_Sort3Box, 0),
            new GameResource(GameResource.Type.Booster_SortCup, 0),
        };
    }

    public GameResource GetGameResource(GameResource.Type resourceType)
    {
        GameResource resource = null;
        for (int i = 0; i < GameResourceList.Count; i++)
        {
            if (GameResourceList[i].ResourceType == resourceType)
            {
                resource = GameResourceList[i];
                break;
            }
        }
        if (resource == null)
        {
            resource = new GameResource(resourceType, 0);
            GameResourceList.Add(resource);
        }
        return resource;
    }
    public void AddGameResource(GameResource.Type resourceType, BigNumber value)
    {
        GetGameResource(resourceType).Add(value);
    }
    public void AddGameResource(GameResource gameResource)
    {
        AddGameResource(gameResource.ResourceType, gameResource.Amount);
    }
    public void ConsumeGameResource(GameResource.Type resourceType, BigNumber value)
    {
        GetGameResource(resourceType).Consume(value);
    }
    public void ConsumeGameResource(GameResource gameResource)
    {
        ConsumeGameResource(gameResource.ResourceType, gameResource.Amount);
    }
    public bool IsEnoughGameResource(GameResource gameResource)
    {
        return IsEnoughGameResource(gameResource.ResourceType, gameResource.Amount);
    }
    public bool IsEnoughGameResource(GameResource.Type resourceType, BigNumber value)
    {
        return GetGameResource(resourceType).IsEnough(value);
    }
}
public partial class InGameData
{
    [MemoryPackOrder(1)]
    [field: SerializeField, PropertyOrder(1)] public PlayerResourceData PlayerResourceData { get; set; } = new();
}