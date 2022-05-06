using UnityEngine;
using System;
using UnityEngine.Events;
using PixelCrew.Model;

namespace PixelCrew.Components.Health
{

    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] public UnityEvent _onDie;
        [SerializeField] private UnityEvent _onHeal;
        [SerializeField] public HealthChangeEvent _onChange;
        [SerializeField] private bool _imunne;

        private GameSession _session;
        private int _maxHealth;

        public int Health => _health;

        public bool Imunne
        {
            get => _imunne;
            set => _imunne = value;
        }

        private void Awake()
        {
            _maxHealth = _health;            
        }

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();    
        }

        public void ModifyHP(int hpValue)
        {
            if (hpValue < 0 && Imunne) return;

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

