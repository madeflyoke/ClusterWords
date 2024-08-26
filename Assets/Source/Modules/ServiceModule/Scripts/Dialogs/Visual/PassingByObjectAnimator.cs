using System;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Source.Modules.ServiceModule.Scripts.Dialogs.Visual
{
    public class PassingByObjectAnimator : MonoBehaviour
    {
        [SerializeField] private float _intervalSec;
        [SerializeField] private Transform _originPoint;
        [SerializeField] private List<Transform> _finalDestinations;
    
        [SerializeField] private float _durationSec =10f;
        private Sequence _seq;

        private void Start()
        {
            Observable.Interval(TimeSpan.FromSeconds(_intervalSec)).Subscribe(x =>
            {
                Animate();
            }).AddTo(this);
        }

        private void Animate()
        {
            transform.position = _originPoint.position;
            transform.rotation = Quaternion.identity;

            var finalPoint = _finalDestinations[Random.Range(0, _finalDestinations.Count)].position;
            var finalRotation = transform.localScale +
                                Random.Range(Random.value > 0.5f ? 20f : -40f, Random.value > 0.5f ? 40f : -20f) *
                                Vector3.forward;
            _seq?.Kill();
            _seq = DOTween.Sequence();
            _seq.Append(transform.DOMove(finalPoint, _durationSec)
                    .SetEase(Ease.Linear))
                .Join(transform.DOLocalRotate(finalRotation, _durationSec).SetEase(Ease.Linear));
        }
    }
}
