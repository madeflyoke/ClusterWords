using System;
using DG.Tweening;
using Source.Modules.AudioModule.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Source.Modules.ServiceModule.Scripts.Dialogs.Visual
{
    [RequireComponent(typeof(Button))]
    public class ButtonAnimationPuncher : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Button _button;
        [SerializeField] private float _punchForce = 0.95f;
        [SerializeField] private bool _withSound = true;
        private Tween _tween;
        private Vector3 _defaultScale;
        private static AudioPlayer _audioPlayer;

        private void Awake()
        {
            _defaultScale = _button.transform.localScale;
        }

        [Inject]
        public void Construct(AudioPlayer audioPlayer)
        {
            _audioPlayer ??= audioPlayer;
        }
        
        private void OnDisable()
        {
            _tween?.Kill();
            _button.transform.localScale = _defaultScale;
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_button.interactable)
            {
                return;
            }
            
            _tween?.Kill();
            _tween = _button.transform.DOScale(_defaultScale * _punchForce, 0.1f).SetEase(Ease.Linear).SetUpdate(true);
            if (_withSound)
            {
                _audioPlayer.PlaySound(SoundType.BUTTON_CLICK_PRESS, .8f);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_button.interactable)
            {
                return;
            }
            
            _tween?.Kill();
            _tween = _button.transform.DOScale(_defaultScale, 0.1f).OnKill(()=>
            {
                _button.transform.localScale = _defaultScale;
            }).SetEase(Ease.Linear).SetUpdate(true);
        }
        
#if UNITY_EDITOR

        private void OnValidate()
        {
            if (_button==null)
            {
                _button = GetComponent<Button>();
            }
        }

#endif
    }
}
