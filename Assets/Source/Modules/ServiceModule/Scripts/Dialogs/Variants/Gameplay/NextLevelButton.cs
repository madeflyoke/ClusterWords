using System;
using DG.Tweening;
using Source.Modules.ServiceModule.Scripts.Dialogs.Visual;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Modules.ServiceModule.Scripts.Dialogs.Variants.Gameplay
{
    public class NextLevelButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private ScalePulser _scalePulser;
        private Tween _tween;

        public void Activate(Action onClick)
        {
            _tween?.Kill();
            var defaultScale = transform.localScale;
            transform.localScale = Vector3.zero;
            gameObject.SetActive(true);
            _tween = transform.DOScale(defaultScale, .1f).SetEase(Ease.Linear).OnKill(() =>
            {
                transform.localScale = defaultScale;
            }).OnComplete(() =>
            {
                _scalePulser.StartPulser();
                _button.onClick.AddListener(()=>
                {
                    _button.onClick.RemoveAllListeners();
                    onClick?.Invoke();
                });
            });
        }

        public void HideInstant()
        {
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            _tween?.Kill();
        }
    }
}
