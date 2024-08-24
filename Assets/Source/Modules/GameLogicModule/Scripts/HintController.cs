using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Source.Modules.GameLogicModule.Scripts.Clusters;
using Source.Modules.GameLogicModule.Scripts.Levels;
using Source.Modules.GameLogicModule.Scripts.Words;
using UnityEngine;
using Zenject;

namespace Source.Modules.GameLogicModule.Scripts
{
    public class HintController : MonoBehaviour
    {
        private List<Word> LeftWords => _levelWordsHolder.Words.Except(_wordsHandler.GuessesWords).ToList();
        
        [SerializeField] private WordsHandler _wordsHandler;
        private LevelWordsHolder _levelWordsHolder;
        private List<ClusterController> _clusterControllers;
        
        public void Initialize(List<ClusterController> clusterControllers, LevelWordsHolder levelWordsHolder)
        {
            _clusterControllers = clusterControllers;
            _levelWordsHolder = levelWordsHolder;
        }
        
        [Button]
        public bool Hint()
        {
            return TryFindPartialWords() || TryFindEmptyWords(); //order matter
        }
        
        private bool TryFindPartialWords()
        {
            foreach (var partialWordController in _wordsHandler.WordControllers //if partial words exists
                         .Where(x => x.enabled && x.IsEmpty == false))
            {
                List<Cluster<char>> partialClusters = partialWordController.GetCurrentClusters();

                Cluster<char> missingCluster = null;
                for (int i = 0; i < LeftWords.Count; i++)
                {
                    List<Cluster<char>> wordClusters = LeftWords[i].WordClusters;

                    if (partialClusters.Select((pc, index) => 
                            index < wordClusters.Count && pc.Equals(wordClusters[index])).All(match => match)
                        && partialWordController.CellsCount==LeftWords[i].CellsCount)
                    {
                        missingCluster = wordClusters.Except(partialClusters).FirstOrDefault();
                        break;
                    }
                }

                ClusterController cluster = _clusterControllers
                    .FirstOrDefault(x => x.IsAddedToWord==false && x.GetCluster() == missingCluster);

                if (cluster!=null)
                {
                    var cell = partialWordController.GetClosestEmptyCell();
                    if (cell.CanAddCluster(cluster))
                    {
                        cluster.SetClusterToWordController(cell);
                        return true;
                    }
                }
            }

            return false;
        }

        private bool TryFindEmptyWords()
        {
            var emptyWordController = _wordsHandler.WordControllers
                .FirstOrDefault(x => x.enabled && x.IsEmpty);
            if (emptyWordController!=null)
            {
                List<Cluster<char>> firstClusters = LeftWords.Where(x=>x.CellsCount==emptyWordController.CellsCount)
                    .Select(x=>x.WordClusters[0]).ToList();

                var allClusters = _clusterControllers.Where(x => x.IsAddedToWord==false).ToList();
                
                foreach (var firstCluster in firstClusters)
                {
                    ClusterController cluster = allClusters.FirstOrDefault(x=>x.GetCluster().Equals(firstCluster));
                    if (cluster!=null)
                    {
                        var cell = emptyWordController.GetClosestEmptyCell();
                        if (cell.CanAddCluster(cluster))
                        {
                            cluster.SetClusterToWordController(cell);
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
