using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrew.Components;


namespace PixelCrew
{

    public class Hero : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpforce;
        [SerializeField] private float _damageJumpForce;
        [SerializeField] private LayerCheck _groundCheck;
        [SerializeField] private float _interactionRadius;
        [SerializeField] private LayerMask _interactionLayer;

        [SerializeField] private SpawnParticlesComponent _footStepsParticles;
        [SerializeField] private ParticleSystem _hitParticles;

        private Rigidbody2D _rigidbody;
        private Vector2 _direction;
        private Animator _animator;
        private bool _isGrounded;
        private bool _allowDoubleJump;
        private Collider2D[] _interactionResult = new Collider2D[1];
        private float _lastYVelocity;
        private bool _allowFallDustSpawn;
        private bool _isJumping;

        public int _money;

        private static readonly int IsGroundKey = Animator.StringToHash("IsGround");
        private static readonly int IsRunningKey = Animator.StringToHash("IsRunning");
        private static readonly int VerticalVelocityKey = Animator.StringToHash("VerticalVelocity");
        private static readonly int Hit = Animator.StringToHash("Hit");


        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
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

            // Смотрим какой скорости падения достигли. Если меньше -1 - разрешаем спавнить пыль
            if (yVelocity < -15)
            {
                _allowFallDustSpawn = true;
            }
            // если все условия выполнены - спавним пыль
            if ((_lastYVelocity < 0) & (yVelocity == 0) & (_allowFallDustSpawn))
            {
                SpawnFallDust();
            }

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
                _allowFallDustSpawn = true;
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

            if (_money > 0)
            {
                SpawnCoins();
            }
        }

        private void SpawnCoins()
        {
            var numCoinsToDispose = Mathf.Min(_money, 5);
            _money -= numCoinsToDispose;

            var burst = _hitParticles.emission.GetBurst(0);
            burst.count = numCoinsToDispose;
            _hitParticles.emission.SetBurst(0, burst);
            _hitParticles.gameObject.SetActive(true);
            _hitParticles.Play();
        }

        public void SpawnFootDust ()
        {
            _footStepsParticles.Spawn(0);
        }

        public void SpawnJumpDust()
        {
            _footStepsParticles.Spawn(1);
        }

        public void SpawnFallDust()
        {
            _footStepsParticles.Spawn(2);
            _allowFallDustSpawn = false;
        }

    }
}
