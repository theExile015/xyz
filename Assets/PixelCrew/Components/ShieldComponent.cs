using PixelCrew.Components.Health;
using PixelCrew.Utils;
using System.Collections;
using UnityEngine;

namespace PixelCrew.Components
{
    public class ShieldComponent : MonoBehaviour
    {
        [SerializeField] private HealthComponent _health;
        [SerializeField] private Cooldown _cooldown;

        public void Use()
        {
            _health.Imunne = true;
            _cooldown.Reset();
            gameObject.SetActive(true);
        }

        public void Update()
        {
            if (_cooldown.IsReady)
                gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            _health.Imunne = false;
        }
    }
}