using System;
using Cysharp.Threading.Tasks;
using TW.UGUI.Core.Modals;
using UnityEngine;

public class ModalWin : Modal
{
    [field: SerializeField] public ModalWinContext.UIPresenter UIPresenter {get; private set;}
    
    protected override void Awake()
    {
        base.Awake();
        // The lifecycle event of the view will be added with priority 0.
        // Presenters should be processed after the view so set the priority to 1.
        AddLifecycleEvent(UIPresenter, 1);
    }
    
    public override async UniTask Initialize(Memory<object> args)
    {
        await base.Initialize(args);
    }

    public override async UniTask WillPushEnter(Memory<object> args)
    {
        await base.WillPushEnter(args);
    }

    public override async UniTask WillPopEnter(Memory<object> args)
    {
        await base.WillPopEnter(args);
    }
}
