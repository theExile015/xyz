using PixelCrew.Model;
using PixelCrew.UI;
using PixelCrew.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.PixelCrew.UI.GameMenu
{
    public class GameMenu : AnimatedWindow
    {
        private System.Action _closeAction;
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

        private void OnDestroy()
        {
            Time.timeScale = _defaultTimeScale;
        }
    }
}