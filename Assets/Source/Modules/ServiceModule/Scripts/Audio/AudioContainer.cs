using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Source.Modules.ServiceModule.Scripts.Audio
{
    public class AudioContainer : SerializedMonoBehaviour
    {
        public Dictionary<SoundType, AudioClip> AudioClips => _audioClips;

        [field: SerializeField] public AudioSource AudioSourcePrefab { get; private set; }
        [SerializeField] private Dictionary<SoundType, AudioClip> _audioClips;
    }
}
