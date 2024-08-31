using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace Source.Modules.ServiceModule.Scripts.Dialogs.Visual
{
    public class TransitionAnimation : MonoBehaviour
    {
        public bool IsClosed { get; private set; } = true;

        [SerializeField] private CanvasGroup _fadeParts;
        [SerializeField] private Transform _upperPart;
        [SerializeField] private Transform _upperPoint;
        [SerializeField] private Transform _lowerPart;
        [SerializeField] private Transform _lowerPoint;
        
        private Vector3 _upperPartDef;
        private Vector3 _lowerPartDef;
        
        private float _curtainDuration = 0.5f;
        private float _fadeDuration = 0.2f;
        
        private Sequence _seq;
        
        public void Initialize()
        {
            _upperPartDef = _upperPart.position;
            _lowerPartDef = _lowerPart.position;
        }

        public void InstantOpen()
        {
            if (IsClosed==false)
            {
                return;
            }

            var upperPos = _upperPart.position;
            upperPos.y = _upperPoint.position.y;
            _upperPart.position = upperPos;
            
            var lowerPos = _lowerPart.position;
            lowerPos.y = _lowerPoint.position.y;
            _lowerPart.position = lowerPos;
            
            _fadeParts.alpha = 0f;
            IsClosed = false;
        }
        
        [Button]
        public void Close(Action onComplete=null)
        {
            if (IsClosed)
            {
                onComplete?.Invoke();
                return;
            }
            
            _seq?.Kill();
            _fadeParts.blocksRaycasts = true;
            _seq = DOTween.Sequence();
            _seq
                .AppendInterval(0.5f)
                .Append(_upperPart.DOMoveY(_upperPartDef.y, _curtainDuration).SetEase(Ease.Linear))
                .Join(_lowerPart.DOMoveY(_lowerPartDef.y, _curtainDuration).SetEase(Ease.Linear))
                .Join(_fadeParts.DOFade(1, _fadeDuration).SetEase(Ease.Linear))
                .OnComplete(()=>
                {
                    IsClosed = true;
                    onComplete?.Invoke();
                });
        }

        [Button]
        public void Open(Action onComplete=null)
        {
            if (IsClosed==false)
            {
                onComplete?.Invoke();
                return;
            }
            
            _seq?.Kill();
            _seq = DOTween.Sequence();
            _seq
                .AppendInterval(0.5f)
                .Append(_upperPart.DOMoveY(_upperPoint.position.y, _curtainDuration).SetEase(Ease.Linear))
                .Join(_lowerPart.DOMoveY(_lowerPoint.position.y, _curtainDuration).SetEase(Ease.Linear))
                .Join(_fadeParts.DOFade(0, _fadeDuration).SetEase(Ease.Linear))
                .OnComplete(()=>
                {
                    IsClosed = false;
                    _fadeParts.blocksRaycasts = false;
                    onComplete?.Invoke();
                }).OnKill(()=> _fadeParts.blocksRaycasts = false);
        }

        private void OnDisable()
        {
            _seq?.Kill();
        }
    }
}
