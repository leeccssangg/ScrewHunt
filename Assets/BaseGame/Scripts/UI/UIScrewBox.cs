using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TW.Utility.CustomComponent;
using UnityEngine;
using UnityEngine.UI;

public class UIScrewBox : ACachedMonoBehaviour
{
    [field: SerializeField] public ScrewBox ScrewBox { get; private set; }
    [field: SerializeField] public List<UIScrewBoxSlot> ScrewSlots { get; private set; } = new List<UIScrewBoxSlot>();
    [field: SerializeField] public Image ImgScrewBoxMain { get; private set; }
    [field: SerializeField] public Image ImgScrewBoxCover { get; private set; }
    [field: SerializeField] public FeelAnimation AnimCloseBox { get; private set; }
    [field: SerializeField] public FeelAnimation AnimOpenBox { get; private set; }
    
    public void Init(ScrewBox screwBox)
    {
        this.ScrewBox = screwBox;
        for (int i = 0; i < ScrewSlots.Count; i++)
        {
            if(i < ScrewBox.MaxCapacity)
            {
                ScrewSlots[i].Init(i,true, ScrewBox.ColorId);
            }
            else
            {
                ScrewSlots[i].Init(i,false, ColorId.None); // Disable unused slots
            }
        }

        ImgScrewBoxMain.sprite = SpritesGlobalConfig.Instance.GetBoxMainSprite(ScrewBox.ColorId);
        ImgScrewBoxCover.sprite = SpritesGlobalConfig.Instance.GetBoxCoverSprite(ScrewBox.ColorId);
        //ImgScrewBoxCover.gameObject.SetActive(false);
        //ColorUtil.SetColorToImage(this.ImgScrewBox, ScrewBox.ColorId);
        AnimOpenBox.Play();
    }
    public bool IsAbleToAddScrew(ColorId colorId)
    {
        if (this.ScrewBox.ColorId != colorId)
        {
            return false; // Cannot add screw of different color
        }

        for (var i = 0; i < ScrewSlots.Count; i++)
        {
            if (!ScrewSlots[i].IsHaveScrew)
                return true;
        }
        return false;
    }
    public Vector3 GetPositionForScrew()
    {
        for(int i = 0;i< ScrewSlots.Count; i++)
        {
            if (!ScrewSlots[i].IsHaveScrew)
            {
                ScrewSlots[i].SetSlotStatus(true);
                return ScrewSlots[i].Transform.position;
            }
        }
        return Vector3.zero;
    }

    public async Task AddScrewToSlot()
    {
        ScrewSlots[this.ScrewBox.CurrentScrewCount].SetSlotFull(true);
        AudioManager.Instance.PlaySoundFx(AudioType.SfxDropBlock);
        this.ScrewBox.IncreaseScrewCount(1);
        if (this.ScrewBox.CurrentScrewCount >= this.ScrewBox.MaxCapacity)
        {
            await UniTask.WaitForSeconds(0.25f);
            AudioManager.Instance.PlaySoundFx(AudioType.SfxBoxDone);
            //ImgScrewBoxCover.gameObject.SetActive(true);
            AnimCloseBox.Play();
        }
        //this.ScrewBox.IncreaseScrewCount(1);
    }

    public void OnDespawn()
    {
        this.ScrewBox = null;
    }
}
