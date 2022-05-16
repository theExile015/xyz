using PixelCrew.Model;
using PixelCrew.UI.Windows;
using PixelCrew.Utils;
using System;
using UnityEngine.SceneManagement;

namespace Assets.PixelCrew.UI.GameMenu.Windows
{
    public class GameMenu : AnimatedWindow
    {
        private Action _closeAction;

        protected override void Start()
        {
            base.Start();

        }
        public void OnShowSettings()
        {
            WindowUtils.CreateWindow("UI/SettingsWindow");
        }

        public void OnExitGame()
        {
            SceneManager.LoadScene("Main menu");

            var session = FindObjectOfType<GameSession>();
            Destroy(session.gameObject);
        }

        public void OnExitMenu()
        {
            this.Close();
        }

        public override void OnCloseAnimationComplete()
        {
            base.OnCloseAnimationComplete();
            _closeAction?.Invoke();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}