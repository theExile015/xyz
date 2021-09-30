using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{

    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _maxHealth;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onDie;
        private int _health;

        private void Awake()
        {
            _health = _maxHealth;    
        }

        public void ModifyHP(int hpValue)
        {
            _health += hpValue;
                        
            if (hpValue < 0)
            {
                _onDamage?.Invoke();
            }

            if (_health <= 0)
            {
                _onDie?.Invoke();
            } else if (_health > _maxHealth)
            {
                _health = _maxHealth;
            }

            Debug.Log("Ouch!! HP = " + _health);
        }
    }

}

