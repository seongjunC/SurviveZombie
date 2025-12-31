using System;
using System.Collections.Generic;
using pattern.Singleton;
using UnityEngine;

namespace Manager
{
    public class SoundManager : Singleton<SoundManager>
    {
        [Header("사운드 파일")]
        public List<SoundData> soundDataList;
        public List<VoiceData> voiceDataList;
        
        [Header("오디오 소스")]
        [SerializeField] private AudioSource bgmSource;
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioSource voiceSources;
        
        private Dictionary<SoundType, AudioClip> soundDic = new Dictionary<SoundType, AudioClip>();
        private Dictionary<(int, VoiceType), AudioClip> voiceDic = new Dictionary<(int,VoiceType), AudioClip>();

        protected override void Awake()
        {
            base.Awake();
            foreach (var soundData in soundDataList)
            {
                soundDic.TryAdd(soundData.type, soundData.clip);
            }
            foreach (var voiceData in voiceDataList)
            {
                voiceDic.TryAdd((voiceData.voiceId, voiceData.voiceType), voiceData.clip);
            }
            
            AudioSetting(0.5f, 0.5f, 0.5f);
        }
        
        public void PlayBGM(SoundType type)
        {
            if (!soundDic.TryGetValue(type, out var clip)) return;
            
            if (bgmSource.clip == clip) return;
            
            StopBGM();
            bgmSource.clip = clip;
            bgmSource.loop = true;
            bgmSource.Play();
        }

        public void StopBGM()
        {
            bgmSource.Stop();
        }
        
        public void PlaySFX(SoundType type)
        {
            if (!soundDic.TryGetValue(type, out var clip)) return;
            sfxSource.PlayOneShot(clip);
        }

        public void PlaySFXAtPoint(SoundType type, Vector3 position, float volume = 1f)
        {
            if (!soundDic.TryGetValue(type, out var clip)) return;
            AudioSource.PlayClipAtPoint(clip, position,volume);
        }

        public void AudioSetting(float BGMVolume, float SFXVolume, float voiceVolume )
        {
            bgmSource.volume = PlayerPrefs.GetFloat("BGMVolume", 1f);
            sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1f);
            voiceSources.volume = PlayerPrefs.GetFloat("VoiceVolume", 1f);
        }
        
        public void PlayVoice(int voiceId, VoiceType voiceType)
        {
            if (!voiceDic.TryGetValue((voiceId, voiceType), out var clip)) return;
            if (voiceSources.isPlaying) voiceSources.Stop();
            voiceSources.PlayOneShot(clip);
        }

        public void StopVoice()
        {
            voiceSources.Stop();
        }
        
    }
    public enum SoundType
    {
        BGM_Title,
        BGM_Main,
        SFX_PlayerShoot,   
        SFX_PlayerReload,
        SFX_ZombieReact,
        SFX_ZombieDead
    }

    public enum VoiceType
    {
        Korean,
        English
    }

    [Serializable]
    public class VoiceData
    {
        public int voiceId;
        public VoiceType voiceType;
        public AudioClip clip;
    }

    [Serializable]
    public class SoundData
    {
        public SoundType type;
        public AudioClip clip;
    }
}