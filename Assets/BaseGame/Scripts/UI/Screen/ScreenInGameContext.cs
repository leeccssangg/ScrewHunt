using System;
using Cysharp.Threading.Tasks;
using TW.UGUI.MVPPattern;
using UnityEngine;
using R3;
using Sirenix.OdinInspector;
using TMPro;
using TW.Reactive.CustomComponent;
using TW.UGUI.Core.Screens;
using UnityEngine.UI;
using TW.UGUI.Core.Views;
using TW.UGUI.Core.Modals;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Manager;
using Unity.VisualScripting.FullSerializer;
using Pextension;
using TW.UGUI.Core.Activities;
using Object = UnityEngine.Object;

[Serializable]
public class ScreenInGameContext 
{
    public static class Events
    {
        public static Subject<Unit> SampleEvent { get; set; } = new();
        public static Action LoadLevel { get; set; }
        public static Action OnSelectSector { get; set; }
        public static Action<bool> SetBlockingUI { get; set; }
        public static Action<Vector3, ColorId> SpawnScrew { get; set; }
        public static Action<UIScewEffect, UIScrewBox> DespawnScrew { get; set; }
        public static Action SetupUIScrewBox { get; set; }
    }
    
    [HideLabel]
    [Serializable]
    public class UIModel : IAModel
    {
        [field: Title(nameof(UIModel))]
        [field: SerializeField] public ReactiveValue<int> CurLevel { get; private set; }
        [field: SerializeField] public ReactiveValue<bool> IsWinning { get; private set; }
        public UniTask Initialize(Memory<object> args)
        {
            //CurrentMapProcess = LevelManager.Instance.CurrentMapProcess;
            CurLevel = InGameDataManager.Instance.InGameData.LevelSaveData.Level;
            //IsWinning = LevelManager.Instance.IsWinning;
            return UniTask.CompletedTask;
        }
    }

    [HideLabel]
    [Serializable]
    public class UIView : IAView
    {
        [field: Title(nameof(UIView))]
        [field: SerializeField] public CanvasGroup MainView {get; private set;}
        [field: SerializeField] public TextMeshProUGUI TxtLevel { get; private set; }
        [field: SerializeField] public Button BtnSetting { get; private set; }
        [field: SerializeField] public Transform TfLevelContainer { get; private set; }
        [field: SerializeField] public Button BtnReplay { get; private set; }
        [field: SerializeField] public GameObject UIHandTut { get; private set; }
        [field: SerializeField] public FeelAnimation[] TutAnimation { get; private set; }
        [field: SerializeField] public TextMeshProUGUI[] TxtTut { get; private set; }
        [field: SerializeField] public Transform UIHandTUTInitPos { get; private set; }
        [field: SerializeField] public Transform PosCollectScrews { get; private set; }
        [field: SerializeField] public UIScewEffect UIScrewEffectPrefab { get; private set; }
        [field: SerializeField] public Transform TfParentUIScrewEffect { get; private set; }
        [field: SerializeField] public List<UIScrewBox> ScrewBoxes { get; private set; } = new();
        
        [field: SerializeField] public UIScrewBox UIScrewBoxPrefab { get; private set; }
        [field: SerializeField] public Transform TfUIScrewBoxContainer { get; private set; }
        //[field: SerializeField] public GameObject GOUIMaxLevel { get; private set; }
        
        //[field: SerializeField] public UIBooster BtnBoosterSortCup { get; private set; } 
        //[field: SerializeField] public UIBooster BtnBoosterShuffleBox { get; private set; }
        //[field: SerializeField] public UIBooster BtnBoosterSortBox { get; private set; }
        //[field: SerializeField] public UIBooster BtnBoosterSort3Box { get; private set; }


        public UniTask Initialize(Memory<object> args)
        {
            return UniTask.CompletedTask;
        }
    }

    [HideLabel]
    [Serializable]
    public class UIPresenter : IAPresenter, IScreenLifecycleEventSimple
    {
        [field: SerializeField] public UIModel Model { get; private set; }
        [field: SerializeField] public UIView View { get; set; } = new();
        [field: SerializeField] public bool IsBlockingUI { get; private set; } = false;
        private MiniPool<UIScewEffect> _poolUIScrewEffect = new();
        private MiniPool<UIScrewBox> _poolUIScrewBox = new();
        
