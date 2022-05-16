﻿using PixelCrew.Utils.Disposables;
using System.Collections;
using UnityEngine;

namespace PixelCrew.Components.Health
{
    [RequireComponent(typeof(HealthComponent))]
    public class ImmuneAfterHit : MonoBehaviour
    {
        [SerializeField] private float _immuneTime;
        private HealthComponent _health;
        private Coroutine _coroutine;
        private readonly CompositeDisposable _trash = new CompositeDisposable();


        private void Awake()
        {
            _health = GetComponent<HealthComponent>();
            _trash.Retain(_health._onDamage.Subscribe(OnDamage));
        }

        private void OnDamage()
        {
            TryStop();
            if (_immuneTime > 0)
                _coroutine = StartCoroutine(MakeImmune());
        }

        private void TryStop()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            _coroutine = null;
        }

        private IEnumerator MakeImmune()
        {
            _health.Imunne.Retain(this);
            yield return new WaitForSeconds(_immuneTime);
            _health.Imunne.Release(this);
        }

        private void OnDestroy()
        {
            TryStop();
            _trash.Dispose();
        }
    }
}