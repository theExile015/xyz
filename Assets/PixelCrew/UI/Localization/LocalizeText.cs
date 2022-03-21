using PixelCrew.Model.Definitions.Localization;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Localization
{
    [RequireComponent(typeof(Text))]
    public class LocalizeText : AbsractLocalizeComponent
    {
        [SerializeField] private string _key;
        [SerializeField] private bool _capitalize;

        private Text _text;

        protected override void Awake()
        {
            _text = GetComponent<Text>();
            base.Awake();
        }

        protected override void Localize()
        {
            var localized = LocalizationManager.I.Localize(_key);
            _text.text = _capitalize ? localized.ToUpper() : localized;
        }
    }
}