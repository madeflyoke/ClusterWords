using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Source.Modules.ServiceModule.Scripts.Dialogs.Visual
{
    [RequireComponent(typeof(Button))]
    public class ButtonAnimationPuncher : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Button _button;
        [SerializeField] private float _punchForce = 0.95f;
        private Tween _tween;
        private Vector3 _defaultScale;

        private void Awake()
        {
            _defaultScale = _button.transform.localScale;
        }
        
        private void OnDisable()
        {
            _tween?.Kill();
            _button.transform.localScale = _defaultScale;
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_button.interactable)
            {
                return;
            }
            
            _tween?.Kill();
            _tween = _button.transform.DOScale(_defaultScale * _punchForce, 0.1f).SetEase(Ease.Linear).SetUpdate(true);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_button.interactable)
            {
                return;
            }
            
            _tween?.Kill();
            _tween = _button.transform.DOScale(_defaultScale, 0.1f).OnKill(()=>
            {
                _button.transform.localScale = _defaultScale;
            }).SetEase(Ease.Linear).SetUpdate(true);
        }
        
#if UNITY_EDITOR

        private void OnValidate()
        {
            if (_button==null)
            {
                _button = GetComponent<Button>();
            }
        }

#endif
    }
}
