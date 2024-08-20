using Source.Modules.GameLogicModule.Scripts.Clusters;
using Source.Modules.GameLogicModule.Scripts.Levels;
using Source.Modules.GameLogicModule.Scripts.Words;
using UnityEngine;
using Zenject;

namespace Source.Modules.GameLogicModule.Scripts.Installers
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private HintController _hintController;
        [SerializeField] private WordsHandler _wordsHandler;
       // [SerializeField] private Canvas _canvas;
        private LevelWordsHolder _levelWordsHolder;
        
        public override void InstallBindings()
        {
            _levelWordsHolder = new();
            Container.Bind<LevelWordsHolder>().FromInstance(_levelWordsHolder).AsSingle();
            Container.Bind<WordsHandler>().FromInstance(_wordsHandler).AsSingle();
         //   Container.Bind<Canvas>().FromInstance(_canvas).AsSingle();
            Container.Bind<HintController>().FromInstance(_hintController).AsSingle();
        }
    }
}