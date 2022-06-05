using PixelCrew.Model;
using PixelCrew.UI.Windows;
using PixelCrew.Utils;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.PixelCrew.UI.GameMenu.Windows
{
    public class GameMenu : AnimatedWindow
    {
        private float _defaultTimeScale;

        protected override void Start()
        {
            base.Start();

            _defaultTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }

        public void OnShowSettings()
        {
            WindowUtils.CreateWindow("UI/SettingsWindow");
        }

        public void OnExit()
        {
            SceneManager.LoadScene("MainMenu");

            var session = FindObjectOfType<GameSession>();
            Destroy(session.gameObject);
        }

        private void OnDestroy()
        {
            Time.timeScale = _defaultTimeScale;
        }
    }
}