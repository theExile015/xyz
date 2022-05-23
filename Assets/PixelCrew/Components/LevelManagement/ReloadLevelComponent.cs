﻿using PixelCrew.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Components.LevelManagement
{

    public class ReloadLevelComponent : MonoBehaviour
    {
        [ContextMenu("Reload")]
        public void Reload()
        {
            var session = GameSession.Instance;
            session.LoadLastSave();


            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }

}
