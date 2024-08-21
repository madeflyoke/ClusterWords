using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Modules.ServiceModule.Scripts.Dialogs.Variants
{
    public class LoadingScreenDialog : Dialog
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Image _floatingImage;
        [SerializeField] private float _xOffsetSpeed = 0.04f;
        [SerializeField] private Image _rotatingImage;
        [SerializeField] private float _rotatingImageDelay;
        [SerializeField] private float _rotatingImageSpeed;
        private Sequence _rotatingSeq;

        protected override void Start()
        {
            base.Start();
            _canvasGroup.alpha = 0f;

            return;
            var defaultRot = _rotatingImage.transform.rotation.eulerAngles;
            _rotatingSeq?.Kill();
            _rotatingSeq = DOTween.Sequence();
            _rotatingSeq.Append(_rotatingImage.transform.DORotate(defaultRot + Vector3.up * 180f, _rotatingImageSpeed)
                    .SetEase(Ease.Linear))
                .AppendInterval(_rotatingImageDelay)
                .SetLoops(-1, LoopType.Restart);
        }

        [Button]
        public void SetAlpha(float duration)
        {
            _canvasGroup.alpha = 0f;
            DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 1f, duration);
        }
        
        private void Update()
        {
           // _floatingImage.material.mainTextureOffset += Vector2.left * Time.deltaTime * _xOffsetSpeed;
        }

        protected override void OnDestroy()
        {
            _rotatingSeq?.Kill();
            base.OnDestroy();
        }
    }
}
