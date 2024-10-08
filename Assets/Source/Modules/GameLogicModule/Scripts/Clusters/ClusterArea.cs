using System.Collections.Generic;
using System.Linq;
using Source.Modules.GameLogicModule.Scripts.UI;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

namespace Source.Modules.GameLogicModule.Scripts.Clusters
{
    public class ClusterArea : MonoBehaviour
    {
        private const int MAX_CLUSTERS_PER_BLOCK = 4;

        [SerializeField] private UIElementsBlock _blockPrefab;
        private List<UIElementsBlock> _currentBlocks;

        private void Awake()
        {
            _currentBlocks = new List<UIElementsBlock>();
            
            transform.ObserveEveryValueChanged(x => x.transform.childCount)
                .Subscribe(_ => LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform))
                .AddTo(this);
        }
        
        public Transform GetAvailableClusterParent()
        {
            UIElementsBlock availableBlock = _currentBlocks.FirstOrDefault(x => x.transform.childCount < x.Capacity);
            if (availableBlock==null)
            {
                availableBlock =Instantiate(_blockPrefab, transform).Initialize(MAX_CLUSTERS_PER_BLOCK, 7f);
                _currentBlocks.Add(availableBlock);
            }

            return availableBlock.transform;
        }
    }
}
