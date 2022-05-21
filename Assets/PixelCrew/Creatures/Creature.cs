using PixelCrew.Components.Audio;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.goBased;
using UnityEngine;
using UnityEngine.Profiling;

namespace PixelCrew.Creatures
{
    public class Creature : MonoBehaviour
    {
        [Header("Params")]
        [SerializeField] private bool _invertScale;
        [SerializeField] private float _speed;
        [SerializeField] private float _hasteSpeed;
        [SerializeField] protected float Jumpforce;
        [SerializeField] private float _damageJumpForce;
        [SerializeField] private int _meleeDamage = 1;

        [Header("Checkers")]
        [SerializeField] protected LayerMask _groundLayer;
        [SerializeField] protected LayerCheck _groundCheck;
        [SerializeField] private CheckCircleOverlap _attackRange;
        [SerializeField] protected SpawnListComponent _particles;
        [SerializeField] bool _isPlayer;

        protected Rigidbody2D Rigidbody;
        protected Vector2 Direction;
        protected Animator Animator;
        protected PlaySoundsComponent Sounds;
        protected bool IsGrounded;
        protected bool IsJumping;
        protected bool IsFalling;

        public bool CreatureIsJumping => IsJumping;
        public bool CreatureIsGrounded => IsGrounded;
        public bool CreatureIsFalling => IsFalling;

        private static readonly int IsGroundKey = Animator.StringToHash("IsGround");
        private static readonly int IsRunningKey = Animator.StringToHash("IsRunning");
        private static readonly int VerticalVelocityKey = Animator.StringToHash("VerticalVelocity");
        private static readonly int Hit = Animator.StringToHash("Hit");
        private static readonly int AttackKey = Animator.StringToHash("Attack");



        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
            Sounds = GetComponent<PlaySoundsComponent>();
        }

        public void SetDirection(Vector2 direction)
        {
            if (_isPlayer)
            {
                Direction = direction;
                return;
            }
            else if (Direction.y == 0)
                Direction.x = direction.x;
        }

        protected virtual void Update()
        {
            IsGrounded = _groundCheck.IsTouchingLayer;
        }

        private void FixedUpdate()
        {
            var xVelocity = CalculateXVelocity();
            var yVelocity = CalculateYVelocity();

            Rigidbody.velocity = new Vector2(xVelocity, yVelocity);


            Animator.SetBool(IsRunningKey, Direction.x != 0);
            Animator.SetFloat(VerticalVelocityKey, Rigidbody.velocity.y);
            Animator.SetBool(IsGroundKey, IsGrounded);

            UpdateSpriteDirection(Direction);
        }

        protected virtual float CalculateXVelocity()
        {
            return Direction.x * CalculateSpeed();
        }

        protected virtual float CalculateSpeed()
        {
            return _speed;
        }

        protected virtual float CalculateYVelocity()
        {
            var yVelocity = Rigidbody.velocity.y;
            var isJumpPressing = Direction.y > 0;

            if (IsGrounded)
            {
                IsJumping = false;
            }

            if (isJumpPressing)
            {
                IsJumping = true;
                IsFalling = Rigidbody.velocity.y <= 0.5f;
                if (!IsFalling) return yVelocity;
                yVelocity = IsFalling ? CalculateJumpVelocity(yVelocity) : yVelocity;
            }
            else if (Rigidbody.velocity.y > 0 && IsJumping)
            {
                yVelocity *= 0.5f;
            }

            return yVelocity;
        }

        protected virtual float CalculateJumpVelocity(float yVelocity)
        {
            if (IsGrounded)
            {
                Rigidbody.gravityScale = 2f;
                yVelocity += Jumpforce;
                DoJumpVfx();
            }

            return yVelocity;
        }

        protected void DoJumpVfx()
        {
            _particles.Spawn("Jump");
            Sounds.Play("Jump");
        }

        public void UpdateSpriteDirection(Vector2 direction)
        {
            var multiplier = _invertScale ? -1 : 1;
            if (direction.x > 0)
            {
                transform.localScale = new Vector3(multiplier, 1, 1);
            }
            else if (direction.x < 0)
            {
                transform.localScale = new Vector3(-1 * multiplier, 1, 1);
            }
        }

        public virtual void TakeDamage()
        {
            IsJumping = false;
            Animator.SetTrigger(Hit);
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, _damageJumpForce);
        }

        public virtual void Attack()
        {
            Animator.SetTrigger(AttackKey);
            Sounds.Play("Melee");
        }

        public virtual void OnMeleeAttack()
        {
            _attackRange.Check();
        }

        public void SetMobJump(bool jump)
        {
            if (Direction.x == 0) return;
            if (jump)
            {
                Sounds.Play("Jump");
                Direction.y = 1;
            }
            else
            {
                Direction.y = 0;
            }
        }
    }
}