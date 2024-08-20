using UnityEngine;

namespace Source.Modules.GameLogicModule.Scripts.Clusters
{
    public class ClusterCompositeRoot : MonoBehaviour
    {
        public ClusterController ClusterController => _clusterController;
        
        [SerializeField] private ClusterController _clusterController;
        public void Init(Cluster<char> cluster, ClusterSpawner clusterSpawner, Canvas relatedCanvas)
        {
            _clusterController.Init(new ClusterModel(cluster, _clusterController.Parent as RectTransform), clusterSpawner, relatedCanvas);
        }
    }
}