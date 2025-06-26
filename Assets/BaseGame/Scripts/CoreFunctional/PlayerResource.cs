using System.Collections.Generic;
using TW.Utility.CustomType;

[System.Serializable]
public static class PlayerResource
{
    public static GameResource Get(GameResource.Type resourceType)
    {
        return InGameDataManager.Instance.InGameData.PlayerResourceData.GetGameResource(resourceType);
    }
    public static bool IsEnough(GameResource.Type resourceType, BigNumber value)
    {
        return InGameDataManager.Instance.InGameData.PlayerResourceData.IsEnoughGameResource(resourceType, value);
    }
    public static bool IsEnough(GameResource gameResource)
    {
        return InGameDataManager.Instance.InGameData.PlayerResourceData.IsEnoughGameResource(gameResource);
    }
    public static void Add(GameResource.Type resourceType, BigNumber value)
    {
        InGameDataManager.Instance.InGameData.PlayerResourceData.AddGameResource(resourceType, value);
        InGameDataManager.Instance.SaveData();
    }
    public static void Add(GameResource gameResource)
    {
        InGameDataManager.Instance.InGameData.PlayerResourceData.AddGameResource(gameResource);
        InGameDataManager.Instance.SaveData();
    }
    public static void Consume(GameResource.Type resourceType, BigNumber value)
    {
        InGameDataManager.Instance.InGameData.PlayerResourceData.ConsumeGameResource(resourceType, value);
        InGameDataManager.Instance.SaveData();
    }
    public static void Consume(GameResource gameResource)
    {
        InGameDataManager.Instance.InGameData.PlayerResourceData.ConsumeGameResource(gameResource);
        InGameDataManager.Instance.SaveData();
    }
    public static void ClaimListReward(List<GameResource> gameResources, float rewardMultiplier)
    {
        foreach (var gameResource in gameResources)
        {
            GameResource resource = new(gameResource.ResourceType,gameResource.Amount*rewardMultiplier);
            InGameDataManager.Instance.InGameData.PlayerResourceData.AddGameResource(resource);
        }
        InGameDataManager.Instance.SaveData();
    }
    // public static void AddListenerResourceChange(GameResource.Type resourceType, Action action)
    // {
    //     if (action == null)
    //     {
    //         Debug.LogError("Action is null");
    //         return;
    //     }
    //     Get(resourceType).ValueChange.Subscribe(_ => action.Invoke());
    //     
    // }
    // public static void AddListenerResourceChangeValue(GameResource.Type resourceType, Action<BigNumber> action)
    // {
    //     if (action == null)
    //     {
    //         Debug.LogError("Action is null");
    //         return;
    //     }
    //     if (!GameResourceChangeValueCallbackDict.TryAdd(resourceType, action))
    //     {
    //         GameResourceChangeValueCallbackDict[resourceType] += action;
    //     }
    // }
    // public static void AddListenerResourceChangeToValue(GameResource.Type resourceType, Action<BigNumber> action)
    // {
    //     if (action == null)
    //     {
    //         Debug.LogError("Action is null");
    //         return;
    //     }
    //     if (!GameResourceChangeToValueCallbackDict.TryAdd(resourceType, action))
    //     {
    //         GameResourceChangeToValueCallbackDict[resourceType] += action;
    //     }
    // }
    // public static void RemoveListenerResourceChange(GameResource.Type resourceType, Action action)
    // {
    //     if (GameResourceChangeCallbackDict.ContainsKey(resourceType))
    //     {
    //         GameResourceChangeCallbackDict[resourceType] -= action;
    //     }
    //     Get(resourceType).ValueChange.
    // }
    // public static void RemoveListenerResourceChangeValue(GameResource.Type resourceType, Action<BigNumber> action)
    // {
    //     if (GameResourceChangeValueCallbackDict.ContainsKey(resourceType))
    //     {
    //         GameResourceChangeValueCallbackDict[resourceType] -= action;
    //     }
    // }
    // public static void RemoveListenerResourceChangeToValue(GameResource.Type resourceType, Action<BigNumber> action)
    // {
    //     if (GameResourceChangeToValueCallbackDict.ContainsKey(resourceType))
    //     {
    //         GameResourceChangeToValueCallbackDict[resourceType] -= action;
    //     }
    // }
}