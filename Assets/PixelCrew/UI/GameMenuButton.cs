using System.Collections;
using UnityEngine;

namespace PixelCrew.UI
{
    public class GameMenuButton : MonoBehaviour
    {
        // Update is called once per frame
        public void OnGameMenuButtonClick()
        {
            var window = Resources.Load<GameObject>("UI/GameMenuWindow");
            var canvas = FindObjectOfType<Canvas>();
            Instantiate(window, canvas.transform);
        }
    }
}