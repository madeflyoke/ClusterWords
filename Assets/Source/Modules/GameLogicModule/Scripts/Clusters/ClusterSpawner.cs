using System.Collections.Generic;
using System.Linq;
using Source.Modules.GameLogicModule.Scripts.Words;
using UnityEngine;

namespace Source.Modules.GameLogicModule.Scripts.Clusters
{
    public class ClusterSpawner : MonoBehaviour
    {
        [SerializeField] private ClusterArea _clusterArea;
        [SerializeField] private Transform _dragParent;
        [SerializeField] private ClusterCompositeRoot _clusterCompositeRoot;

        public Transform AvailableClustersParent => _clusterArea.GetAvailableClusterParent();
        public Transform DraggedClustersParent => _dragParent;

        public void SpawnClusters(IEnumerable<Word> words)
        {
            foreach (Cluster<char> wordCluster in words.SelectMany(x=>x.WordClusters.ShuffledCopy()).ShuffledCopy())
            {
                ClusterCompositeRoot clusterCompositeRoot = Instantiate(_clusterCompositeRoot, _clusterArea.GetAvailableClusterParent());
                clusterCompositeRoot.Init(wordCluster);
            }
        }
    }
}
