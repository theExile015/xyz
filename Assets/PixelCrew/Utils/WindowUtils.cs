using UnityEngine;

namespace PixelCrew.Utils
{
    public static class WindowUtils
    {
        public static void CreateWindow(string resourcePath)
        {
            var window = Resources.Load<GameObject>(resourcePath);
            var canvas = Object.FindObjectOfType<Canvas>();
            Object.Instantiate(window, canvas.transform); 
        }

        public static void CreateWindowSafeCall(string resourcePath)
        {
            var go = GameObject.Find("Canvas");
            var window = Resources.Load<GameObject>(resourcePath);
            var canvas = go.GetComponent<Canvas>();
            Object.Instantiate(window, canvas.transform);
        }
    }
}