using Lofelt.NiceVibrations;
using R3;
using TW.Utility.DesignPattern;
using UnityEngine;


public class VibrationManager : Singleton<VibrationManager>
{
    [field: SerializeField] public bool IsActive {get; set;}

    private void Start()
    {
        InGameDataManager.Instance.InGameData.SettingData.VibrateActive.ReactiveProperty.Subscribe(SetVibration).AddTo(this);
        SetVibration(InGameDataManager.Instance.InGameData.SettingData.VibrateActive);
    }

    public void SetVibration(bool value)
    {
        IsActive = value;
    }
    public void CallHaptic()
    {
        if (!IsActive) return;
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.Success);
    }
}