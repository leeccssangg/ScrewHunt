using System;
using Core.Manager;
using Cysharp.Threading.Tasks;
using TW.UGUI.MVPPattern;
using UnityEngine;
using R3;
//using SDK;
using Sirenix.OdinInspector;
using TW.Reactive.CustomComponent;
using TW.UGUI.Core.Activities;
using TW.UGUI.Core.Modals;
using TW.UGUI.Core.Views;
//using UGUI.Core.Modals;
using UnityEngine.UI;
using DG.Tweening;
using TW.Utility.CustomType;
using TMPro;
//using SDK;
[Serializable]
public class ModalLoseContext
{
    public static class Events
    {
        public static Subject<Unit> SampleEvent { get; set; } = new();
    }

    [HideLabel]
    [Serializable]
    public class UIModel : IAModel
    {
        [field: Title(nameof(UIModel))]
        [field: SerializeField] public ReactiveValue<int> SampleValue { get; private set; }
        public UniTask Initialize(Memory<object> args)
        {
            //CoinReward.Value = GamePlay.instance.GetMoneyEarnedEndGame();
            return UniTask.CompletedTask;
        }
    }

    [HideLabel]
    [Serializable]
    public class UIView : IAView
    {
        [field: Title(nameof(UIView))]
        [field: SerializeField] public CanvasGroup MainView { get; private set; }
        [field: SerializeField] public Button BtnHome { get; private set; }
        [field: SerializeField] public Button BtnRestart { get; private set; }

        public UniTask Initialize(Memory<object> args)
        {
            return UniTask.CompletedTask;
        }
    }

    [HideLabel]
    [Serializable]
    public class UIPresenter : IAPresenter, IModalLifecycleEventSimple
    {
        [field: SerializeField] public UIModel Model { get; private set; } = new();
        [field: SerializeField] public UIView View { get; set; } = new();
        private Tween Tween { get; set; }

        public async UniTask Initialize(Memory<object> args)
        {
            await Model.Initialize(args);
            await View.Initialize(args);

            View.BtnHome.SetOnClickDestination(OnClickBtnHome);
            View.BtnRestart.SetOnClickDestination(OnClickBtnRestart);
        }
        public UniTask Cleanup(Memory<object> args)
        {
            Tween?.Kill();
            return UniTask.CompletedTask;
        }
        private void OnClickBtnHome(Unit _)
        {
            //AdsManager.Instance.ShowInterstitial();
            ViewOptions view = new ViewOptions(nameof(ActivityMatchMaking));
            ActivityContainer.Find(ContainerKey.Activities).ShowAsync(view,false,true);
            ModalContainer.Find(ContainerKey.Modals).PopAsync(true);

        }
        private async UniTask OnClickBtnRestart()
        {
            //AdsManager.Instance.ShowInterstitial();
            LevelManager.Instance.ClearCurLevel();
            await ModalContainer.Find(ContainerKey.Modals).PopAsync(true);
            ViewOptions options = new ViewOptions(nameof(ActivityMatchMaking));
            await ActivityContainer.Find(ContainerKey.Activities).ShowAsync(options, true, true);
            //GamePlay.instance.Revive();
            //GamePlay.Instance.StartPlay();
        }
    }
}