using System.Collections;
using System.Collections.Generic;
using TW.Utility.CustomComponent;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIScrewBoxSlot : ACachedMonoBehaviour
{
    [field: SerializeField] public int SlotId { get; private set; }
    [field: SerializeField] public Image ImgSlotNotFull { get; private set; }
    [field: SerializeField] public Image ImgSlotFull { get; private set; }
    [field: SerializeField] public bool IsHaveScrew { get; private set; }

    public void Init(int slotId, bool isActive, ColorId colorId)
    {
        SlotId = slotId;
        if (isActive)
        {
            ImgSlotNotFull.gameObject.SetActive(true);
            ImgSlotFull.gameObject.SetActive(false);
            ImgSlotFull.sprite = SpritesGlobalConfig.Instance.GetScrewSprite(colorId);
            //ColorUtil.SetColorToImage(this.ImgSlotFull, colorId);
        }
        else
        {
            ImgSlotNotFull.gameObject.SetActive(false);
            ImgSlotFull.gameObject.SetActive(false);
        }
        IsHaveScrew = false;
    }
    public void SetSlotFull(bool isFull)
    {
        ImgSlotNotFull.gameObject.SetActive(!isFull);
        ImgSlotFull.gameObject.SetActive(isFull);
    }
    public void SetSlotStatus(bool isHaveScrew)
    {
        this.IsHaveScrew = isHaveScrew;
    }
}
