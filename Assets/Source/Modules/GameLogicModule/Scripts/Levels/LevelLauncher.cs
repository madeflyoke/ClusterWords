using System.Collections.Generic;
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
        [SerializeField] private ClusterHandler _clusterHandler;
        [SerializeField] private WordsHandler _wordsHandler;
        [SerializeField] private HintController _hintController;

        private LevelWordsHolder _levelWordsHolder;
        private LevelContainer _levelContainer;
        
        [Inject]
        public void Construct(LevelContainer levelContainer)
        {
            _levelContainer = levelContainer;
        }
        
        public async void LaunchLevel()
        {
            var rawWords = new WordsFetcher().GetWords(_levelsConfig.GetLevelData(_levelContainer.CurrentLevelId).WordsRequests); 
            // var rawWords = new List<string>() //EXAMPLE of debugging words
            // {
            //     "мама",
            //     "амма",
            //     "мага",
            //     "гама",
            //     "мамама"
            // };
            _levelWordsHolder = new(CreateWords(rawWords));
            
#if UNITY_EDITOR
            Debug.Log("Level started, id: "+_levelContainer.CurrentLevelId);
            
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Spawned words: ");
            foreach (var word in rawWords)
            {
                stringBuilder.Append(" "+word+" ");
            }
            Debug.Log(stringBuilder);
#endif
            
            _wordsHandler.Initialize(_levelWordsHolder);
            _clusterHandler.Initialize(_levelWordsHolder);
            _hintController.Initialize(_clusterHandler.ClusterControllers, _levelWordsHolder);
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
