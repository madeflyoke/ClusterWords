using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Source.Modules.AudioModule.Scripts
{
    public class AudioPlayer
    {
        public bool SoundActive { get; private set; } = true; //default values
        public bool MusicActive { get; private set; } = true;
        
        private readonly Dictionary<SoundType, AudioClip> _audioClips;
        private readonly AudioSource _audioSourcePrefab;
        
        private readonly List<AudioSource> _poolAudioSource = new();
        private AudioSource _currentMusicSource;
        
        public AudioPlayer(Dictionary<SoundType, AudioClip> audioClips,AudioSource audioSourcePrefab)
        {
            _audioClips = audioClips;
            _audioSourcePrefab = audioSourcePrefab;
        }

        public void ActiveSound()
        {
            SoundActive = true;
        }

        public void DeActiveSound()
        {
            SoundActive = false;
        }

        public void ActiveMusic()
        {
            MusicActive = true;
            _currentMusicSource.volume = 1f;
        }

        public void DeActiveMusic()
        {
            MusicActive = false;
            _currentMusicSource.volume = 0f;
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