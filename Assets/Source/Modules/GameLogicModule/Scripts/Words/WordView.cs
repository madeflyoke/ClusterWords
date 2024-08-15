using System;
using DG.Tweening;
using UnityEngine;

namespace Source.Modules.GameLogicModule.Scripts.Words
{
    public class WordView : MonoBehaviour
    {
        [SerializeField] private GameObject _completedWordBg;
        private Tween _tween;
        private Vector3 _defaultCompletedWordBgScale;
        
        private void Awake()
        {
            _defaultCompletedWordBgScale = _completedWordBg.transform.localScale;
        }

        public void SetViewState(bool isWordCompleted)
        {
            if (isWordCompleted)
            {
                _completedWordBg.transform.localScale = _defaultCompletedWordBgScale * 1.2f;
                _completedWordBg.SetActive(true);
                _tween?.Kill();
                _tween = _completedWordBg.transform.DOScale(_defaultCompletedWordBgScale, 0.15f).SetEase(Ease.InQuad);
            }
            else
            {
                _completedWordBg.transform.localScale = _defaultCompletedWordBgScale;
                _completedWordBg.SetActive(true);
                _tween?.Kill();
                _tween = _completedWordBg.transform.DOScale(_defaultCompletedWordBgScale *1.2f, 0.15f).SetEase(Ease.OutQuad);
            }
        }
    }
    
}
