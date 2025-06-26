using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using R3;
using TMPro;
using TW.UGUI.MVPPattern;
using TW.Utility.CustomComponent;
using TW.Utility.CustomType;
using UnityEngine;
using LitMotion;

public class UIResourceCoin : ACachedMonoBehaviour, IAView
{
    [field: SerializeField] private string TextFormat { get; set; }
    [field: SerializeField] public TextMeshProUGUI TextCoin { get; private set; }
    private BigNumber StartResource { get; set; }
    private BigNumber TargetResource { get; set; }
    private BigNumber CurrentResource { get; set; }
    private MotionHandle RemapMotion { get; set; }
    private MotionHandle EffectMotion { get; set; }
    private bool IsDelayIncrease { get; set; }
    private int DelayIncreaseValue { get; set; }
    private Vector3 DelayIncreaseValuePos { get; set; }
    [field: SerializeField] public Transform MainView { get; private set; }
    [field: SerializeField] public RectTransform ResourceImage { get; private set; }
    [field: SerializeField] public UIResourceEffect UIResourceEffect { get; private set; }
    public UniTask Initialize(Memory<object> args)
    {
        TargetResource = PlayerResource.Get(GameResource.Type.Coin).Amount;
        CurrentResource = TargetResource;
        StartResource = TargetResource;
        PlayerResource.Get(GameResource.Type.Coin).ReactiveProperty.Subscribe(OnGemChange).AddTo(this);
        return UniTask.CompletedTask;
    }

    private void OnGemChange(BigNumber gem)
    {
        if (IsDelayIncrease)
        {
            IsDelayIncrease = false;
            List<GameObject> list = new List<GameObject>();
            for (int i = 0; i < DelayIncreaseValue; i++)
            {
                UIResourceEffect ui = Instantiate(UIResourceEffect, DelayIncreaseValuePos, Quaternion.identity, MainView);
                ui.Setup(DelayIncreaseValuePos, ResourceImage.position);
            }

            EffectMotion.TryCancel();
            EffectMotion = LMotion.Create(0, 1, 1).WithOnComplete(() =>
            {
                OnGemChangeCompleted(gem);
            })
            .RunWithoutBinding();
        }
        else
        {
            OnGemChangeCompleted(gem);
        }
    }
    private void OnGemChangeCompleted(BigNumber gem)
    {
        RemapMotion.TryCancel();
        TargetResource = gem;
        StartResource = CurrentResource;
        RemapMotion = LMotion.Create(0f, 1f, 0.5f)
            .WithEase(Ease.Linear)
            .Bind(OnUpdateResource);
    }

    private void OnUpdateResource(float process)
    {
        CurrentResource = ReMap(StartResource, TargetResource, process);
        TextCoin.SetText($"<style=s>{CurrentResource.RoundToInt().ToStringUI()}");
    }

    private BigNumber ReMap(BigNumber a, BigNumber b, float t)
    {
        return a + t * (b - a);
    }
    public void SetDelayIncreaseValue(Vector3 pos, int value)
    {
        IsDelayIncrease = true;
        DelayIncreaseValuePos = pos;
        DelayIncreaseValue = value;
    }
}
