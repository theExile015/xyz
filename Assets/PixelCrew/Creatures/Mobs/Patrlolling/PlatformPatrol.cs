using PixelCrew.Components.ColliderBased;
using PixelCrew.Utils;
using System.Collections;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs.Patrlolling
{
    public class PlatformPatrol : Patrol
    {
        [SerializeField] private LayerCheck _layerCheck;
        [SerializeField] private bool _invertPatrolTrigger;
        private Creatures _creature;
        private bool _leftDirection;
        [SerializeField] private Cooldown _cooldown;


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
                if ((_layerCheck.IsTouchingLayer == _invertPatrolTrigger))
                {
                    ChangeDirection();
                    
                    yield return new WaitForSeconds(0.1f);
                }
                yield return null;
            }
        }

        private void ChangeDirection()
        {
            if (!_cooldown.IsReady) return;
            _cooldown.Reset();

            _leftDirection = !_leftDirection;
           UpdateDirection();
        }

        private void UpdateDirection()
        {
            if (_creature.CreatureIsJumping) return;
            
            var multiplier = _leftDirection ? -1 : 1;
            var direction = new Vector3(multiplier * 1.5f, 0, 0);
            _creature.SetDirection(direction.normalized);
        }

    }
}