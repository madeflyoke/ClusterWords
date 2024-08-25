using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Modules.ServiceModule.Scripts.Dialogs.Variants.MainMenu
{
    public class LevelSelectButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private float _lockedAlpha;
        
        public void Initialize(int titleId, bool isLocked, Action onClick)
        {
            _levelText.text = titleId.ToString();

            if (isLocked)
            {
                _canvasGroup.alpha = _lockedAlpha;
                return;
            }

            _button.onClick.AddListener(()=>
            {
                _button.onClick.RemoveAllListeners();
                onClick?.Invoke();
            });
        }

        private void OnDisable()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}
