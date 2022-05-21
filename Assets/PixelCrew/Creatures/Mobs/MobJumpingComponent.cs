using PixelCrew.Components.ColliderBased;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs
{
    public class MobJumpingComponent : MonoBehaviour
    {
        [SerializeField] private LayerCheck _groundEndCheck;
        //[SerializeField] private LayerCheck _jumpTargetCheck;
        private Creature _creature;

        private void Awake()
        {
            _creature = GetComponent<Creature>();
        }

        public void Update()
        {
            if (!_creature.CreatureIsGrounded) return;


            if ((!_groundEndCheck.IsTouchingLayer))
            {
                _creature.SetMobJump(true);
            }
            else
            {
                _creature.SetMobJump(false);
            }

        }
    }
}