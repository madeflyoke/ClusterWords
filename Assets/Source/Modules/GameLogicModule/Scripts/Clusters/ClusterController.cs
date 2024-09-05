using System;
using System.Collections.Generic;
using System.Linq;
using Source.Modules.GameLogicModule.Scripts.Clusters.ClusterItem;
using Source.Modules.GameLogicModule.Scripts.Words;
using Source.Modules.GameLogicModule.Scripts.Words.WordCells;
using Source.Modules.ServiceModule.Scripts;
using Source.Modules.ServiceModule.Scripts.Audio;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Source.Modules.GameLogicModule.Scripts.Clusters
{
    public class ClusterController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public event Action<ClusterController> BeginDrag;
        public event Action<ClusterController> EndDrag;
        public event Action<ClusterController> Drag;

        public Transform Parent => _clusterView.transform;
        public bool IsAddedToWord { get; private set; }
        
        [SerializeField] private ClusterView _clusterView;
        [SerializeField] private ClusterItemSpawner _clusterItemSpawner;

        private ClusterModel _clusterModel;
        private Canvas _canvas;
        private GraphicRaycaster _raycaster;
        private ClusterSpawner _clusterSpawner;
        private WordController _previousWordController;
        private AudioPlayer _audioPlayer;

        private List<ClusterItemCompositeRoot> _relatedClusterItems;
        private List<RaycastResult> _cachedRaycastResults;
        private RectTransform _startClusterPoint;
      
        [Inject]
        private void Construct(ServicesHolder servicesHolder)
        {
            _audioPlayer = servicesHolder.GetService<AudioService>().AudioPlayer;
        }
        
        public void Initialize(ClusterModel clusterModel, ClusterSpawner clusterSpawner, Canvas relatedCanvas)
        {
            _canvas = relatedCanvas;
            _raycaster = _canvas.GetComponent<GraphicRaycaster>();
            
            _cachedRaycastResults = new();
            _clusterModel = clusterModel;
            _clusterSpawner = clusterSpawner;
            _relatedClusterItems = _clusterItemSpawner.SpawnClusterItems(clusterModel.Cluster);
            _startClusterPoint = _relatedClusterItems[0].transform as RectTransform;
        }

        public Cluster<char> GetCluster()
        {
            return _clusterModel.Cluster;
        }

        public void SetActiveState(bool isActive)
        {
            enabled = isActive;
            _clusterView.SetViewState(isActive);
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            BeginDrag?.Invoke(this);
            _clusterModel.SetParent(_canvas.transform);
            _clusterView.BeginDrag();
        }

        public void OnDrag(PointerEventData eventData)
        {
            Drag?.Invoke(this);
            
            _clusterModel.MoveCluster(eventData.delta/ _canvas.scaleFactor);
            
            GameObject currentGameObject = RaycastCheckForCell();
            
            if (currentGameObject != null &&
                currentGameObject.TryGetComponent(out WordCellController wordCellController))
            {
                if (_previousWordController != null) _previousWordController.StopAnimateCells();

                if (wordCellController.CanAddCluster(this))
                {
                    wordCellController.WordController.StopAnimateCells();
                    wordCellController.WordController.AnimateWordCells(_clusterModel.Cluster,
                        wordCellController.GetCellIndex());
                }
                else
                {
                    wordCellController.WordController.StopAnimateCells();
                }

                _previousWordController = wordCellController.WordController;
            }
            else if (_previousWordController != null) 
            {
               _previousWordController.StopAnimateCells();
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            EndDrag?.Invoke(this);
            GameObject currentGameObject = RaycastCheckForCell();;
            if (currentGameObject != null &&
                currentGameObject.TryGetComponent(out WordCellController wordCellController) &&
                wordCellController.CanAddCluster(this))
            {
                SetClusterToWordController(wordCellController);
            }
            else
            {
                IsAddedToWord = false;
                _clusterModel.SetParent(_clusterSpawner.GetAvailableClusterParent());
                _clusterView.EndDrag();
                _audioPlayer.PlaySound(SoundType.OUT_OF_CELL, .15f);
            }
        }

        public void SetClusterToWordController(WordCellController wordCellController)
        {
            IsAddedToWord = true;
            wordCellController.AddCluster(this);
            
            _clusterModel.UpdatePosition(wordCellController.WordController.GetAveragePositionBetweenCells(_clusterModel.Cluster,
                wordCellController.GetCellIndex()));
            _clusterModel.SetParent(_clusterSpawner.DraggedClustersParent);
            _clusterView.SetViewInCell();
            _audioPlayer.PlaySound(SoundType.SET_CLUSTER_IN_CELL, .6f);
            wordCellController.WordController.StopAnimateCells();
        }

        private GameObject RaycastCheckForCell()
        {
            _cachedRaycastResults.Clear();
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = _canvas.worldCamera.WorldToScreenPoint(_startClusterPoint.position)
            };
            
            _raycaster.Raycast(pointerData, _cachedRaycastResults);
            
            if (_cachedRaycastResults.Count>0)
            {
                var target = _cachedRaycastResults.FirstOrDefault(x => x.gameObject.layer != gameObject.layer).gameObject;
                if (target!=null)
                {
                    return target;
                }
            }

            return null;
        }
    }
}