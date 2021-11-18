using System.Collections;
using UnityEngine;

namespace PixelCrew.Creatures
{
    public class PlatformPatrol : Patrol
    {
        [SerializeField] private LayerCheck _layerCheck;
        private Creatures _creature;
        private bool _leftDirection;
        

        private void Awake()
        {
            _creature = GetComponent<Creatures>();
            UpdateDirection();
        }

        public override IEnumerator DoPatrol()
        {
            UpdateDirection();
            while (enabled)
            {
                if (!_layerCheck.IsTouchingLayer)
                {
                    _leftDirection = !_leftDirection;
                    UpdateDirection();
                    yield return new WaitForSeconds(0.1f);
                }
                yield return null;
            }
        }

        private void UpdateDirection()
        {
            var multiplier = _leftDirection ? -1 : 1;
            var direction = new Vector3(multiplier * 1.5f, 0, 0);
            _creature.SetDirection(direction.normalized);
        }

    }
}