using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace Agava.YandexGames
{
    /// <summary>
    /// Proxy for ysdk.adv.showRewardedVideo() method in YandexGames SDK.
    /// </summary>
    public static class RewardedAd
    {
        public static Action BeforeShow;
        public static Action AfterShow;
        
        // More statics to the god of statics.
        private static Action s_onOpenCallback;
        private static Action s_onRewardedCallback;
        private static Action s_onCloseCallback;
        private static Action<string> s_onErrorCallback;

        private static bool s_isVideoAdOpen = false;

        /// <summary>
        /// Shows the rewarded video ad.
        /// </summary>
        /// <remarks>
        /// Doesn't seem to have any call-per-minute limit (at the time of writing).
        /// </remarks>
        public static void Show(Action onOpenCallback = null, Action onRewardedCallback = null,
            Action onCloseCallback = null, Action<string> onErrorCallback = null)
        {
            // And this is where static fields backfire. Instant Karma.
            s_onOpenCallback = onOpenCallback;
            s_onRewardedCallback = onRewardedCallback;
            s_onCloseCallback = onCloseCallback;
            s_onErrorCallback = onErrorCallback;
            
#if UNITY_EDITOR

            BeforeShow?.Invoke();
            OnOpenCallback();
            OnRewardedCallback();
            OnCloseCallback();

#else
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                OnOpenCallback();
                OnCloseCallback();
                return;
            }

            BeforeShow?.Invoke();
            VideoAdShow(OnOpenCallback, OnRewardedCallback, OnCloseCallback, OnErrorCallback);
#endif

        }

        [DllImport("__Internal")]
        private static extern bool VideoAdShow(Action openCallback, Action rewardedCallback, Action closeCallback, Action<string> errorCallback);

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnOpenCallback()
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(RewardedAd)}.{nameof(OnOpenCallback)} invoked");

            s_isVideoAdOpen = true;

            s_onOpenCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnRewardedCallback()
        {
            if (!s_isVideoAdOpen)
            {
                if (YandexGamesSdk.CallbackLogging)
                    Debug.Log($"Ignoring {nameof(RewardedAd)}.{nameof(OnRewardedCallback)} because {nameof(s_isVideoAdOpen)} is {s_isVideoAdOpen}");

                return;
            }

            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(RewardedAd)}.{nameof(OnRewardedCallback)} invoked");

            s_onRewardedCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnCloseCallback()
        {
            AfterShow?.Invoke();

            if (!s_isVideoAdOpen)
            {
                if (YandexGamesSdk.CallbackLogging)
                    Debug.Log($"Ignoring {nameof(RewardedAd)}.{nameof(OnCloseCallback)} because {nameof(s_isVideoAdOpen)} is {s_isVideoAdOpen}");

                return;
            }

            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(RewardedAd)}.{nameof(OnCloseCallback)} invoked");

            s_isVideoAdOpen = false;

            s_onCloseCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnErrorCallback(string errorMessage)
        {
            AfterShow?.Invoke();
            
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(RewardedAd)}.{nameof(OnErrorCallback)} invoked, {nameof(errorMessage)} = {errorMessage}");

            s_onErrorCallback?.Invoke(errorMessage);
        }
    }
}
