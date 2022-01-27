using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.UI.Widgets
{

    public class CustomButton : UnityEngine.UI.Button
    {
        [SerializeField] private GameObject _normal;
        [SerializeField] private GameObject _pressed;

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);

            _normal.SetActive(state != SelectionState.Pressed);
            _pressed.SetActive(state == SelectionState.Pressed);
        }

    }

}
