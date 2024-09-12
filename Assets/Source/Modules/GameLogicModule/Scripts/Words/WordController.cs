using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using Source.Modules.GameLogicModule.Scripts.Clusters;
using Source.Modules.GameLogicModule.Scripts.Words.WordCells;
using UnityEngine;

namespace Source.Modules.GameLogicModule.Scripts.Words
{
    public class WordController : MonoBehaviour
    {
        public event Action<WordController> WordCreated;
        public event Action<WordController> WordChanged;

        public bool IsEmpty => _clusters.Keys.Count == 0;
        public int CellsCount => _wordCellControllers.Count;

        public Transform ClustersParent => _clusterParent;
        private readonly Dictionary<ClusterController, List<int>> _clusters = new();
        
        [SerializeField] private WordView _wordView;
        [SerializeField] private Transform _clusterParent;
        private List<WordCellController> _wordCellControllers;
        private WordModel _wordModel;
        
        public void Initialize(List<WordCellController> wordCellControllers)
        {
            _wordModel = new WordModel();
            _wordCellControllers = wordCellControllers;
            for (var i = 0; i < _wordCellControllers.Count; i++)
            {
                _wordCellControllers[i].SetCellIndex(i);
            }
            _clusterParent.SetAsLastSibling();
        }

        public List<Cluster<char>> GetCurrentClusters()
        {
            return _clusters.Keys.Select(x => x.GetCluster()).ToList();
        }

        public WordCellController GetClosestEmptyCell()
        {
            return _wordCellControllers.FirstOrDefault(x => x.IsEmptyCell());
        }
        
        public Word GetCurrentWord() => _wordModel.CurrentWord;
        
        public void AnimateWordCells(Cluster<char> cluster, int startIndex)
        {
            for (var i = 0; i < cluster.ClusterItems.Count; i++) 
                _wordCellControllers[i + startIndex].StartAnimate();
        }

        public Vector3 GetAveragePositionBetweenCells(Cluster<char> cluster, int startIndex)
        {
            var finalPos = _wordCellControllers[0].transform.position;
            finalPos.x = _wordCellControllers.GetRange(startIndex, cluster.ClusterItems.Count)
                .Average(x => x.transform.position.x);
            return finalPos;
        }

        public void StopAnimateCells()
        {
            foreach (WordCellController wordCellController in _wordCellControllers.Where(x => x.IsEmptyCell()))
                wordCellController.StopAnimate();
        }

        public bool CanAddCluster(Cluster<char> cluster, int index)
        {
            if (index + cluster.ClusterItems.Count > _wordCellControllers.Count) return false;
            
            for (var i = 0; i < cluster.ClusterItems.Count; i++)
            {
                if (_wordCellControllers[i + index].IsEmptyCell() == false) return false;
            }

            return true;
        }

        public void AddCluster(ClusterController clusterController, int index)
        {
            var cluster = clusterController.GetCluster();
            
            if (CanAddCluster(cluster, index) == false) return;

            if (_clusters.ContainsKey(clusterController)) RemoveCluster(clusterController);

            List<int> clustersIndex = new(cluster.ClusterItems.Count);
            for (int i = index, j = 0; j < cluster.ClusterItems.Count; i++, j++)
            {
                _wordCellControllers[i].UpdateCellItem(cluster.ClusterItems[j]);
                clustersIndex.Add(i);
            }

            _clusters.Add(clusterController, clustersIndex);
            RegenerateCurrentWord();
            
            if (_wordModel.CurrentWord.CellsCount == _wordCellControllers.Count)
            {
                WordCreated?.Invoke(this);
            }
        }

        public void RemoveCluster(ClusterController clusterController)
        {
            var cluster = clusterController.GetCluster();
            for (var i = 0; i < cluster.ClusterItems.Count; i++)
                _wordCellControllers[_clusters[clusterController][i]].UpdateCellItem(null);
            
            _clusters.Remove(clusterController);
            
            RegenerateCurrentWord();
        }

        public void MarkAsCompleted()
        {
            _wordView.SetViewState(true);
            _clusters.Keys.ForEach(x => x.SetActiveState(false));
            enabled = false;
        }

        private void RegenerateCurrentWord()
        {
            _wordModel.CurrentWord.WordClusters.Clear();
            foreach (var keyValuePair in _clusters.OrderBy(x => x.Value[0]))
                _wordModel.CurrentWord.AddCluster(keyValuePair.Key.GetCluster());
            WordChanged?.Invoke(this);
        }

        private class WordModel
        {
            public Word CurrentWord { get; private set; } = new Word();
        }
    }
}