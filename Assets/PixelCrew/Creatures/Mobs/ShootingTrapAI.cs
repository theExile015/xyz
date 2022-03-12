using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.goBased;
using PixelCrew.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs
{
    public class ShootingTrapAI : MonoBehaviour
    {
        [SerializeField] public LayerCheck _vision;

        [Header("Melee")]
        [SerializeField] private Cooldown _meleeCooldown;
        [SerializeField] private CheckCircleOverlap _meleeAttack;
        [SerializeField] private LayerCheck _meleeCanAttack;

        [Header("Range")]
        [SerializeField] private Cooldown _rangeCooldown;
        [SerializeField] private SpawnComponent _rangeAttack;

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

                if (_meleeCanAttack.IsTouchingLayer)
                {
                    if (_meleeCooldown.IsReady)
                        MeleeAttack();
                    return;
                }

                if (_rangeCooldown.IsReady)
                {
                    RangeAttack();
                }
            } else
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

        public void OnRangeAttack()
        {
            _rangeAttack.Spawn();
        }
    }
} 