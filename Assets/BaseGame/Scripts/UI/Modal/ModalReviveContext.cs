using System;
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
public class ModalReviveContext 
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
        [field: SerializeField] public bool IsUnlockAllSlot { get; private set; }
        [field: SerializeField] public GameResource CostRevive { get; private set; }
        //[field: SerializeField] public ResourcePrice BoosterRevive { get; private set; }

        public UniTask Initialize(Memory<object> args)
        {
            //IsUnlockAllSlot = (bool)args.Span[0];
            //if (!IsUnlockAllSlot)
            //{
            //    CostRevive = LevelRewardGlobalConfig.Instance.GetReviveByLevel(InGameDataManager.Instance.InGameData.LevelSaveData.Level).cost;
            //}
            //else
            //{
            //    BoosterRevive = BoosterPriceGlobalConfig.Instance.GetBoosterPrice(GameResource.Type.Booster_SortCup);
            //    CostRevive = BoosterRevive.cost;
            //}

            return UniTask.CompletedTask;
        }
    }
    
    [HideLabel]
    [Serializable]
    public class UIView : IAView
    {
        [field: Title(nameof(UIView))]
        [field: SerializeField] public CanvasGroup MainView {get; private set;}
        [field: SerializeField] public Image ImgIcon { get; private set; }
        [field: SerializeField] public TextMeshProUGUI TxtPrice { get; private set; }
        [field: SerializeField] public TextMeshProUGUI TxtDes { get; private set; }
        [field: SerializeField] public Button BtnBack { get; private set; }
        [field: SerializeField] public Button BtnReviveCoin { get; private set; }
        [field: SerializeField] public Button BtnReviveAds { get; private set; }
        [field: SerializeField] public UIResourceCoin UIResourceCoin { get; private set; }

        public UniTask Initialize(Memory<object> args)
        {
            UIResourceCoin.Initialize(args);
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

            PlayerResource.Get(GameResource.Type.Coin).ReactiveProperty.Subscribe(OnCoinChange).AddTo(View.MainView);

            View.BtnBack.SetOnClickDestination(OnClickBtnBack);
            View.BtnReviveCoin.SetOnClickDestination(OnClickBtnBuyCoin);
            View.BtnReviveAds.SetOnClickDestination(OnClickBtnBuyAds);
        }
        public void SetupUI()
        {
            //if (Model.IsUnlockAllSlot)
            //{
            //    View.TxtDes.SetText($"<style=l>{Model.BoosterRevive.description}");
            //    View.TxtPrice.SetText($"<style=h3><sprite index=0> {Model.CostRevive.Amount.ToStringUI()}");
            //    View.ImgIcon.sprite = SpriteGlobalConfig.Instance.GetSpriteByResourceType(Model.BoosterRevive.reward.ResourceType);
            //}
            //else
            //{
            //    View.TxtDes.SetText($"<style=l>UNLOCK MORE BOX SLOT & SORT THE CUP");
            //    View.TxtPrice.SetText($"<style=h3><sprite index=0> {Model.CostRevive.Amount.ToStringUI()}");
            //}
            ////View.ImgIcon
            //View.BtnReviveCoin.interactable = PlayerResource.IsEnough(Model.CostRevive);
        }
        private void OnCoinChange(BigNumber value)
        {
            View.BtnReviveCoin.interactable = PlayerResource.IsEnough(Model.CostRevive);
        }
        private async UniTask OnClickBtnBack()
        {
            await ModalContainer.Find(ContainerKey.Modals).PopAsync(true);
            ViewOptions options = new ViewOptions(nameof(ModalLose));
            await ModalContainer.Find(ContainerKey.Modals).PushAsync(options);
            Debug.Log("CloseRevive");
        }
        private void OnClickBtnBuyCoin(Unit _)
        {
            PlayerResource.Consume(Model.CostRevive);
            OnRevive();
        }
        private void OnClickBtnBuyAds(Unit _)
        {
        //    AdsManager.Instance.ShowRewardVideo(
        //        "ReviveAds",
        //        OnRevive,
        //        null,
        //        null
        //        );
        }
        private void OnRevive()
        {
            ModalContainer.Find(ContainerKey.Modals).PopAsync(true);
            //GamePlay.Instance.OnResumeGame();
        }
    }
}