using DG.Tweening;
using UnityEngine;

namespace Source.Modules.ServiceModule.Scripts.Dialogs.Visual
{
    public class ScalePulser : MonoBehaviour
    {
        private Vector3 _defaultScale;
        private Tween _tween;

        private void Awake()
        {
            _defaultScale = transform.localScale;
        }

        public void OnEnable()
        {
            _tween?.Kill();
            _tween = transform.DOPunchScale(Vector3.one * 0.05f, 1.5f, 1).SetLoops(-1, LoopType.Restart).SetEase(Ease.InOutSine)
                .SetUpdate(true).OnKill(()=>transform.localScale=_defaultScale);
        }
        public void OnDisable()
        {
            _tween?.Kill();
        }
    }
}
