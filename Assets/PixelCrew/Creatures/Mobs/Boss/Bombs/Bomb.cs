using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Creatures.Mobs.Boss.Bombs
{
    public class Bomb : MonoBehaviour
    {
        [SerializeField] private bool _forCannon;
        [SerializeField] private float _ttl;
        [SerializeField] private UnityEvent _onDetonate;
        private Coroutine _coroutine;

        private void Start()
        {
            if (_forCannon)
                ShootBomb();
        }

        private void ShootBomb()
        {
            var rigidbody = GetComponent<Rigidbody2D>();
            System.Random rnd = new System.Random();
            var force = 10 + (float) rnd.Next(10);
            var forceVector = new Vector2(force * - 0.85f, force * 0.15f);
            rigidbody.AddForce(forceVector, ForceMode2D.Impulse);
        }

        private void OnEnable()
        {
            TryStop();
            _coroutine = StartCoroutine(WaitAndDetonate());
        }

        private IEnumerator WaitAndDetonate()
        {
            yield return new WaitForSeconds(_ttl);
            Detonate();
        }

        public void Detonate()
        {
            _onDetonate?.Invoke();
        }

        private void OnDisable()
        {
            TryStop();
        }

        private void TryStop()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }
}