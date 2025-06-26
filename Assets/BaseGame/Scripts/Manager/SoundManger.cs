using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManger : MonoBehaviour
    {
        
    }

    public interface ISound
    {
        public void Play();
    }
    public class PlaySoundByClip : ISound
    {
        public PlaySoundByClip(AudioClip clip)
        {
            Clip = clip;
        }
        [field:SerializeField] public AudioClip Clip { get; set; }
        public void Play()
        {
            AudioManager.Instance.PlaySoundFx(Clip);
        }
    }
    [Serializable]
    public class PlaySoundRandom : ISound
    {   
        public PlaySoundRandom(List<AudioClip> clips)
        {
            Clips = clips;
        }
        [field:SerializeField] public List<AudioClip> Clips { get; set; }
        public void Play()
        {
            if(Clips.Count == 0) return;
            AudioClip clip = Clips[Random.Range(0, Clips.Count)];
            AudioManager.Instance.PlaySoundFx(clip);
        }
    }
    [Serializable]
    public class PlaySoundLoop : ISound
    {
        public PlaySoundLoop(AudioClip clip, float loopTime)
        {
            Clip = clip;
            LoopTime = loopTime;
        }
        [field:SerializeField] private AudioClip Clip { get; set; }
        [field:SerializeField] private float LoopTime { get; set; }
        public void Play()
        {
            AudioManager.Instance.PlaySoundFxDuration(Clip, LoopTime);
            //GameUtilityManager.SoundManager.PlaySoundAudio(Clip,(int)LoopTime);
        }
    }