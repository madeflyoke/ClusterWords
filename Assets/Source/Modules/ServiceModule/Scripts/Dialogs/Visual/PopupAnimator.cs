using System;
using DG.Tweening;
using UnityEngine;

namespace Source.Modules.ServiceModule.Scripts.Dialogs.Visual
{
    public class PopupAnimator : MonoBehaviour
    {
        [SerializeField] private float _scaleMultiplier = 1.2f;
        [SerializeField] private Vector3 _defaultScale;
        private Tween _tween;
        
        public void PlayShowAnimation(Action onComplete=null, bool instant=false)
        {
            _tween?.Kill();
            
            if (instant==false)
            {
                transform.localScale /= _scaleMultiplier;
                gameObject.SetActive(true);
                _tween = transform.DOScale(_defaultScale, 0.065f).SetEase(Ease.Linear).SetUpdate(true).OnComplete(()=>onComplete?.Invoke());
            }
            else
            {
                gameObject.SetActive(true);
            }
        }

        public void PlayHideAnimation(Action onComplete=null, bool instant=false)
        {
            gameObject.SetActive(false);
            transform.localScale = _defaultScale;
            onComplete?.Invoke();
        }

        private void OnDisable()
        {
            _tween?.Kill();
        }
        
#if UNITY_EDITOR

        private void OnValidate()
        {
            _defaultScale = transform.localScale;
        }


#endif
    }
}
