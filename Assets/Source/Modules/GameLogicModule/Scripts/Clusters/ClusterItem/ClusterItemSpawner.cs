using System.Collections.Generic;
using UnityEngine;

namespace Source.Modules.GameLogicModule.Scripts.Clusters.ClusterItem
{
    public class ClusterItemSpawner : MonoBehaviour
    {
        [SerializeField] private Transform _parent;
        [SerializeField] private ClusterItemCompositeRoot _clusterItemCompositeRootPrefab;

        public List<ClusterItemCompositeRoot> SpawnClusterItems(Cluster<char> cluster)
        {
            var items = new List<ClusterItemCompositeRoot>();
            foreach (var clusterClusterItem in cluster.ClusterItems)
            {
                ClusterItemCompositeRoot clusterItemCompositeRoot = Instantiate(_clusterItemCompositeRootPrefab, _parent);
                clusterItemCompositeRoot.Init(clusterClusterItem);
                items.Add(clusterItemCompositeRoot);
            }

            return items;
        }
    }
}