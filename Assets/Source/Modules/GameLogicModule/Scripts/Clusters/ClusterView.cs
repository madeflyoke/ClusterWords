using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Modules.GameLogicModule.Scripts.Clusters
{
    public class ClusterView : MonoBehaviour
    {
        [SerializeField] private Image _draggablePart;
        [SerializeField] private GameObject _background;
        [SerializeField] private Vector2 _beginDragScale;
        [SerializeField] private Vector2 _endDragScale;
        [SerializeField] private Vector2 _inCellScale;

        private void Start()
        {
            transform.DOScale(_endDragScale, 0);
        }

        public void SetViewState(bool isActive)
        {
            _background.SetActive(isActive);
        }
        
        public void BeginDrag()
        {
            _draggablePart.raycastTarget = false;
            transform.DOScale(_beginDragScale, 0.3f);
        }

        public void SetViewInCell()
        {
            transform.DOScale(_inCellScale, 0.3f);
            _draggablePart.raycastTarget = true;
        }
        
        public void EndDrag()
        {
            _draggablePart.raycastTarget = true;
            transform.DOScale(_endDragScale, 0.3f);
        }
    }
}