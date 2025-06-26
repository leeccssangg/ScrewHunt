using System.Collections;
using System.Collections.Generic;
using Core.Manager;
using R3;
using TW.Reactive.CustomComponent;
using TW.Utility.CustomComponent;
using UnityEngine;

[System.Serializable]
public class ScrewBox 
{
    [field: SerializeField] public ColorId ColorId { get; private set; }
    [field: SerializeField] public int MaxCapacity {get; private set;}
    [field: SerializeField] public int CurrentScrewCount { get; private set; } = 0;
    
    public void Init(ColorId colorId, int maxCapa)
    {
        this.ColorId = colorId;
        this.MaxCapacity = maxCapa;
        this.CurrentScrewCount = 0;
    }
    public void IncreaseScrewCount(int amount)
    {
        if (CurrentScrewCount < MaxCapacity)
        {
            CurrentScrewCount += amount;
            LevelManager.Instance.CurrentLevelMap.IncreaseScrewsToBox(amount);
        }
        if (CurrentScrewCount >= MaxCapacity)
        {
            CurrentScrewCount = MaxCapacity; // Clamp to max capacity
        }

    }
    private bool IsMaxCapacityReached()
    {
        if (CurrentScrewCount >= MaxCapacity)
        {
            // Handle max capacity reached logic here
            return true;
        }
        return false;
    }
    public void Reset()
    {
        CurrentScrewCount = 0; // Reset screw count
    }
}

[System.Serializable]
public class ScrewBoxConfig
{
    public ColorId ColorId;
    public int Capacity;
}
