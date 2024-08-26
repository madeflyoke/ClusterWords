using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Modules.ServiceModule.Scripts.Dialogs.Variants.MainMenu
{
    public class LevelSelectButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private Image _disabledImage;
        [SerializeField] private Color _disabledTextColor;
        
        public void Initialize(int titleId, bool isLocked, Action onClick)
        {
            _levelText.text = titleId.ToString();
            _disabledImage.gameObject.SetActive(false);

            if (isLocked)
            {
                _levelText.color = _disabledTextColor;
                _disabledImage.gameObject.SetActive(true);
                _button.interactable = false;
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
