using System;
using Agava.YandexGames;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Modules.ServiceModule.Scripts.Dialogs.Variants.Gameplay
{
    public class HintButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _countText;
        [SerializeField] private Image _adImage;
        private int _maxCount;
        private int _currentCount;
        
        public void Initialize(int count, Action onClick)
        {
            _maxCount = count;
            ResetCount();
            
            _button.onClick.AddListener(()=>
            {
                if (_currentCount<=0)
                {
                    RewardedAd.Show(onRewardedCallback:RewardedCallback);
                    return;
                }
                onClick?.Invoke();
                Mathf.Clamp(--_currentCount, 0, _maxCount);
                RefreshView();
            });
        }
        
        
        private void RewardedCallback()
        {
            ResetCount();
        }
        
        private void ResetCount()
        {
            _currentCount = _maxCount;
            RefreshView();
        }

        private void RefreshView()
        {
            var countInvalid = _currentCount <= 0;
            _adImage.gameObject.SetActive(countInvalid);
            _countText.gameObject.SetActive(!countInvalid);
            
            _countText.text = _currentCount.ToString();
        }

        private void OnDisable()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}
