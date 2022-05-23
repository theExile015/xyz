﻿using PixelCrew.Model;
using PixelCrew.UI.LevelsLoader;
using UnityEngine;

namespace PixelCrew.Components.LevelManagement
{

    public class ExitLevelComponent : MonoBehaviour
    {
        [SerializeField] private string _sceneName;
        public void Exit()
        {
            var session = GameSession.Instance;
            session.Save();
            var loader = FindObjectOfType<LevelLoader>();
            loader.LoadLevel(_sceneName);
        }
    }
}
