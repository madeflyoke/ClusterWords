using UnityEngine;
using Zenject;

namespace Source.Modules.AudioModule.Scripts
{
    public class SoundInstaller : MonoInstaller
    {
        [SerializeField] private AudioSource _audioSourcePrefab;
        [SerializeField] private AudioContainer _audioContainer;
       
        public override void InstallBindings()
        {
            AudioPlayer audioPlayer = new(_audioContainer.AudioClips, _audioSourcePrefab);
            Container.Bind<AudioPlayer>().FromInstance(audioPlayer).AsSingle();
        }
    }
}