using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Source.Modules.GameLogicModule.Scripts.Clusters;
using Source.Modules.GameLogicModule.Scripts.Words;
using UnityEngine;
using Zenject;

namespace Source.Modules.GameLogicModule.Scripts.Levels
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private LevelsConfig _levelsConfig;
        [SerializeField] private ClusterSpawner _clusterSpawner;
        [SerializeField] private WordsSpawner _wordsSpawner;
        [SerializeField] private WordsHandler _wordsHandler;
        [SerializeField] private HintController _hintController;
        [SerializeField] private Canvas _canvas;

        private LevelWordsHolder _levelWordsHolder;

        public override void InstallBindings()
        {
            _levelWordsHolder = new ();
            var rawWords = new WordsFetcher().GetWords(_levelsConfig.GetLevelData(0).WordsRequests); 
            
#if UNITY_EDITOR
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Spawned words: ");
            foreach (var word in rawWords)
            {
                stringBuilder.Append(" "+word+" ");
            }
            Debug.LogWarning(stringBuilder);
#endif
            
            _levelWordsHolder.SetWords(CreateWords(rawWords));
            
            Container.Bind<ClusterSpawner>().FromInstance(_clusterSpawner).AsSingle();
            Container.Bind<WordsSpawner>().FromInstance(_wordsSpawner).AsSingle();
            Container.Bind<WordsHandler>().FromInstance(_wordsHandler).AsSingle();
            Container.Bind<LevelWordsHolder>().FromInstance(_levelWordsHolder).AsSingle();
            Container.Bind<Canvas>().FromInstance(_canvas).AsSingle();
            Container.Bind<HintController>().FromInstance(_hintController).AsSingle();
        }

        public override void Start()
        {
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
                List<Cluster<char>> clusters = SeparateWordByClusters(levelModelWord);
                result.Add(new Word(clusters));
            }
            
            return result;
        }
        
        private List<Cluster<char>> SeparateWordByClusters(string word) //it just works
        {
            #region ClustersAlgorithm

            List<string> clusters = new List<string>();
            
            int index = 0;
            int wordLength = word.Length;

            while (index < wordLength)
            {
                int remainingLength = wordLength - index;
               
                if (remainingLength == 1)
                {
                    break;
                }
       
                List<int> validClusterSizes = new List<int>();
                
                for (int i = GameConstants.MIN_CLUSTER_SIZE; i <= GameConstants.MAX_CLUSTER_SIZE; i++)
                {
                    if (remainingLength >=i && wordLength!=i)
                    {
                        validClusterSizes.Add(i);
                    }
                }
            
                if (validClusterSizes.Count > 0)
                {
                    int clusterSize = validClusterSizes[Random.Range(0,validClusterSizes.Count)];

                    if (remainingLength - clusterSize == 1 && validClusterSizes.Count > 1)
                    {
                        validClusterSizes.Remove(clusterSize);
                        clusterSize = validClusterSizes[Random.Range(0,validClusterSizes.Count)];
                    }

                    clusters.Add(word.Substring(index, clusterSize));
                    index += clusterSize;
                }
            }

            if (clusters.Count > 0 && index < wordLength)
            {
                clusters[^1] += word.Substring(index);
            }


            #endregion
            
            List<Cluster<char>> tempWordClusters = new List<Cluster<char>>();
            foreach (var clusterData in clusters)
            {
                Cluster<char> cluster = new Cluster<char>();
                foreach (var value in clusterData)
                {
                    cluster.AddItem(value);
                }
                tempWordClusters.Add(cluster);
            }
            
            return tempWordClusters;
        }
    }
}