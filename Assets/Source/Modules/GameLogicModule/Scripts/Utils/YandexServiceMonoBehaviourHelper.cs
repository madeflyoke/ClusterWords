using Agava.YandexGames;
using UniRx;
using UnityEngine;

namespace Source.Modules.GameLogicModule.Scripts.Utils
{
    public class YandexServiceMonoBehaviourHelper : MonoBehaviour
    {
        private bool _isInitialized;
        private bool _gameplayActiveWhileFocused;
        
        public void Initialize()
        {
            RewardedAd.S_isVideoAdOpen.ObserveEveryValueChanged(x => x.Value).Skip(1)
                .Subscribe(isOpened =>
            {
                if (isOpened)
                {
                    YandexGamesSdk.GameplayStop();
                }
                else
                {
                    YandexGamesSdk.GameplayStart();
                }
            }).AddTo(this);
            
            InterstitialAd.S_isVideoAdOpen.ObserveEveryValueChanged(x => x.Value).Skip(1)
                .Subscribe(isOpened =>
                {
                    if (isOpened)
                    {
                        YandexGamesSdk.GameplayStop();
                    }
                    else
                    {
                        YandexGamesSdk.GameplayStart();
                    }
                }).AddTo(this);
            _isInitialized = true;
        }
        
        private void OnApplicationFocus(bool hasFocus)
        {
            if (_isInitialized)
            {
                if (hasFocus)
                {
                    if (_gameplayActiveWhileFocused)
                    {
                        YandexGamesSdk.GameplayStart();
                    }
                }
                else
                {
                    _gameplayActiveWhileFocused = YandexGamesSdk.IsGameplayStopped==false;
                    YandexGamesSdk.GameplayStop();
                }
            }
        }
    }
}
