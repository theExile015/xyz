using PixelCrew.Model;
using PixelCrew.Utils;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.Health
{

    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] public UnityEvent _onDamage;
        [SerializeField] public UnityEvent _onDie;
        [SerializeField] private UnityEvent _onHeal;
        [SerializeField] public HealthChangeEvent _onChange;

        private Lock _imunne = new Lock();
        private GameSession _session;
        private int _maxHealth;

        public int Health => _health;

        public Lock Imunne => _imunne;


        private void Awake()
        {
            _maxHealth = _health;
        }

        private void Start()
        {
            _session = GameSession.Instance;
        }

        public void ModifyHP(int hpValue)
        {
            if (hpValue < 0 && Imunne.IsLocked) return;

            if (_health <= 0) return;

            _health += hpValue;

            _onChange?.Invoke(_health);

            if (hpValue < 0)
            {
                _onDamage?.Invoke();
            }

            if (_health <= 0)
            {
                _onDie?.Invoke();
            }

            if (hpValue > 0)
            {
                _onHeal?.Invoke();

                if (_health > _maxHealth)
                {
                    _health = _maxHealth;
                }
            }

            Debug.Log("Ouch!! HP = " + _health);
        }

        private void OnDestroy()
        {
            _onDie.RemoveAllListeners();
        }

        [Serializable]
        public class HealthChangeEvent : UnityEvent<int>
        {

        }

        public void SetHealth(int health)
        {
            _health = health;
        }

    }

}

