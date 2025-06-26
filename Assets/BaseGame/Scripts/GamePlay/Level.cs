using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Manager;
using Sirenix.OdinInspector;
using TW.Utility.CustomComponent;
using UnityEngine;

public class Level : ACachedMonoBehaviour
{
    [field: SerializeField] public const int MaxCapaPerScrewBox = 3;
    [field: SerializeField] public List<SelectableObject> SelectableObjects { get; private set; } = new List<SelectableObject>();
    [field: SerializeField] public List<ScrewBox> ScrewBoxes { get; private set; } = new List<ScrewBox>();
    [field: SerializeField] public int TotalScrews {get; private set;}
    [field: SerializeField] public int CurScrewsToBox;
    public void Init()
    {
        TotalScrews = 0;
        for (var i = 0; i < SelectableObjects.Count; i++)
        {
            SelectableObjects[i].Init(i);
        }
        CalculateScrewBoxs();
        CurScrewsToBox = 0;
        CameraController.Instance.ResetLevelCamera();
    }
    public SelectableObject GetSelectableObjectByScrew(Screw screw)
    {
        foreach (var selectableObject in SelectableObjects)
        {
            if (selectableObject.Id == screw.SelectableObject.Id)
            {
                return selectableObject;
            }
        }
        return null; 
    }
    private void CalculateScrewBoxs()
    {
        ScrewBoxes.Clear();
        int colorCount = Enum.GetNames(typeof(ColorId)).Length -1;
        for (int i = 0; i < colorCount; i++)
        {
            ColorId colorId = (ColorId)i;
            CalculateScrewBoxesByColorId(colorId);
        }
    }
    private void CalculateScrewBoxesByColorId(ColorId colorId)
    {
        int numScrews = 0;
        numScrews = GetNumScrewsOfColorId(colorId);
        if (numScrews <= 0)
        {
            return;
        }
        int numBoxesMaxCapacity = numScrews / MaxCapaPerScrewBox;
        int numScrewsLeft = numScrews % MaxCapaPerScrewBox;
        for(int i = 0; i < numBoxesMaxCapacity; i++)
        {
            ScrewBox screwBox = new ScrewBox();
            screwBox.Init(colorId,MaxCapaPerScrewBox);
            ScrewBoxes.Add(screwBox);
        }
        if (numScrewsLeft > 0)
        {
            ScrewBox screwBox = new ScrewBox();
            screwBox.Init(colorId,numScrewsLeft);
            ScrewBoxes.Add(screwBox);
        }
        TotalScrews += numScrews;
    }
    private int GetNumScrewsOfColorId(ColorId colorId)
    {
        int count = 0;
        for (var i = 0; i < SelectableObjects.Count; i++)
        {
            count += SelectableObjects[i].GetNumScrewsColor(colorId);
        }
        return count;
    }

    public void IncreaseScrewsToBox(int amount)
    {
        CurScrewsToBox += amount;
        if (CurScrewsToBox >= TotalScrews)
        {
            CurScrewsToBox = TotalScrews; // Clamp to total screws
            LevelManager.Instance.Win();
        }
    }
    
    #if UNITY_EDITOR
    [Button]
    private void GetAllEditorSetupObjects()
    {
        SelectableObjects.Clear();
        SelectableObjects = this.GetComponentsInChildren<SelectableObject>().ToList();
        for (var i = 0; i < SelectableObjects.Count; i++)
        {
            SelectableObjects[i].SetId(i);
            SelectableObjects[i].GetAllScrews();
        }
    }
    [Button]
    private void InitLevel()
    {
        Init();
    }
   
    #endif
}
