using System;
using System.Collections;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace Agava.YandexGames
{
    public static class YandexGamesSdk
    {
        private static Action s_onInitializeSuccessCallback;

        /// <summary>
        /// Enable it to log SDK callbacks in the console.
        /// </summary>
        public static bool CallbackLogging = true;

        /// <summary>
        /// SDK is initialized automatically on load.
        /// If either something fails or called way too early, this will return false.
        /// </summary>
        public static bool IsInitialized => GetYandexGamesSdkIsInitialized();

        /// <summary>
        /// Use it to check whether you're using Build and Run or running in the Editor.<br/>
        /// Can be called without initializing the SDK, can be called in Editor.
        /// </summary>
        public static bool IsRunningOnYandex
        {
            get
            {
#if UNITY_WEBGL && !UNITY_EDITOR
                return YandexGamesSdkIsRunningOnYandex();
#else
                return false;
#endif
            }
        }
        
        public static bool IsGameplayStopped { get; private set; }

        private static bool _isGameReadySet;

        [DllImport("__Internal")]
        private static extern bool GetYandexGamesSdkIsInitialized();

        public static YandexGamesEnvironment Environment
        {
            get
            {
                string environmentJson = GetYandexGamesSdkEnvironment();
                return JsonUtility.FromJson<YandexGamesEnvironment>(environmentJson);
            }
        }

        [DllImport("__Internal")]
        private static extern string GetYandexGamesSdkEnvironment();

        /// <summary>
        /// Invoke this and wait for coroutine to finish before using any SDK methods.<br/>
        /// Downloads Yandex SDK script and inserts it into the HTML page.
        /// </summary>
        /// <returns>Coroutine waiting for <see cref="IsInitialized"/> to return true.</returns>
        public static IEnumerator Initialize(Action onSuccessCallback = null)
        {
            s_onInitializeSuccessCallback = onSuccessCallback;

            YandexGamesSdkInitialize(OnInitializeSuccessCallback);

            while (!IsInitialized)
                yield return null;
        }

        [DllImport("__Internal")]
        private static extern void YandexGamesSdkInitialize(Action successCallback);

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnInitializeSuccessCallback()
        {
            if (CallbackLogging)
                Debug.Log($"{nameof(YandexGamesSdk)}.{nameof(OnInitializeSuccessCallback)} invoked");

            s_onInitializeSuccessCallback?.Invoke();
        }

        public static void GameReady()
        {
            if (_isGameReadySet)
            {
                return;
            }
            if (CallbackLogging)
                Debug.Log($"{nameof(YandexGamesSdk)}.{nameof(GameReady)} invoked");

#if !UNITY_EDITOR
            YandexGamesSdkGameReady();
#endif
            _isGameReadySet = true;
        }

        public static void GameplayStart()
        {
            if (IsGameplayStopped)
            {
                if (CallbackLogging)
                    Debug.Log($"{nameof(YandexGamesSdk)}.{nameof(GameplayStart)} invoked");
            
#if !UNITY_EDITOR
                YandexGamesSdkGameplayStart();
#endif
                IsGameplayStopped = false;
            }
        }
        
        public static void GameplayStop()
        {
            if (IsGameplayStopped)
            {
                return;
            }
            
            if (CallbackLogging)
                Debug.Log($"{nameof(YandexGamesSdk)}.{nameof(GameplayStop)} invoked");
            
#if !UNITY_EDITOR
            YandexGamesSdkGameplayStop();
#endif
            IsGameplayStopped = true;
        }

        [DllImport("__Internal")]
        private static extern void YandexGamesSdkGameReady();
        
        [DllImport("__Internal")]
        private static extern void YandexGamesSdkGameplayStart();
        
        [DllImport("__Internal")]
        private static extern void YandexGamesSdkGameplayStop();

        [DllImport("__Internal")]
        private static extern bool YandexGamesSdkIsRunningOnYandex();
    }
}
