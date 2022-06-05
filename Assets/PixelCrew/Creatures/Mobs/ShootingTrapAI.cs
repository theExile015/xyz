using PixelCrew.Audio;
using PixelCrew.Components.Audio;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.goBased;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs
{
    public class ShootingTrapAI : MonoBehaviour
    {
        [SerializeField] private bool _isTotem;
        [SerializeField] public LayerCheck _vision;

        [Header("Melee")]
        [SerializeField] private Cooldown _meleeCooldown;
        [SerializeField] private CheckCircleOverlap _meleeAttack;
        [SerializeField] private LayerCheck _meleeCanAttack;

        [Header("Range")]
        [SerializeField] private Cooldown _rangeCooldown;
        [SerializeField] private SpawnComponent _rangeAttack;
        [SerializeField] private SpawnComponent _rangeAttackEffect;

        private static readonly int Melee = Animator.StringToHash("melee");
        private static readonly int Range = Animator.StringToHash("range");

        private Animator _animator;
        private bool _isAttacking;
        private float _tagTime;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (_vision.IsTouchingLayer)
            {
                if (!_isAttacking)
                {
                    _isAttacking = true;
                    _tagTime = Time.time;
                }

                if (_meleeCanAttack.IsTouchingLayer && !_isTotem)
                {
                    if (_meleeCooldown.IsReady)
                        MeleeAttack();
                    return;
                }

                if (_rangeCooldown.IsReady)
                {
                    RangeAttack();
                }
            }
            else
            {
                _isAttacking = false;
            }
        }

        private void MeleeAttack()
        {
            _meleeCooldown.Reset();
            _animator.SetTrigger(Melee);
        }

        public void RangeAttack()
        {
            _rangeCooldown.Reset();
            _animator.SetTrigger(Range);
        }


        public void OnMeleeAttack()
        {
            _meleeAttack.Check();
        }

        [ContextMenu("Fire!")]
        public void OnRangeAttack()
        {
            _rangeAttack.Spawn();
            var sound = GetComponent<PlaySoundsComponent>();
            if (sound != null)
                sound.Play("shoot");
        }

        public void OnStartShooting()
        {
            _rangeAttackEffect.Spawn();
        }
    }
}