using UnityEngine;

namespace Source.Modules.GameLogicModule.Scripts.Clusters.ClusterItem
{
    public class ClusterItemSpawner : MonoBehaviour
    {
        [SerializeField] private Transform _parent;
        [SerializeField] private ClusterItemCompositeRoot _clusterItemCompositeRootPrefab;

        public void SpawnClusterItems(Cluster<char> cluster)
        {
            foreach (var clusterClusterItem in cluster.ClusterItems)
            {
                ClusterItemCompositeRoot clusterItemCompositeRoot = Instantiate(_clusterItemCompositeRootPrefab, _parent);
                clusterItemCompositeRoot.Init(clusterClusterItem);
            }
        }
    }
}