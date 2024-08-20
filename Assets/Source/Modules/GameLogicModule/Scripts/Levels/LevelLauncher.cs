using System.Collections.Generic;
using System.Linq;
using System.Text;
using Source.Modules.GameLogicModule.Scripts.Clusters;
using Source.Modules.GameLogicModule.Scripts.Utils;
using Source.Modules.GameLogicModule.Scripts.Words;
using UnityEngine;
using Zenject;

namespace Source.Modules.GameLogicModule.Scripts.Levels
{
    public class LevelLauncher : MonoBehaviour
    {
        [SerializeField] private LevelsConfig _levelsConfig;
        [SerializeField] private ClusterSpawner _clusterSpawner;
        [SerializeField] private WordsSpawner _wordsSpawner;
        
        private LevelWordsHolder _levelWordsHolder;
        private HintController _hintController;
        private WordsHandler _wordsHandler;
        
        [Inject]
        public void Construct(LevelWordsHolder levelWordsHolder, HintController hintController, WordsHandler wordsHandler)
        {
            _levelWordsHolder = levelWordsHolder;
            _hintController = hintController;
            _wordsHandler = wordsHandler;
        }
        
        public async void LaunchLevel(int id)
        {
            _levelWordsHolder = new ();
            
            var rawWords = new WordsFetcher().GetWords(_levelsConfig.GetLevelData(id).WordsRequests); 
            _levelWordsHolder.SetWords(CreateWords(rawWords));
            
#if UNITY_EDITOR
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Spawned words: ");
            foreach (var word in rawWords)
            {
                stringBuilder.Append(" "+word+" ");
            }
            Debug.LogWarning(stringBuilder);
#endif
            
            List<ClusterController> clusterControllers = _clusterSpawner.SpawnClusters(_levelWordsHolder.Words)
                .Select(x=>x.ClusterController).ToList();
            
            List<WordController> wordControllers = _wordsSpawner.SpawnWords(_levelWordsHolder.Words)
                .Select(x => x.WordController).ToList();
            
            _wordsHandler.Initialize(wordControllers);
            _hintController.Initialize(clusterControllers);
        }
        
        private List<Word> CreateWords(List<string> rawWords)
        {
            List<Word> result = new();
            foreach (var levelModelWord in rawWords)
            {
                List<Cluster<char>> clusters = WordUtils.SeparateWordByClusters(levelModelWord);
                result.Add(new Word(clusters));
            }
            
            return result;
        }
    }
}
