using PixelCrew.Model;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace PixelCrew.Components
{
    public class HeroFlashLight : MonoBehaviour
    {
        [SerializeField] private float _consumePerSecond;
        [SerializeField] private Light2D _light;
        private GameSession _session;
        private float _defaultIntensity = 1f;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            if (!_light) return;
            _defaultIntensity = _light.intensity;
        }

        private void Update()
        {
            var consumed = Time.deltaTime * _consumePerSecond;
            var currentValue = _session.Data.Fuel.Value;
            var nextValue = currentValue - consumed;
            nextValue = Mathf.Max(nextValue, 0);
            _session.Data.Fuel.Value = nextValue;

            if (!_light) return;
            var progress = Mathf.Clamp(nextValue / 20, 0, 1);
            _light.intensity = _defaultIntensity * progress;

            if (nextValue == 0)
                this.gameObject.SetActive(false);

        }
    }
}