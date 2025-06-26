using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using R3;
using Sirenix.OdinInspector;
using TW.UGUI.MVPPattern;
using TW.Reactive.CustomComponent;
using TW.UGUI.Core.Screens;
using TW.UGUI.Core.Modals;
using UnityEngine.UI;
using Pextension;
using TW.UGUI.Core.Views;
using TMPro;
using TW.UGUI.Core.Activities;

[Serializable]
public class ScreenMainMenuContext 
{
    public static class Events
    {
        // public static Subject<Unit> SampleEvent { get; set; } = new();
        public static Action<bool> NotiDailyQuest { get; set; }
        public static Action<bool> NotiDailyGift { get; set; }
    }
    [HideLabel]
    [Serializable]
    public class UIModel : IAModel
    {
        [field: Title(nameof(UIModel))]
        [field: SerializeField] public ReactiveValue<int> CurrentLevel { get; private set; }
        public UniTask Initialize(Memory<object> args)
        {
            CurrentLevel = InGameDataManager.Instance.InGameData.LevelSaveData.Level;
            return UniTask.CompletedTask;
        }
    }

    [HideLabel]
    [Serializable]
    public class UIView : IAView
    {
        [field: Title(nameof(UIView))]
        [field: SerializeField] public CanvasGroup MainView {get; private set;}
        //[field: SerializeField] public UIResourceCoin UIResourceCoin {get; private set;}
        [field: SerializeField] public Button ButtonSetting {get; private set;}
        [field: SerializeField] public Button ButtonPlay {get; private set;}
        [field: SerializeField] public Button ButtonShop {get; private set;}
        [field: SerializeField] public Button ButtonDailyGift { get; private set;}
        [field: SerializeField] public Button ButtonDailyQuest { get; private set;}
        [field: SerializeField] public GameObject GONotiDailyQuest { get; private set; }
        [field: SerializeField] public GameObject GONotiDailyGift { get; private set; }
        [field: SerializeField] public TextMeshProUGUI TxtLevel { get; private set; }


        public UniTask Initialize(Memory<object> args)
        {
            //UIResourceCoin.Initialize(args);  
            return UniTask.CompletedTask;
        }
        public void OnLevelChange(int level)
        {
            TxtLevel.text = $"<style=H2>Level {level.ToString()}";
        }
        private void CloseAllPanel()
        {
            
        }
    }

    [HideLabel]
    [Serializable]
    public class UIPresenter : IAPresenter, IScreenLifecycleEventSimple
    {
        [field: SerializeField] public UIModel Model {get; private set;}
        [field: SerializeField] public UIView View { get; set; } = new();

        public async UniTask Initialize(Memory<object> args)
        {
            await Model.Initialize(args);
            await View.Initialize(args);
            
            Events.NotiDailyQuest = OnNotiDailyQuest;
            Events.NotiDailyGift = OnNotiDailyGift;

            Model.CurrentLevel.ReactiveProperty.Subscribe(View.OnLevelChange).AddTo(View.MainView);
            View.ButtonSetting.SetOnClickDestination(OnClickButtonSetting);
            View.ButtonPlay.SetOnClickDestination(OnClickButtonPlay);
            //View.ButtonShop.SetOnClickDestination(OnClickButtonShop);
            View.ButtonDailyGift.SetOnClickDestination(OnClickButtonDailyGift);
            View.ButtonDailyQuest.SetOnClickDestination(OnClickButtonDailyQuest);
            //View.GONotiDailyQuest.SetActive(AllQuestManager.Instance.IsShowNotiDailyQuest());
        }
        public UniTask Cleanup(Memory<object> args)
        {
            Events.NotiDailyQuest = null;
            Events.NotiDailyGift = null;
            return UniTask.CompletedTask;
        }

        public void DidPushEnter(Memory<object> args)
        {
            // AllQuestManager.Instance.NotiDailyQuest();
            // DailyGiftManager.Instance.NotiDailyGift();
        }

        public void DidPopEnter(Memory<object> args)
        {
            // AllQuestManager.Instance.NotiDailyQuest();
            // DailyGiftManager.Instance.NotiDailyGift();
        }

        public void OnClickButtonSetting(Unit unit)
        {
            //ViewOptions options = new ViewOptions(nameof(ModalSettings));
            //ModalContainer.Find(ContainerKey.Modals).PushAsync(options);
        } 
        public void OnClickButtonPlay(Unit unit)
        {
            //InputManager.Instance.SetInputActive(true);
            //InputManager.Instance.HandleInput(unit);
            //ViewOptions options = new ViewOptions(nameof(ScreenInGame));
            //ScreenContainer.Find(ContainerKey.Screens).PopAsync(true);
            //GamePlay.Instance.StartPlay();
            ViewOptions options = new ViewOptions(nameof(ActivityMatchMaking));
            ActivityContainer.Find(ContainerKey.Activities).ShowAsync(options, true,false);
            //ScreenContainer.Find(ContainerKey.Screens).PopAsync(true);
        }
        public void OnClickButtonShop(Unit unit)
        {
            //ViewOptions options = new ViewOptions(nameof(ScreenShop));
            //ScreenContainer.Find(ContainerKey.Screens).PushAsync(options);
        }
        public void OnClickButtonDailyGift(Unit unit)
        {
            //ViewOptions options = new ViewOptions(nameof(ModalDailyGift));
            //ModalContainer.Find(ContainerKey.Modals).PushAsync(options);
        }
        public void OnClickButtonDailyQuest(Unit unit)
        {
            //ViewOptions options = new ViewOptions(nameof(ModalDailyQuest));
            //ModalContainer.Find(ContainerKey.Modals).PushAsync(options);
        }
        public void OnNotiDailyQuest(bool isActive)
        {
            //View.GONotiDailyQuest.SetActive(isActive);
        }
        public void OnNotiDailyGift(bool isActive)
        {
            //View.GONotiDailyGift.SetActive(isActive);
        }
    }
}