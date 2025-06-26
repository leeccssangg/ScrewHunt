using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using TW.Reactive.CustomComponent;
using TW.UGUI.Core.Activities;
using TW.UGUI.Core.Modals;
using TW.UGUI.Core.Views;
using TW.Utility.DesignPattern;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Core.Manager
{
    public class LevelManager : Singleton<LevelManager>
    {
        [field: SerializeField] public Transform TfContainer { get; private set; }
        [field: SerializeField] public LevelSaveData LevelSaveData { get; private set; } = new();
        [field: SerializeField] public ReactiveValue<int> CurrentLevelId { get; private set; } = new(1);
        [field: SerializeField] public ReactiveValue<bool> IsWinning { get; private set; } = new(false);
        [field: SerializeField] public ReactiveValue<bool> IsLosing { get; private set; } = new(false);
        [field: SerializeField] public Level CurrentLevelMap { get; private set; } = null;
        [field: SerializeField] public LevelConfigData CurrentLevelConfigData { get; private set; } = new();
        [field: SerializeField] public Camera MainCamera { get; private set; }

        #region Unity Functions

        private void Awake()
        {
        }

        private void Start()
        {
            LoadData();
        }

        #endregion

        #region Save Load Functions

        private void LoadData()
        {
            LevelSaveData = InGameDataManager.Instance.InGameData.LevelSaveData;
            CurrentLevelId = LevelSaveData.Level;
        }

        public void SaveData()
        {
            LevelSaveData.Level.Value = CurrentLevelId;
            InGameDataManager.Instance.SaveData();
        }

        #endregion

        #region Manager Functions

        public void SetCurLevel(int levelId)
        {
            IsWinning.Value = false;
            IsLosing.Value = false;
            CurrentLevelConfigData = LevelGlobalConfig.Instance.GetLevelConfigData(levelId);
            CurrentLevelMap =
                Instantiate(CurrentLevelConfigData.levelPrefab, TfContainer.position, Quaternion.identity);
            CurrentLevelMap.Init();
            //InputManager.Instance.Init(CurrentLevelMap.RingCollider);
            if (InGameDataManager.Instance.InGameData.LevelSaveData.Level >
                LevelGlobalConfig.Instance.LevelConfigs[^1].level)
            {
                //InputManager.Instance.SetActive(false);
                ViewOptions viewOptions = new ViewOptions(nameof(ActivityMaxLevel));
                ActivityContainer.Find(ContainerKey.Activities).ShowAsync(viewOptions);
            }
            else
            {
                //InputManager.Instance.SetActive(true);
            }
            ScreenInGameContext.Events.SetupUIScrewBox?.Invoke();
        }

        public void ClearCurLevel()
        {
            if (CurrentLevelMap != null)
            {
                Destroy(CurrentLevelMap.gameObject);
            }
            
            CurrentLevelMap = null;
            CurrentLevelConfigData = null;
        }

        [Button]
        public void Win()
        {
            IsWinning.Value = true;
            //InputManager.Instance.SetActive(false);
            OnWinning().Forget();
        }

        [Button]
        public void Lose()
        {
            IsLosing.Value = true;
            //InputManager.Instance.SetActive(false);
            OnLose().Forget();
        }

        private async UniTask OnWinning()
        {
            await UniTask.Delay(1000, cancellationToken: this.GetCancellationTokenOnDestroy());
            AudioManager.Instance.PlaySoundFx(AudioType.SfxUIWinGame);
            ViewOptions options = new(nameof(ModalWin));
            await ModalContainer.Find(ContainerKey.Modals).PushAsync(options);
        }

        private async UniTask OnLose()
        {
            await UniTask.Delay(500, cancellationToken: this.GetCancellationTokenOnDestroy());
            ViewOptions options = new(nameof(ModalLose));
            await ModalContainer.Find(ContainerKey.Modals).PushAsync(options);
        }

        #endregion
        public void InitLevel()
        {
            if(CurrentLevelMap == null) return;
            IsWinning.Value = false;
            IsLosing.Value = false;
            CurrentLevelMap.Init();
            //InputManager.Instance.Init(CurrentLevelMap.RingCollider);
            //InputManager.Instance.SetActive(true);
        }
    }
}