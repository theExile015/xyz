using PixelCrew.UI.HUD.Dialogs;
using System.Collections;
using UnityEngine;

namespace PixelCrew.Components.Dialogs
{
    public class ShowOptionsComponent : MonoBehaviour
    {
        [SerializeField] private OptionDialogData _data;
        private OptionsDialogController _dialogBox;

        public void Show()
        {
            if (_dialogBox == null)
                _dialogBox = FindObjectOfType<OptionsDialogController>();

            _dialogBox.Show(_data);
        }
    }
}