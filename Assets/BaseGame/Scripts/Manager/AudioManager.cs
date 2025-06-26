using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using TW.Utility.DesignPattern;
using TW.Utility.Extension;
using UnityEngine;
using UnityEngine.Events;


public enum AudioType
{
    BgMainMenu = 0,
    BgGamePlay = 1,
    
    SfxUIClickBtn = 31,
    SfxUIWinGame = 32,
    SfxUILoseGame = 33,
    SfxUICoinCollect = 34,
    SfxUIKeyCollect = 35,
    SfxUnlockSkin = 36,
    SfxOpenGift= 37,
    
    SfxPickupBox,
    SfxPickupCup,
    SfxBoxDone,
    
    SfxPickupBlock,
    SfxDropBlock,
    SfxCrashBlock,
}
public class AudioManager : Singleton<AudioManager>
{
    [field: SerializeField] public AudioConfig[] SoundFxArray {get; private set;}
    [field: SerializeField] public AudioConfig[] MusicArray {get; private set;}

    [field: SerializeField] public AudioSource SoundFxAudioSource { get; private set;}
    [field: SerializeField] public AudioSource MusicAudioSource { get; private set;}
    [field: SerializeField] public AudioSource IndependenceAudioSource { get; private set;}
    public UnityAction<bool> SoundFxChangeCallback {get; set;}
    private DOGetter<float> DoGetMusicAudioSourceVolume { get; set; }
    private DOSetter<float> DoSetMusicAudioSourceVolume { get; set; }
    private TweenCallback MusicScaleVolumeToAZeroCallback { get; set; }
    private AudioClip CurrentMusicAudioClip { get; set; }
    private List<Tween> ScaleVolumeTween { get; set; } = new List<Tween>();
    private float DelayCallSoundEffect { get; set; }
    private List<GameObject> IndependenceCallerList { get; set; } = new List<GameObject>();

    protected override void Awake()
    {
        base.Awake();
        DoGetMusicAudioSourceVolume = GetMusicAudioSourceVolume;
        DoSetMusicAudioSourceVolume = SetMusicAudioSourceVolume;
        MusicScaleVolumeToAZeroCallback = () =>
        {
            MusicAudioSource.clip = CurrentMusicAudioClip;
            MusicAudioSource.Play();
        };
    }

    private void Start()
    {
        SetSoundFx(InGameDataManager.Instance.InGameData.SettingData.SoundActive);
        SetMusic(InGameDataManager.Instance.InGameData.SettingData.MusicActive);
    }

    private AudioConfig GetFxAudioConfig(AudioType audioType)
    {
        AudioConfig audioConfig = null; 
        for (int index = 0; index < SoundFxArray.Length; index++)
        {
            AudioConfig config = SoundFxArray[index];
            if (config.AudioType != audioType) continue;
            audioConfig = config;
            break;
        }

        return audioConfig;
    }
    private AudioConfig GetMusicAudioConfig(AudioType audioType)
    {
        AudioConfig audioConfig = null; 
        for (int index = 0; index < MusicArray.Length; index++)
        {
            AudioConfig config = MusicArray[index];
            if (config.AudioType != audioType) continue;
            audioConfig = config;
            break;
        }
        return audioConfig;
    }
    public void PlaySoundFx(AudioType audioType, float delay = 0)
    {
        AudioConfig audioConfig = GetFxAudioConfig(audioType);
        if (audioConfig == null) return;
        PlaySoundDelay(audioConfig.AudioClip.GetRandomElement(), delay);
    }

    public void PlaySoundFx(AudioClip audioClip, float delay = 0)
    {
        if (audioClip == null) return;
        PlaySoundDelay(audioClip, delay);
    }

