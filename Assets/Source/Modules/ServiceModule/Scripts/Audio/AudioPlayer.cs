using System;
using System.Collections.Generic;
using System.Linq;
using Agava.YandexGames;
using Source.Modules.ServiceModule.Scripts.Player.Settings;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Source.Modules.ServiceModule.Scripts.Audio
{
    public class AudioPlayer : IDisposable
    {
        public bool SoundActive { get; private set; } 
        public bool MusicActive { get; private set; }
        
        private readonly Dictionary<SoundType, AudioClip> _audioClips;
        private readonly AudioSource _audioSourcePrefab;
        
        private readonly List<AudioSource> _poolAudioSource = new();
        private readonly SettingsHandler _settingsHandler;
        private AudioSource _currentMusicSource;

        public AudioPlayer(Dictionary<SoundType, AudioClip> audioClips,AudioSource audioSourcePrefab, SettingsHandler settingsHandler)
        {
            _audioClips = audioClips;
            _audioSourcePrefab = audioSourcePrefab;

            _settingsHandler = settingsHandler;
            ExtractAudioSettings();
            
            RewardedAd.BeforeShow += OnAdShowed;
            InterstitialAd.BeforeShow += OnAdShowed;
        }

        private void ExtractAudioSettings()
        {
            _settingsHandler.GetAudioStatus(out bool musicActive, out bool soundActive);
            
            if (musicActive)
                ActivateMusic();
            else
                DeActivateMusic();

            if (soundActive)
                ActivateSound();
            else
                DeActivateSound();
        }

        
        private void OnAdShowed()
        {
            RewardedAd.AfterShow += OnAdClosed;
            InterstitialAd.BeforeShow += OnAdClosed;
            DeActivateMusic();
            DeActivateSound();
        }

        private void OnAdClosed()
        {
            RewardedAd.AfterShow -= OnAdClosed;
            InterstitialAd.BeforeShow -= OnAdClosed;
            ExtractAudioSettings();
        }

        public void ActivateSound()
        {
            Debug.LogWarning("SOUND ON");

            SoundActive = true;
        }

        public void DeActivateSound()
        {
            Debug.LogWarning("SOUND OFF");

            SoundActive = false;
        }

        public void ActivateMusic()
        {
            Debug.LogWarning("MUSIC ON");
            MusicActive = true;
            if (_currentMusicSource!=null)
            {
                _currentMusicSource.mute = false;
            }
        }

        public void DeActivateMusic()
        {
            Debug.LogWarning("MUSIC OFF");

            MusicActive = false;
            if (_currentMusicSource!=null)
            {
                _currentMusicSource.mute = true;
            }
        }

        public void PlaySound(SoundType soundType,float volume = 1f)
        {
            if(SoundActive == false) return;
            
            PlayFreeAudioSource(soundType, false, volume);
        }

        public void PlayAsMusic(SoundType soundType,float volume = 1f)
        {
            if (_currentMusicSource != null)
            {
                _currentMusicSource.Stop();
            }

            PlayFreeAudioSource(soundType, true, volume);
            
            if (_currentMusicSource != null)
            {
                _currentMusicSource.mute = !MusicActive;
            }
        }

        private void PlayFreeAudioSource(SoundType soundType, bool isMusic = false, float volume = 1f)
        {
            if (_audioClips.TryGetValue(soundType, out AudioClip clip))
            {
                AudioSource freeAudioSource = _poolAudioSource.FirstOrDefault(x => x.isPlaying == false);
                if (freeAudioSource == null)
                {
                    freeAudioSource = Object.Instantiate(_audioSourcePrefab);
                    Object.DontDestroyOnLoad(freeAudioSource.gameObject);
                    _poolAudioSource.Add(freeAudioSource);
                }

                freeAudioSource.loop = false;
                freeAudioSource.volume = volume;
                freeAudioSource.clip = clip;
                
                if (isMusic)
                {
                    _currentMusicSource = freeAudioSource;
                    freeAudioSource.loop = true;
                }
                
                freeAudioSource.Play();
            }
        }

        public void Dispose()
        {
            RewardedAd.BeforeShow -= OnAdShowed;
            InterstitialAd.BeforeShow -= OnAdShowed;
        }
    }
}