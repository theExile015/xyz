using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Widgets
{
    public class ProgressBarWidget : MonoBehaviour
    {
        [SerializeField] private Image _image;

        public void SetProgress(float progress)
        {
            _image.fillAmount = progress;
        }
    }
}