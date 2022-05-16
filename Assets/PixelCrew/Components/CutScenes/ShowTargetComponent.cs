using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.CutScenes
{
    public class ShowTargetComponent : MonoBehaviour
    {
        [SerializeField] private Transform _traget;
        [SerializeField] private float _delay = 0.5f;
        [SerializeField] private UnityEvent _onDelay;

        [SerializeField] private ShowTargetController _controller;

        private Coroutine _coroutine;

        private void OnValidate()
        {
            if (_controller == null)
                _controller = FindObjectOfType<ShowTargetController>();
        }

        public void ShowTarget()
        {
            _controller.SetPosition(_traget.position);
            _controller.SetState(true);

            if (_coroutine != null)
                StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(WaitAndReturn());
        }

        private IEnumerator WaitAndReturn()
        {
            yield return new WaitForSeconds(_delay);

            _onDelay?.Invoke();
            _controller.SetState(false);
        }
    }

}
