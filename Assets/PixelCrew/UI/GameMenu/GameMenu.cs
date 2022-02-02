using PixelCrew.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.PixelCrew.UI.GameMenu
{
    public class GameMenu : AnimatedWindow
    {

        private System.Action _closeAction;

        public void OnShowSettings()
        {
            var window = Resources.Load<GameObject>("UI/SettingsWindow");
            var canvas = FindObjectOfType<Canvas>();
            Instantiate(window, canvas.transform);
        }

        public void OnRetartGame()
        {
            _closeAction = () => { SceneManager.LoadScene("Level2"); };
            Close();
        }

        public void OnExit()
        {
            _closeAction = () =>
            {
                Application.Quit();

#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            };

            Close();
        }

        public override void OnCloseAnimationComplete()
        {
            base.OnCloseAnimationComplete();
            _closeAction?.Invoke();

        }
    }
}