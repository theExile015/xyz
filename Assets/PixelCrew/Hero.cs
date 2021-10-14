using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrew.Components;
using PixelCrew.Utils;
using UnityEditor.Animations;
using PixelCrew.Model;

namespace PixelCrew
{

    public class Hero : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpforce;
        [SerializeField] private float _damageJumpForce;
        [SerializeField] private float _slamDownVelocity;
        [SerializeField] private int _meleeDamage = 1;
        [SerializeField] private LayerCheck _groundCheck;
        [SerializeField] private float _interactionRadius;
        [SerializeField] private LayerMask _interactionLayer;

        [SerializeField] private CheckCircleOverlap _attackRange;

        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _unarmed;



        [Space][Header("Particles")]
        [SerializeField] private SpawnParticlesComponent _footStepsParticles;
        [SerializeField] private SpawnParticlesComponent _jumpParticles;
        [SerializeField] private SpawnParticlesComponent _fallParticles;
        [SerializeField] private SpawnParticlesComponent _attackParticles;

        [SerializeField] private ParticleSystem _hitParticles;

        private Rigidbody2D _rigidbody;
        private Vector2 _direction;
        private Animator _animator;
        private bool _isGrounded;
        private bool _allowDoubleJump;
        private readonly Collider2D[] _interactionResult = new Collider2D[1];
        private float _lastYVelocity;
        private bool _isJumping;

        private GameSession _session;

        private static readonly int IsGroundKey = Animator.StringToHash("IsGround");
        private static readonly int IsRunningKey = Animator.StringToHash("IsRunning");
        private static readonly int VerticalVelocityKey = Animator.StringToHash("VerticalVelocity");
        private static readonly int Hit = Animator.StringToHash("Hit");
        private static readonly int AttackKey = Animator.StringToHash("Attack");


        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
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

        private void Update()
        {
            _isGrounded = IsGrounded();
        }

        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }

        private void FixedUpdate()
        {
            var xVelocity = _direction.x * _speed;
            var yVelocity = CalculateYVelocity();

            _rigidbody.velocity = new Vector2(xVelocity, yVelocity);


            _animator.SetBool(IsRunningKey, _direction.x != 0);
            _animator.SetFloat(VerticalVelocityKey, _rigidbody.velocity.y);
            _animator.SetBool(IsGroundKey, _isGrounded);

            UpdateSpriteDirection();
        }

        private float CalculateYVelocity()
        {
            var yVelocity = _rigidbody.velocity.y;
            var isJumpPressing = _direction.y > 0;

            if (_isGrounded)
            {
                _allowDoubleJump = true;
                _isJumping = false;
            }

            if (isJumpPressing)
            {
                _isJumping = true;
                yVelocity = CalculateJumpVelocity(yVelocity);
            }
            else if (_rigidbody.velocity.y > 0 && _isJumping)
            {
                yVelocity *= 0.5f;
            }

            _lastYVelocity = yVelocity;
            return yVelocity;
        }

        private float CalculateJumpVelocity(float yVelocity)
        {
            var isFalling = _rigidbody.velocity.y <= 0.5f;
            if (!isFalling) return yVelocity;

            if (_isGrounded)
            {
                yVelocity += _jumpforce;
                SpawnJumpDust();
            }
            else if (_allowDoubleJump)
            {
                yVelocity = _jumpforce;
                SpawnJumpDust();
                _allowDoubleJump = false;
            }
            return yVelocity;
        }

        private void UpdateSpriteDirection()
        {
            if (_direction.x > 0)
            {
                transform.localScale = Vector3.one;
            }
            else if (_direction.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }


        private bool IsGrounded()
        {
            return _groundCheck.IsTouchingLayer;
        }

        public void SaySomething()
        {
            Debug.Log("Something");
        }

        public void Interact()
        {
            var size = Physics2D.OverlapCircleNonAlloc(transform.position,
                                                       _interactionRadius,
                                                       _interactionResult,
                                                       _interactionLayer);
            for (int i = 0; i < size; i++)
            {
                var interactable = _interactionResult[i].GetComponent<InteractableComponent>();
                if (interactable != null)
                {
                    interactable.Interact();
                }

            }
        }

        public void TakeDamage()
        {
            _isJumping = false;
            _animator.SetTrigger(Hit);
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _damageJumpForce);

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
            if (other.gameObject.IsInLayer(_groundCheck._groundlayer))
            {
                var contact = other.contacts[0];
                if (contact.relativeVelocity.y >= _slamDownVelocity)
                {
                    SpawnFallDust();
                }
            }
        }

        public void SpawnFootDust ()
        {
            _footStepsParticles.Spawn();
        }

        public void SpawnJumpDust()
        {
            _jumpParticles.Spawn();
        }

        public void SpawnFallDust()
        {
            _fallParticles.Spawn();
        }

        public void SpawnAttackTrace()
        {
            _attackParticles.Spawn();
        }

        public void Attack()
        {
            if (!_session.Data.IsArmed) return;

            _animator.SetTrigger(AttackKey);
        }

        public void OnMeleeAttack()
        {
            var gos = _attackRange.GetObjectsInRange(); 
            foreach (var go in gos)
            {
                var _hp = go.GetComponent<HealthComponent>();
                if (_hp != null && go.CompareTag("Enemy"))
                {
                    _hp.ModifyHP(-_meleeDamage);
                }

            }
            SpawnAttackTrace();
        }

        public void ArmHero()
        {
            _session.Data.IsArmed = true;
            _animator.runtimeAnimatorController = _armed;
            UpdateHeroWeaopn();
        }

        private void UpdateHeroWeaopn()
        {
            _animator.runtimeAnimatorController = _session.Data.IsArmed ? _armed : _unarmed;
        }

        public void AddSomeMoney(int _money)
        {
            _session.Data.Coins += _money;
            Debug.Log("Вы нашли монетку ценностью " + _money + " дублонов. Общее количество дублонов " + _session.Data.Coins);
        }

    }
}