        [field: SerializeField] public UIScrewBox CurrentUIScrewBox { get; private set; }

        public async UniTask Initialize(Memory<object> args)
        {
            await Model.Initialize(args);
            await View.Initialize(args);

            _poolUIScrewEffect.OnInit(View.UIScrewEffectPrefab,20, View.TfParentUIScrewEffect);
            _poolUIScrewBox.OnInit(View.UIScrewBoxPrefab, 20, View.TfUIScrewBoxContainer);

            Model.CurLevel.ReactiveProperty.Subscribe(SetupUI).AddTo(View.MainView);

            View.BtnSetting.SetOnClickDestination(OnClickBtnSettings);
            View.BtnReplay.SetOnClickDestination(_ => OnClickBtnReplay(_));

            Events.OnSelectSector = OnSelectSector;
            Events.SetBlockingUI = SetBlockingUI;
            Events.SpawnScrew = SpawnScew;
            Events.DespawnScrew = DespawnScrew;
            Events.SetupUIScrewBox = SetupUIScrewBox;
            Events.LoadLevel = ResetUI;

            //LoadLevel();
            //View.BtnBoosterSortCup.Init(OnClickBtnAddBoosterSortCup);
            //View.BtnBoosterShuffleBox.Init(OnClickBtnAddBoosterShuffleBox);
            //View.BtnBoosterSortBox.Init(OnClickBtnAddBoosterSortBox);
            //View.BtnBoosterSort3Box.Init(OnClickBtnAddBoosterSort3Box);

            //GamePlay.Instance.NewBoosterUnlocked.ReactiveProperty.Subscribe(OnUnlockBooster).AddTo(View.MainView);
        }

