using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

namespace Source.Modules.GameLogicModule.Scripts.Clusters
{
    public class ClusterArea : MonoBehaviour
    {
        [SerializeField] private GameObject _wordsBlockPrefab;
        [SerializeField] private int _wordsBlockCapacity = 5;
        private List<GameObject> _currentWordsBlocks;

        private void Awake()
        {
            _currentWordsBlocks = new List<GameObject>();
            
            transform.ObserveEveryValueChanged(x => x.transform.childCount)
                .Subscribe(_ => LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform))
                .AddTo(this);

        }

        public Transform GetAvailableClusterParent()
        {
            var availableBlock = _currentWordsBlocks.FirstOrDefault(x => x.transform.childCount < _wordsBlockCapacity);
            if (availableBlock==null)
            {
                availableBlock =Instantiate(_wordsBlockPrefab, transform);
                _currentWordsBlocks.Add(availableBlock);
            }

            return availableBlock.transform;
        }
    }
}
