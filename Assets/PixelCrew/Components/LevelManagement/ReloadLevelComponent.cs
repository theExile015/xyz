﻿using UnityEngine;
using UnityEngine.SceneManagement;
using PixelCrew.Model;

namespace PixelCrew.Components.LevelManagement
{

    public class ReloadLevelComponent : MonoBehaviour
    {
        [ContextMenu("Reload")]
        public void Reload()
        {
            var session = FindObjectOfType<GameSession>();
            session.LoadLastSave();


            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }

}