        private void SetBlockingUI(bool value)
        {
            IsBlockingUI = value;
        }
        private void SetupUI(int level)
        {
            View.TxtLevel.SetText($"LEVEL {level}");
            SetUpUIHandTut();
            IsBlockingUI = false;
            // View.GOUIMaxLevel.SetActive(false);
            if (Model.CurLevel.Value > LevelGlobalConfig.Instance.LevelConfigs[^1].level)
            {
                View.TxtLevel.SetText($"LEVEL {LevelGlobalConfig.Instance.LevelConfigs[^1].level}");
            }
            _poolUIScrewBox.Collect();
            _poolUIScrewEffect.Collect();
            //_poolUIScrewEffect.Release();
            //SetupUIScrewBox();
        }
        private void ResetUI()
        {
            foreach (var VARIABLE in View.ScrewBoxes)
            {
                VARIABLE.OnDespawn();
            }
            _poolUIScrewBox.Collect();
            _poolUIScrewEffect.Collect();
            _poolUIScrewEffect.Release();
        }
        private void SetupUIScrewBox()
        {
            View.ScrewBoxes.Clear();
            _poolUIScrewBox.Collect();
            for (var i = 0; i < LevelManager.Instance.CurrentLevelMap.ScrewBoxes.Count; i++)
            {
                ScrewBox screwBox = LevelManager.Instance.CurrentLevelMap.ScrewBoxes[i];
                UIScrewBox uiScrewBox = _poolUIScrewBox.Spawn();
                uiScrewBox.Init(screwBox);
                View.ScrewBoxes.Add(uiScrewBox);
            }
            _poolUIScrewEffect.Collect();
        }
        public UniTask Cleanup(Memory<object> args)
        {
            Events.LoadLevel = null;
            Events.OnSelectSector = null;
            Events.SetBlockingUI = null;
            Events.SpawnScrew = null;
            Events.DespawnScrew = null;
            Events.SetupUIScrewBox = null;
            return UniTask.CompletedTask;
        }
        public void DidPushEnter(Memory<object> args)
        {
            //LoadLevel();
            //LevelManager.Instance.SetCurLevel(Model.CurLevel.Value);
        }
        private void OnClickBtnSettings(Unit _)
        {
            if(IsBlockingUI) return;
            ViewOptions options = new(nameof(ModalSettings));
            ModalContainer.Find(ContainerKey.Modals).PushAsync(options);
        }
        private async UniTask OnClickBtnReplay(Unit _)
        {
            //LevelManager.Instance.ClearCurLevel();
            //LevelManager.Instance.SetCurLevel(Model.CurLevel.Value);
            //GamePlay.Instance.StartPlay();
            if(IsBlockingUI) return;
            ViewOptions options = new ViewOptions(nameof(ActivityMatchMaking));
            await ActivityContainer.Find(ContainerKey.Activities).ShowAsync(options, true, true);
        }
        private void SetUpUIHandTut()
        {
            // if(Model.CurLevel == 1)
            // {
            //     View.UIHandTut.SetActive(true);
            //     View.TxtTut[0].gameObject.SetActive(true);
            //     View.TutAnimation[0].gameObject.SetActive(true);
            //     View.TutAnimation[0].Play();
            //     View.TxtTut[1].gameObject.SetActive(false);
            //     View.TutAnimation[1].gameObject.SetActive(false);
            // }
            // else if(Model.CurLevel == 2)
            // {
            //     View.UIHandTut.SetActive(true);
            //     View.TxtTut[1].gameObject.SetActive(true);
            //     View.TutAnimation[1].gameObject.SetActive(true);
            //     View.TutAnimation[1].Play();
            //     View.TxtTut[0].gameObject.SetActive(false);
            //     View.TutAnimation[0].gameObject.SetActive(false);
            // }
            // else
            // {
            //     View.UIHandTut.SetActive(false);
            //     foreach (var textMeshProUGUI in View.TxtTut)
            //     {
            //         textMeshProUGUI.gameObject.SetActive(false);
            //     }
            // }
        }
        private void OnSelectSector()
        {
            // if(Model.CurLevel == 1)
            // {
            //     //View.TutAnimation.Stop();
            //     View.UIHandTut.SetActive(false);
            //     View.TxtTut[0].gameObject.SetActive(false);
            //     View.TutAnimation[0].gameObject.SetActive(false);
            // }
            // else if(Model.CurLevel == 2)
            // {
            //     View.UIHandTut.SetActive(false);
            //     View.TxtTut[1].gameObject.SetActive(false);
            //     View.TutAnimation[1].gameObject.SetActive(false);
            // }
            // View.UIHandTut.transform.position = View.UIHandTUTInitPos.position;
        }
        private void SpawnScew(Vector3 position, ColorId colorId)
        {
            // Instantiate the prefab at the specified position and set its parent
            //UIScewEffect uiScewEffect = Object.Instantiate(View.UIScrewEffectPrefab, position, Quaternion.identity, View.MainView.transform);
            UIScewEffect uiScewEffect = _poolUIScrewEffect.Spawn(position, Quaternion.identity);
            Debug.Log("SpawnScrew at position: " + position);
            // Optionally, perform additional setup on the instantiated object
            Vector3 endPos = GetCurrentScrewBoxPosition(colorId, out UIScrewBox uiScrewBox);
            uiScewEffect.Setup(position, endPos, uiScrewBox);
        }

        private Vector3 GetCurrentScrewBoxPosition(ColorId colorId, out UIScrewBox uiScrewBox)
        {
            for (var i = 0; i < View.ScrewBoxes.Count; i++)
            {
                if (View.ScrewBoxes[i].IsAbleToAddScrew(colorId))
                {
                    uiScrewBox = View.ScrewBoxes[i];
                    //uiScrewBox.ScrewBox.IncreaseScrewCount(1);
                    return uiScrewBox.GetPositionForScrew();
                }
            }
            uiScrewBox = null;
            return Vector3.zero;
        }
        private void DespawnScrew(UIScewEffect uiScrewEffect, UIScrewBox uiScrewBox)
        {
            if (uiScrewEffect != null)
            {
                uiScrewEffect.DeactiveTrail();
                _poolUIScrewEffect.Despawn(uiScrewEffect);
                uiScrewBox.AddScrewToSlot();
                //Object.Destroy(uiScrewEffect.gameObject);
            }
        }
    }
}