using Agava.YandexGames;
using UniRx;
using UnityEngine;

namespace Source.Modules.GameLogicModule.Scripts.Utils
{
    public class YandexServiceMonoBehaviourHelper : MonoBehaviour
    {
        private bool _isInitialized;
        
        public void Initialize()
        {
            RewardedAd.S_isVideoAdOpen.ObserveEveryValueChanged(x => x.Value)
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
            
            InterstitialAd.S_isVideoAdOpen.ObserveEveryValueChanged(x => x.Value)
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
                    if (InterstitialAd.S_isVideoAdOpen.Value || RewardedAd.S_isVideoAdOpen.Value)
                    {
                        return;
                    }
                    YandexGamesSdk.GameplayStart();
                }
                else
                {
                    YandexGamesSdk.GameplayStop();
                }
            }
        }
    }
}
