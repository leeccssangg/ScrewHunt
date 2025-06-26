using System;
using Core.Manager;
using Cysharp.Threading.Tasks;
using TW.UGUI.MVPPattern;
using UnityEngine;
using R3;
using Sirenix.OdinInspector;
using TW.Reactive.CustomComponent;
using TW.UGUI.Core.Activities;
using TW.UGUI.Core.Screens;
using TW.UGUI.Core.Views;

[Serializable]
public class ActivityMatchMakingContext 
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
            return UniTask.CompletedTask;
        }
    }
    
    [HideLabel]
    [Serializable]
    public class UIView : IAView
    {
        [field: Title(nameof(UIView))]
        [field: SerializeField] public CanvasGroup MainView {get; private set;}  
        
        public UniTask Initialize(Memory<object> args)
        {
            return UniTask.CompletedTask;
        }
    }

    [HideLabel]
    [Serializable]
    public class UIPresenter : IAPresenter, IActivityLifecycleEventSimple
    {
        [field: SerializeField] public UIModel Model {get; private set;} = new();
        [field: SerializeField] public UIView View { get; set; } = new();
        [field: SerializeField] public bool IsGamePlay { get; private set; }
        [field: SerializeField] public bool IsActiveScreenInGame { get; private set; }


        public async UniTask Initialize(Memory<object> args)
        {
            await Model.Initialize(args);
            await View.Initialize(args);

            IsGamePlay = (bool)args.Span[0];
            IsActiveScreenInGame = (bool)args.Span[1];
            //if(GamePlay.Instance) GamePlay.Instance.State = GamePlayState.None;
        }
        public void DidEnter(Memory<object> args)
        {
            DelayHide().Forget();
        }
        public void DidExit(Memory<object> args)
        {

        }
        private async UniTask DelayHide()
        {
            //LevelManager.Instance.ClearCurLevel();
            await UniTask.Delay(1500, cancellationToken: View.MainView.GetCancellationTokenOnDestroy());
            ActivityContainer.Find(ContainerKey.Activities).HideAsync(nameof(ActivityMatchMaking)).Forget();
            if (IsGamePlay)
            {
                if(!IsActiveScreenInGame)
                    await ScreenContainer.Find(ContainerKey.Screens).PopAsync(true);
                //GamePlay.Instance.StartPlay();
                LevelManager.Instance.ClearCurLevel();
                LevelManager.Instance.SetCurLevel(InGameDataManager.Instance.InGameData.LevelSaveData.Level.Value);
                //ScreenInGameContext.Events.LoadLevel?.Invoke();
            }
            else
            {
                ViewOptions view = new ViewOptions(nameof(ScreenMainMenu));
                await ScreenContainer.Find(ContainerKey.Screens).PushAsync(view, true);
            }

        }
    }
}