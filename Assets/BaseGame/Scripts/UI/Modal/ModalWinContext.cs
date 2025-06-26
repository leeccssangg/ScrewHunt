using System;
using Cysharp.Threading.Tasks;
using TW.UGUI.MVPPattern;
using UnityEngine;
using R3;
//using SDK;
using Sirenix.OdinInspector;
using TW.Reactive.CustomComponent;
using TW.UGUI.Core.Modals;
using UnityEngine.UI;
using System.Collections.Generic;
using Core.Manager;
using TMPro;
using TW.UGUI.Core.Views;
using TW.UGUI.Core.Activities;
//using SDK;
//using UGUI.Core.Modals;

[Serializable]
public class ModalWinContext 
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
        [field: SerializeField] public List<GameResource> Reward { get; private set; } = new();

        public UniTask Initialize(Memory<object> args)
        {
            //Reward = LevelRewardGlobalConfig.Instance.GetListRewardByLevel(InGameDataManager.Instance.InGameData.LevelSaveData.Level).rewards;
            return UniTask.CompletedTask;
        }
    }
    [HideLabel]
    [Serializable]
    public class UIView : IAView
    {
        [field: Title(nameof(UIView))]
        [field: SerializeField] public CanvasGroup MainView { get; private set; }
        [field: SerializeField] public Button BtnContinue { get; private set; }
        [field: SerializeField] public Button BtnClaimX2 { get; private set; }
        [field: SerializeField] public TextMeshProUGUI TxtLevel { get; private set; }
        [field: SerializeField] public UIItemReward UIItemReward { get; private set; }
        [field: SerializeField] public UIResourceCoin UIResourceCoin { get; private set; }
        [field: SerializeField] public Transform TfIconCoin { get; private set; }

        public UniTask Initialize(Memory<object> args)
        {
            //UIResourceCoin.Initialize(args);
            return UniTask.CompletedTask;
        }
    }

    [HideLabel]
    [Serializable]
    public class UIPresenter : IAPresenter, IModalLifecycleEventSimple
    {
        [field: SerializeField] public UIModel Model { get; private set; } = new();
        [field: SerializeField] public UIView View { get; set; } = new();

        public async UniTask Initialize(Memory<object> args)
        {
            await Model.Initialize(args);
            await View.Initialize(args);

            SetupUI();

            View.BtnContinue.SetOnClickDestination(OnClickBtnContinue);
            View.BtnClaimX2.SetOnClickDestination(OnClickBtnClaimX2);
        }
        public void SetupUI()
        {
            //View.UIItemReward.Setup(Model.Reward[0]);
            View.TxtLevel.SetText($"Level {InGameDataManager.Instance.InGameData.LevelSaveData.Level.Value}");
            if (InGameDataManager.Instance.InGameData.LevelSaveData.Level < 3)
            {
                View.BtnClaimX2.gameObject.SetActive(false);
            }
        }
        public void DidPushEnter(Memory<object> args)
        {
            
        }
        public async void OnClickBtnContinue(Unit _)
        {
            //View.UIResourceCoin.SetDelayIncreaseValue(View.TfIconCoin.position, 10);
            //PlayerResource.ClaimListReward(Model.Reward, 1);
            await OnClaimCompleted();
        }
        public async UniTask OnClickBtnClaimX2()
        {
            //AdsManager.Instance.ShowRewardVideo(
            //    "x2Win",
            //    async () => {
            //        View.UIResourceCoin.SetDelayIncreaseValue(View.TfIconCoin.position, 10);
            //        PlayerResource.ClaimListReward(Model.Reward, 2);
            //        await OnClaimCompleted();
            //    },
            //    null,
            //    null
            //    );
            await OnClaimCompleted();


        }
        public async UniTask OnClaimCompleted()
        {
            //AdsManager.Instance.ShowInterstitial();
            View.MainView.interactable = false;
            //await UniTask.Delay(1500, cancellationToken: View.MainView.GetCancellationTokenOnDestroy());
            await ModalContainer.Find(ContainerKey.Modals).PopAsync(true);
            LevelManager.Instance.SaveData();
            LevelManager.Instance.ClearCurLevel();
            //AllQuestManager.Instance.Notify(MissionTarget.COMPLETE_LEVEL, "1");
            ViewOptions options = new ViewOptions(nameof(ActivityMatchMaking));
            await ActivityContainer.Find(ContainerKey.Activities).ShowAsync(options, true, true);
            if (InGameDataManager.Instance.InGameData.LevelSaveData.Level.Value <= LevelGlobalConfig.Instance.LevelConfigs[^1].level)
            {
                InGameDataManager.Instance.InGameData.LevelSaveData.Level.Value++;
                InGameDataManager.Instance.SaveData();
            }
            // if(InGameDataManager.Instance.InGameData.LevelSaveData.Level.Value < 3)
            // {
            //     await ActivityContainer.Find(ContainerKey.Activities).ShowAsync(options, true, true);
            // }
            // else
            // {
            //     await ActivityContainer.Find(ContainerKey.Activities).ShowAsync(options, false, true);
            // }

        }
    }
}