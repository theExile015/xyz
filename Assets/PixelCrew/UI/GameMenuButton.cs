using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.UI
{
    public class GameMenuButton : MonoBehaviour
    {
        // Update is called once per frame
        public void OnGameMenuButtonClick(string _path)
        {
            WindowUtils.CreateWindow(_path);
        }
    }
}