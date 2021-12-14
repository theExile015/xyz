using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components.CutScenes
{
    public class ShowTargetComponent : MonoBehaviour
    {
        [SerializeField] private Transform _traget;
        [SerializeField] CameraStateController _controller;
        [SerializeField] private float _delay = 0.5f;


        private void OnValidate()
        {
            if (_controller == null)
                _controller = FindObjectOfType<CameraStateController>();
        }

        public void ShowTarget()
        {
            _controller.SetPosition(_traget.position);
            _controller.SetState(true);
            Invoke(nameof(MoveBack), _delay);
        }

        private void MoveBack()
        {
            _controller.SetState(false);
        }
    }

}
