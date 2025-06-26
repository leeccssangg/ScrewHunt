using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TW.Utility.CustomComponent;
using UnityEngine;
using UnityEngine.UI;

public class Screw : ACachedMonoBehaviour
{
    [field: SerializeField] public SelectableObject SelectableObject { get; private set; }
    [OnValueChanged("UpdateScrewColor")]
    [field: SerializeField] public ColorId ColorId { get; private set; }
    [field: SerializeField] public SpriteRenderer ImgScrewIcon { get; private set; }
    
    public void Init(SelectableObject selectableObject)
    {
        this.SelectableObject = selectableObject;
        //ColorUtil.SetColorToSpriteRenderer(ImgScrewIcon,ColorId);
        ImgScrewIcon.sprite = SpritesGlobalConfig.Instance.GetScrewSprite(this.ColorId);
    }
    public void OnRemoveScrew()
    {
        this.gameObject.SetActive(false);
    }
    public void UpdateScrewColor()
    {
        // Update the screw's color based on ColorId
        // This could involve changing the material, sprite, etc.
        // For example:
        // GetComponent<Renderer>().material.color = ColorManager.GetColor(ColorId);
        //this.ImgScrewIcon.sprite = SpritesGlobalConfig.Instance.ScrewSprite;
        //ColorUtil.SetColorToSpriteRenderer(this.ImgScrewIcon, this.ColorId);
        ImgScrewIcon.sprite = SpritesGlobalConfig.Instance.GetScrewSprite(this.ColorId);
    }
#if UNITY_EDITOR
    public void SetupEditor(SelectableObject selectableObject)
    {
        Init(selectableObject);
    }
#endif
}