    public void PlaySoundFxIndex(AudioType audioType, int index, float delay = 0)
    {
        AudioConfig audioConfig = GetFxAudioConfig(audioType);
        if (audioConfig == null) return;
        PlaySoundDelay(audioConfig.AudioClip[index], delay);
    }
    public void PlaySoundFxDuration(AudioType audioType, out float duration)
    {
        duration = 0;
        AudioConfig audioConfig = GetFxAudioConfig(audioType);
        if (audioConfig == null) return;
        DelayCallSoundEffect = duration;
        AudioClip audioClip = audioConfig.AudioClip.GetRandomElement();
        duration = audioClip.length / IndependenceAudioSource.pitch;
        IndependenceAudioSource.PlayOneShot(audioClip);
    }
    public void PlaySoundFxDuration(AudioType audioType, float duration)
    {
        if (duration < 0)
        {
            return;
        }

        AudioConfig audioConfig = GetFxAudioConfig(audioType);
        if (audioConfig == null) return;
        AudioClip audioClip = audioConfig.AudioClip.GetRandomElement();
        float audioClipDuration = audioClip.length / SoundFxAudioSource.pitch;
        SoundFxAudioSource.PlayOneShot(audioClip);
        DOVirtual.DelayedCall(duration, 
            () => PlaySoundFxDuration(audioClip, duration - audioClipDuration));
    }
    public void PlaySoundFxDuration(AudioClip audioClip, float duration)
    {
        if (duration < 0)
        {
            return;
        }
        SoundFxAudioSource.PlayOneShot(audioClip);
        float audioClipDuration = audioClip.length / SoundFxAudioSource.pitch;
        DOVirtual.DelayedCall(duration, 
            () => PlaySoundFxDuration(audioClip, duration - audioClipDuration));
    }
    public void SetActiveIndependenceAudioSource(AudioType audioType, GameObject caller, bool value)
    {
        AudioConfig audioConfig = GetFxAudioConfig(audioType);
        if (audioConfig == null) return;
        if (value)
        {
            if(IndependenceCallerList.Contains(caller)) return;
            IndependenceCallerList.Add(caller);
            if (IndependenceCallerList.Count == 1)
            {   
                IndependenceAudioSource.clip = audioConfig.AudioClip.GetRandomElement();
                IndependenceAudioSource.Play();
            }
        }
        else
        {
            if(!IndependenceCallerList.Contains(caller)) return;
            IndependenceCallerList.Remove(caller);
            if (IndependenceCallerList.Count == 0)
            {
                IndependenceAudioSource.Stop();
            }
        }



    }
    public void PlayMusic(AudioType audioType)
    {
        AudioConfig audioConfig = GetMusicAudioConfig(audioType);
        if (audioConfig == null) return;
        AudioClip newMusicAudioClip = audioConfig.AudioClip.GetRandomElement();
        if (CurrentMusicAudioClip == newMusicAudioClip) return;
        CurrentMusicAudioClip = newMusicAudioClip;
        MusicAudioSource.clip = CurrentMusicAudioClip;
        MusicAudioSource.Play();
    }
    public void ChangeMusic(AudioType audioType, float time)
    {
        for (int i = 0; i < ScaleVolumeTween.Count; i++)
        {
            ScaleVolumeTween[i].Kill();
        }
        ScaleVolumeTween.Clear();
        AudioConfig audioConfig = GetMusicAudioConfig(audioType);
        if (audioConfig == null) return;
        AudioClip newMusicAudioClip = audioConfig.AudioClip.GetRandomElement();
        if (CurrentMusicAudioClip == newMusicAudioClip) return;
        CurrentMusicAudioClip = newMusicAudioClip;
        DOGetter<float> getMusicAudioSourceVolumeDelegate = GetMusicAudioSourceVolume;
        ScaleVolumeTween.Add(DOTween.To(DoGetMusicAudioSourceVolume, DoSetMusicAudioSourceVolume, 0, time / 2f)
            .OnComplete(MusicScaleVolumeToAZeroCallback));
        ScaleVolumeTween.Add(DOTween.To(DoGetMusicAudioSourceVolume, DoSetMusicAudioSourceVolume, 1, time / 2f)
            .SetDelay(time /2 ));

    }   
    
    private void SetMusicAudioSourceVolume(float x)
    {
        MusicAudioSource.volume = x;
    }
    private float GetMusicAudioSourceVolume()
    {
        return MusicAudioSource.volume;
    }

    private async void PlaySoundDelay(AudioClip audioClip, float delay = 0)
    {
        if (delay < 0.01f)
        {
            SoundFxAudioSource.PlayOneShot(audioClip);
            return;
        }
        await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: this.GetCancellationTokenOnDestroy());
        SoundFxAudioSource.PlayOneShot(audioClip);
    }
    public void SetSoundFx(bool value)
    {
        SoundFxAudioSource.mute = !value;
        IndependenceAudioSource.mute = !value;
        SoundFxChangeCallback?.Invoke(value);
    }
    public void SetMusic(bool value)
    {
        MusicAudioSource.mute = !value;
    }
}

[System.Serializable]
public class AudioConfig
{
    [field: SerializeField] public AudioType AudioType {get; private set;}
    [field: SerializeField] public AudioClip[] AudioClip {get; private set;}
}
public interface ISoundFx
{
    void PlaySoundFx();
}
[Serializable]
public class PlayerSoundClip : ISoundFx
{
    [field:SerializeField] private AudioClip Clip {get; set;}
    public void PlaySoundFx()
    {
        AudioManager.Instance.PlaySoundFx(Clip);
    }
}