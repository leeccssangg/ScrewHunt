using System;
using Core.Manager;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using TW.UGUI.MVPPattern;
using TW.UGUI.Core.Screens;
using TW.UGUI.Core.Views;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ScreenLoadingContext 
{
    [HideLabel]
    [Serializable]
    public class UIView : IAView
    {
        [field: Title(nameof(UIView))]
        [field: SerializeField] public CanvasGroup MainView {get; private set;}
        [field: SerializeField] public Slider LoadingBar {get; private set;}
        public UniTask Initialize(Memory<object> args)
        {
            return UniTask.CompletedTask;
        }
    }

    [HideLabel]
    [Serializable]
    public class UIPresenter : IAPresenter , IScreenLifecycleEventSimple
    {
        [field: SerializeField] public UIView View { get; set; } = new();
        
        public async UniTask Initialize(Memory<object> args)
        {
            await View.Initialize(args);
        }

        public void DidPushEnter(Memory<object> args)
        {
            StartLoading();
        }
        public void StartLoading()
        {
            View.LoadingBar.value = 0;
            View.LoadingBar.DOValue(1, 4).OnComplete(async () => {await OnLoadingComplete(); });
        }
        public async UniTask OnLoadingComplete()
        {
            ViewOptions options = new ViewOptions(nameof(ScreenInGame));
            await ScreenContainer.Find(ContainerKey.Screens).PushAsync(options);
            LevelManager.Instance.SetCurLevel(InGameDataManager.Instance.InGameData.LevelSaveData.Level.Value);
            //GamePlay.Instance.StartPlay();
            //GameManager.Instance.HandleChestReward();
        }
    }
}