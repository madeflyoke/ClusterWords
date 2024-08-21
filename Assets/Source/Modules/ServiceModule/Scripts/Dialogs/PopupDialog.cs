using UnityEngine;
using UnityEngine.UI;

namespace Source.Modules.ServiceModule.Scripts.Dialogs
{
    public class PopupDialog : Dialog
    {
        [SerializeField] private Button _closeButton;

        protected override void Start()
        {
            base.Start();
            if(_closeButton == null) return;
            _closeButton.onClick.AddListener(Hide);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if(_closeButton == null) return;
            _closeButton.onClick.RemoveListener(Hide);
        }
    }
}
