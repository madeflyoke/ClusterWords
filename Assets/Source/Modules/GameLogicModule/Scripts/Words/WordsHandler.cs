using System.Collections.Generic;
using System.Linq;
using Source.Modules.AudioModule.Scripts;
using Source.Modules.GameLogicModule.Scripts.Levels;
using Source.Modules.ServiceModule.Scripts;
using Source.Modules.ServiceModule.Scripts.Progress.Currency;
using Source.Modules.SignalsModule.Scripts;
using UnityEngine;
using Zenject;

namespace Source.Modules.GameLogicModule.Scripts.Words
{
    public class WordsHandler : MonoBehaviour
    {
        [SerializeField] private WordsSpawner _wordsSpawner;
        
        private readonly List<WordController> _guessedWordsControllers = new();
        private readonly List<Word> _guessedWords = new();
        
        private LevelWordsHolder _levelWordsHolder;
        private List<WordController> _wordControllers;
        private SignalBus _signalBus;
        private ProgressService _progressService;
        private LevelContainer _levelContainer;
        private AudioPlayer _audioPlayer;
        
        public IReadOnlyCollection<Word> GuessesWords => _guessedWords.AsReadOnly();
        public IReadOnlyCollection<WordController> WordControllers => _wordControllers.AsReadOnly();
        
        [Inject]
        private void Construct(SignalBus signalBus, ServicesHolder servicesHolder, LevelContainer levelContainer, AudioPlayer audioPlayer)
        {
            _signalBus = signalBus;
            _progressService = servicesHolder.GetService<ProgressService>();
            _levelContainer = levelContainer;
            _audioPlayer = audioPlayer;
        }
        
        public void Initialize(LevelWordsHolder levelWordsHolder)
        {
            _levelWordsHolder = levelWordsHolder;
            
            _wordControllers = _wordsSpawner.SpawnWords(_levelWordsHolder.Words)
                .Select(x => x.WordController).ToList();
            _wordControllers.ForEach(x=>x.WordCreated += OnWordCreated);
            _wordControllers.ForEach(x => x.WordChanged += OnWordChanged);
        }

        private bool ValidateCompleting()
        {
            if (_guessedWordsControllers.Count != _levelWordsHolder.Words.Count)
            {
                return false;
            }
            
            _progressService.LevelProgressHandler.SaveLastCompletedLevel(_levelContainer.CurrentLevelId);
            _signalBus.Fire<LevelCompleteSignal>();
            return true;
        }
        
        private void OnWordChanged(WordController wordController)
        {
            if (_levelWordsHolder.Words.Contains(wordController.GetCurrentWord())) return;
            
            if (_guessedWordsControllers.Contains(wordController))
            {
                _guessedWords.Remove(wordController.GetCurrentWord());
                _guessedWordsControllers.Remove(wordController);
            }
        }

        private void OnWordCreated(WordController wordController)
        {
            if (_levelWordsHolder.Words.Contains(wordController.GetCurrentWord()))
            {
                _guessedWords.Add(wordController.GetCurrentWord());
                _guessedWordsControllers.Add(wordController);
                wordController.MarkAsCompleted();
                if (ValidateCompleting())
                {
                    _audioPlayer.PlaySound(SoundType.LEVEL_COMPLETE, 0.3f);
                }
                else
                {
                    _audioPlayer.PlaySound(SoundType.WORD_COMPLETE, 0.15f);
                }
            }
        }
        
        private void OnDestroy()
        {
            _wordControllers.ForEach(x=>x.WordCreated -= OnWordCreated);
            _wordControllers.ForEach(x => x.WordChanged -= OnWordChanged);
        }
    }
}
