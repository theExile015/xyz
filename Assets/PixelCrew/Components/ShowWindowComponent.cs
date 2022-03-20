using PixelCrew.UI.Windows.Perks;
using PixelCrew.Utils;
using System.Collections;
using UnityEngine;

namespace PixelCrew.Components
{
    public class ShowWindowComponent : MonoBehaviour
    {
        [SerializeField] private string _path;

        public void Show()
        {
            var window = FindObjectOfType<ManagePerksWindow>();
            if (window == null)
                WindowUtils.CreateWindow(_path); 
        }
    }
}