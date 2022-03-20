using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.Utils.Disposables;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.PixelCrew.UI.Widgets
{
    public class PerkIndicatorWidget : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Image _cooldownShadow;
        [SerializeField] private Text _text;

        private readonly CompositeDisposable _trash = new CompositeDisposable();
        public event Action OnChanged;
        private GameSession _session;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();

            _trash.Retain(_session.PerksModel.Subscribe(OnPerkChanged));
        }

        private void FixedUpdate()
        {
            if (!_session.PerksModel.Cooldown.IsReady && (_session.PerksModel.Cooldown.Value != 0))
            {
                _cooldownShadow.gameObject.SetActive(true);
                _cooldownShadow.fillAmount = _session.PerksModel.Cooldown.TimeLasts / _session.PerksModel.Cooldown.Value;
            }
            else _cooldownShadow.gameObject.SetActive(false);
        }

        private void OnPerkChanged()
        {
            var selected = _session.PerksModel.Used;

            if (!string.IsNullOrEmpty(selected))
            {
                _icon.gameObject.SetActive(true);
                _icon.sprite = DefsFacade.I.Perks.Get(selected).Icon;
            }
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}