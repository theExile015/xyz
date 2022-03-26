using PixelCrew.Model;
using PixelCrew.Utils;
using PixelCrew.Utils.Disposables;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace PixelCrew.Components
{
    public class LampComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _lamp;
        [SerializeField] private Light2D _light;
        [SerializeField] private float _maxFuel;
        [SerializeField] private float _defaultIntensity;

        private Cooldown _lampTimer = new Cooldown();

        private GameSession _session;
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private bool _pause;
        private float _pauseTime;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _trash.Retain(_session.Data.Fuel.Subscribe(OnFuelChange));
        }

        public void SetPause()
        {
            if (_session.Data.Fuel.Value == 0) return;

            if (!_pause)
            {
                _pause = true;
                _pauseTime = _lampTimer.TimeLasts;
                _lamp.SetActive(false);
            }
            else
            {
                _pause = false;
                _session.Data.Fuel.Value = _pauseTime;
            }
        }

        private void OnFuelChange(float newValue, float oldValue)
        {
            if (newValue == 0) return;

            var fuel = newValue;
            if (newValue > _maxFuel)
                fuel = (int) _maxFuel;
            _lampTimer.Value = fuel;

            _lampTimer.Reset();
            _light.intensity = _defaultIntensity;
            _lamp.SetActive(true);
        }

        // Update is called once per frame
        private void Update()
        {
            if (_lampTimer.TimeLasts < 10)
            {
                _light.intensity = _lampTimer.TimeLasts / 10 * _defaultIntensity;
            }

            if (_lampTimer.IsReady)
            {
                _lamp.SetActive(false);
                _session.Data.Fuel.Value = 0f;
            }
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}