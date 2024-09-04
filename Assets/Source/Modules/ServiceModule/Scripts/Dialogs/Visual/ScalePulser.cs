using System;
using DG.Tweening;
using UnityEngine;

namespace Source.Modules.ServiceModule.Scripts.Dialogs.Visual
{
    public class ScalePulser : MonoBehaviour
    {
        [SerializeField] private bool _autoStart = false;
        private Tween _tween;
        
        public void StartPulser()
        {
            _tween?.Kill();
            _tween = transform.DOPunchScale(Vector3.one * 0.05f, 1.5f, 1).SetLoops(-1, LoopType.Restart).SetEase(Ease.InOutSine)
                .SetUpdate(true);
        }

        public void OnEnable()
        {
            if (_autoStart)
            {
                StartPulser();
            }
        }
        
        public void OnDisable()
        {
            _tween?.Kill();
        }
    }
}
