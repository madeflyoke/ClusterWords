using System.Collections.Generic;
using System.Linq;
using Source.Modules.GameLogicModule.Scripts.Levels;
using UnityEngine;

namespace Source.Modules.GameLogicModule.Scripts.Clusters
{
    public class ClusterHandler : MonoBehaviour
    {
        public List<ClusterController> ClusterControllers { get; private set; }
        
        [SerializeField] private ClusterSpawner _clusterSpawner;

        public void Initialize(LevelWordsHolder wordsHolder)
        {
            ClusterControllers = _clusterSpawner.SpawnClusters(wordsHolder.Words)
                .Select(x=>x.ClusterController).ToList();
        }
    }
}
