﻿using PixelCrew.Model.Data;
using PixelCrew.UI.Widgets;
using PixelCrew.UI.Windows;
using UnityEngine;

namespace PixelCrew.UI.Settings.Windows
{
    public class SettingsWindow : AnimatedWindow
    {
        [SerializeField] private AudioSettingsWidget _music;
        [SerializeField] private AudioSettingsWidget _sfx;

        protected override void Start()
        {
            base.Start();

            _music.SetModel(GameSettings.I.Music);
            _sfx.SetModel(GameSettings.I.Sfx);
        }
    }
}