using System.Collections.Generic;
using System.Linq;
using Agava.YandexGames;
using Source.Modules.ServiceModule.Scripts.Player.Settings;
using UnityEngine;

namespace Source.Modules.ServiceModule.Scripts.Audio
{
    public class AudioPlayer
    {
        public bool SoundActive { get; private set; } //default values
        public bool MusicActive { get; private set; }
        
        private readonly Dictionary<SoundType, AudioClip> _audioClips;
        private readonly AudioSource _audioSourcePrefab;
        
        private readonly List<AudioSource> _poolAudioSource = new();
        private AudioSource _currentMusicSource;
        private SettingsHandler _settingsHandler;
        
        public AudioPlayer(Dictionary<SoundType, AudioClip> audioClips,AudioSource audioSourcePrefab, SettingsHandler settingsHandler)
        {
            _audioClips = audioClips;
            _audioSourcePrefab = audioSourcePrefab;

            _settingsHandler = settingsHandler;
            _settingsHandler.GetAudioStatus(out bool musicActive, out bool soundActive);
            
            if (musicActive)
                ActivateMusic();
            else
                DeActivateMusic();

            if (soundActive)
                ActivateSound();
            else
                DeActivateSound();
            
            RewardedAd.BeforeRewardShow += OnAdShowed;
        }

        private void OnAdShowed()
        {
            RewardedAd.AfterRewardShow += OnAdClosed;
        }

        private void OnAdClosed()
        {
            RewardedAd.AfterRewardShow -= OnAdClosed;
            
        }

        public void ActivateSound()
        {
            SoundActive = true;
        }

        public void DeActivateSound()
        {
            SoundActive = false;
        }

        public void ActivateMusic()
        {
            MusicActive = true;
            if (_currentMusicSource!=null)
            {
                _currentMusicSource.mute = false;
            }
        }

        public void DeActivateMusic()
        {
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
    }
}