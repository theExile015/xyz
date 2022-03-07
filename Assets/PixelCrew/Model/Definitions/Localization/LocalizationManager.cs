using PixelCrew.Model.Data.Properties;
using PixelCrew.UI.Windows.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Model.Definitions.Localization
{
    public class LocalizationManager
    {
        public readonly static LocalizationManager I;

        private StringPresistentProperty _localeKey = new StringPresistentProperty("en", "localization/current");
        private Dictionary<string, string> _localization;
        public string LocaleKey => _localeKey.Value;

        public event Action OnLocaleChanged;

        static LocalizationManager()
        {
            I = new LocalizationManager();
        }

        public LocalizationManager()
        {
            LoadLocale(_localeKey.Value);
        }

        private void LoadLocale(string localeToLoad)
        {
            var def = Resources.Load<LocaleDef>($"Locales/{localeToLoad}");
            _localization = def.GetData();
            _localeKey.Value = localeToLoad;  
            OnLocaleChanged?.Invoke();
        }

        public string Localize(string key)
        {
            return _localization.TryGetValue(key, out var value) ? value : $"%%%{key}%%%";
        }

        internal void SetLocale(string localeKey)
        {
            LoadLocale(localeKey);
        }
    }
}