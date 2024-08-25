using System.Collections.Generic;
using System.Linq;
using Source.Modules.GameLogicModule.Scripts.Levels;
using Source.Modules.SignalsModule.Scripts;
using UnityEngine;
using UnityEngine.UI;
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
        
        public IReadOnlyCollection<Word> GuessesWords => _guessedWords.AsReadOnly();
        public IReadOnlyCollection<WordController> WordControllers => _wordControllers.AsReadOnly();
        
        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        public void Initialize(LevelWordsHolder levelWordsHolder)
        {
            _levelWordsHolder = levelWordsHolder;
            
            _wordControllers = _wordsSpawner.SpawnWords(_levelWordsHolder.Words)
                .Select(x => x.WordController).ToList();
            _wordControllers.ForEach(x=>x.WordCreated += OnWordCreated);
            _wordControllers.ForEach(x => x.WordChanged += OnWordChanged);
        }

        private void ValidateCompleting()
        {
            if (_guessedWordsControllers.Count != _levelWordsHolder.Words.Count) return;
            
            _signalBus.Fire<LevelCompleteSignal>();
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
                ValidateCompleting();
            }
        }
        
        private void OnDestroy()
        {
            _wordControllers.ForEach(x=>x.WordCreated -= OnWordCreated);
            _wordControllers.ForEach(x => x.WordChanged -= OnWordChanged);
        }
    }
}
