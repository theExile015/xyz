using PixelCrew.Utils;
using System.Collections;
using UnityEngine;

namespace PixelCrew.UI
{
    public class GameMenuButton : MonoBehaviour
    {
        // Update is called once per frame
        public void OnGameMenuButtonClick(string _path)
        {
            WindowUtils.CreateWindowSafeCall(_path);
        }
    }
}