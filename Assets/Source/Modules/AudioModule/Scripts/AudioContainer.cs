using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Source.Modules.AudioModule.Scripts
{
    public class AudioContainer : SerializedMonoBehaviour
    {
        public Dictionary<SoundType, AudioClip> AudioClips => _audioClips;
        
        [SerializeField] private Dictionary<SoundType, AudioClip> _audioClips;
    }
}
