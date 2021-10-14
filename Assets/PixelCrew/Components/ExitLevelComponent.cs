using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PixelCrew.Model;


namespace PixelCrew.Components
{

    public class ExitLevelComponent : MonoBehaviour
    {
        [SerializeField] private string _sceneName;
        public void Exit()
        {
            var _session = FindObjectOfType<GameSession>();
            SceneManager.LoadScene(_sceneName);
        }
    }
}
