using PixelCrew.Model;
using PixelCrew.Model.Player;
using PixelCrew.UI.Widgets;
using UnityEngine;

namespace Assets.PixelCrew.UI.HUD
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField] private ProgressBarWidget _healthBar;

        private GameSession _session;

        private void Start()
        {
            _session = GameSession.Instance;
            _session.Data.HP.OnChanged += OnHealthChanged;

            OnHealthChanged(_session.Data.HP.Value, 0);
        }

        private void OnHealthChanged(int newValue, int oldValue)
        {
            var maxHealth = _session.StatsModel.GetValue(StatId.Hp);
            var value = (float)newValue / maxHealth;
            _healthBar.SetProgress(value);
        }

        private void OnDestroy()
        {
            _session.Data.HP.OnChanged -= OnHealthChanged;
        }
    }
}