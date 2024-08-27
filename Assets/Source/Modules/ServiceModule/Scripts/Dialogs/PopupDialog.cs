using System;
using Source.Modules.ServiceModule.Scripts.Dialogs.Visual;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Modules.ServiceModule.Scripts.Dialogs
{
    public class PopupDialog : Dialog
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _closeAreaButton;
        [SerializeField] private PopupAnimator _popupAnimator;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        protected override void Start()
        {
            base.Start();
            if(_closeButton == null) return;
            _closeButton.onClick.AddListener(Hide);
            
            if(_closeAreaButton == null) return;
            _closeAreaButton.onClick.AddListener(Hide);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if(_closeButton == null) return;
            _closeButton.onClick.RemoveListener(Hide);
            
            if(_closeAreaButton == null) return;
            _closeAreaButton.onClick.RemoveListener(Hide);
        }

        public override void Show(Action onComplete =null)
        {
            _popupAnimator.PlayShowAnimation(()=>base.Show(onComplete));
        }

        public override void Hide()
        {
            _popupAnimator.PlayHideAnimation(()=>base.Hide());
        }
    }
}
