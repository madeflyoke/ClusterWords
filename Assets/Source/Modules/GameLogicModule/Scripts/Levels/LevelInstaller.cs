﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using Source.Modules.GameLogicModule.Scripts.Clusters;
using Source.Modules.GameLogicModule.Scripts.Words;
using UnityEngine;
using Zenject;

namespace Source.Modules.GameLogicModule.Scripts.Levels
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private ClusterSpawner _clusterSpawner;
        [SerializeField] private WordsSpawner _wordsSpawner;
        [SerializeField] private WordsHandler _wordsHandler;
        [SerializeField] private Canvas _canvas;

        private LevelData _levelData;

        public override void InstallBindings()
        {
            string path = Path.Combine(Application.persistentDataPath, "data.json");
            
            _levelData = new ();
            List<LevelModel> levelModels = new LevelModelReader().ReadData(path); 
            _levelData.SetWords(CreateWords(levelModels[0]));
            
            Container.Bind<ClusterSpawner>().FromInstance(_clusterSpawner).AsSingle();
            Container.Bind<WordsSpawner>().FromInstance(_wordsSpawner).AsSingle();
            Container.Bind<WordsHandler>().FromInstance(_wordsHandler).AsSingle();
            Container.Bind<LevelData>().FromInstance(_levelData).AsSingle();
            Container.Bind<Canvas>().FromInstance(_canvas).AsSingle();
        }

        public override void Start()
        {
            _clusterSpawner.SpawnClusters(_levelData.Words);
            List<WordController> wordControllers = _wordsSpawner.SpawnWords(_levelData.Words).Select(x => x.WordController).ToList();
            _wordsHandler.Initialize(wordControllers);
        }

        private List<Word> CreateWords(LevelModel levelModel)
        {
            List<Word> result = new();
            foreach (var levelModelWord in levelModel.Words)
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
          
                var maxClusterSize = 4;
                var minClusterSize = 2;
          
                for (int i = minClusterSize; i <= maxClusterSize; i++)
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