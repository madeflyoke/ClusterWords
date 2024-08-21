using System.Collections.Generic;
using System.Linq;
using Source.Modules.GameLogicModule.Scripts.UI;
using Source.Modules.GameLogicModule.Scripts.Utils;
using Source.Modules.GameLogicModule.Scripts.Words;
using UnityEngine;

namespace Source.Modules.GameLogicModule.Scripts.Clusters
{
    public class ClusterSpawner : MonoBehaviour
    {
        [SerializeField] private ClusterArea _clusterArea;
        [SerializeField] private Transform _dragParent;
        [SerializeField] private ClusterCompositeRoot _clusterCompositeRootPrefab;

        public Transform DraggedClustersParent => _dragParent;
        private Canvas _relatedCanvas;

        private void Awake()
        {
            _relatedCanvas = GetComponentInParent<Canvas>();
        }

        public List<ClusterCompositeRoot> SpawnClusters(IEnumerable<Word> words)
        {
            var clusterComposites = new List<ClusterCompositeRoot>();
            foreach (Cluster<char> wordCluster in words.SelectMany(x=>x.WordClusters.ShuffledCopy()).ShuffledCopy())
            {
                ClusterCompositeRoot clusterCompositeRoot = Instantiate(_clusterCompositeRootPrefab,
                    _clusterArea.GetAvailableClusterParent());
                clusterCompositeRoot.Init(wordCluster, this, _relatedCanvas);
                clusterComposites.Add(clusterCompositeRoot);
            }

            return clusterComposites;
        }

        public Transform GetAvailableClusterParent()
        {
            return _clusterArea.GetAvailableClusterParent();
        }
    }
}
