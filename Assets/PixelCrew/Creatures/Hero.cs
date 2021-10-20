using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrew.Components;
using PixelCrew.Utils;
using UnityEditor.Animations;
using PixelCrew.Model;

namespace PixelCrew.Creatures
{

    public class Hero : Creatures
    {
        [SerializeField] private float _slamDownVelocity;
         
        [SerializeField] private float _interactionRadius;
        [SerializeField] private LayerMask _interactionLayer;


        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _unarmed;

        [SerializeField] private CheckCircleOverlap _interactionCheck;

        [Space][Header("Particles")]
        [SerializeField] private SpawnParticlesComponent _attackParticles;

        [SerializeField] private ParticleSystem _hitParticles;

        private bool _allowDoubleJump;

        private GameSession _session;

 
        protected override void Awake()
        {
            base.Awake();
        }

        public void OnHealthChanged(int _currentHealth)
        {
            _session.Data.HP = _currentHealth;
        }

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            var health = GetComponent<HealthComponent>();
            health.SetHealth(_session.Data.HP);

            UpdateHeroWeaopn();
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override float CalculateYVelocity()
        {
            var yVelocity = Rigidbody.velocity.y;
            var isJumpPressing = Direction.y > 0;

            if (IsGrounded)
            {
                _allowDoubleJump = true;
            }

            return base.CalculateYVelocity();
        }

        protected override float CalculateJumpVelocity(float yVelocity)
        {
            if (!IsGrounded && _allowDoubleJump)
            {
                _particles.Spawn("Jump");
                _allowDoubleJump = false;
                return Jumpforce;
            }
            return base.CalculateJumpVelocity(yVelocity); 
        }

        public void Interact()
        {
            _interactionCheck.Check();
        }

        public override void TakeDamage()
        {
            base.TakeDamage();

            if (_session.Data.Coins > 0)
            {
                SpawnCoins();
            }
        }

        private void SpawnCoins()
        {
            var numCoinsToDispose = Mathf.Min(_session.Data.Coins, 5);
            _session.Data.Coins -= numCoinsToDispose;

            var burst = _hitParticles.emission.GetBurst(0);
            burst.count = numCoinsToDispose;
            _hitParticles.emission.SetBurst(0, burst);
            _hitParticles.gameObject.SetActive(true);
            _hitParticles.Play();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.IsInLayer(_groundLayer))
            {
                var contact = other.contacts[0];
                if (contact.relativeVelocity.y >= _slamDownVelocity)
                {
                    _particles.Spawn("SlamDown");
                }
            }
        }

        public override void Attack()
        {
            if (!_session.Data.IsArmed) return;

            base.Attack();
        }

      //  public override void OnMeleeAttack()
      //  {
      //      Debug.Log("OnMeleeAttack");
      //      Attack();
      //      _particles.Spawn("Attack");
      //  }

        public void ArmHero()
        {
            _session.Data.IsArmed = true;
            Animator.runtimeAnimatorController = _armed;
            UpdateHeroWeaopn();
        }

        private void UpdateHeroWeaopn()
        {
            Animator.runtimeAnimatorController = _session.Data.IsArmed ? _armed : _unarmed;
        }

        public void AddSomeMoney(int _money)
        {
            _session.Data.Coins += _money;
            Debug.Log("Вы нашли монетку ценностью " + _money + " дублонов. Общее количество дублонов " + _session.Data.Coins);
        }

    }
}
