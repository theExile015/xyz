using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.goBased;
using PixelCrew.Components.Health;
using PixelCrew.Model;
using PixelCrew.Utils;
using System.Collections;
using UnityEditor.Animations;
using UnityEngine;

namespace PixelCrew.Creatures.Hero
{

    public class Hero : Creatures
    {
        [SerializeField] private float _slamDownVelocity;

        [SerializeField] private float _interactionRadius;
        [SerializeField] private LayerMask _interactionLayer;

        [SerializeField] private Cooldown _throwCooldown;
        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _unarmed;

        [SerializeField] private CheckCircleOverlap _interactionCheck;

        [Space]
        [Header("Particles")]
        [SerializeField] private SpawnParticlesComponent _attackParticles;

        [SerializeField] private ParticleSystem _hitParticles;

        private static readonly int ThrowKey = Animator.StringToHash("throw");

        private bool _allowDoubleJump;
        private int _thrownShooted;

        private GameSession _session;

        private int CoinCount => _session.Data.Inventory.Count("Coin");
        private int SwordCount => _session.Data.Inventory.Count("Sword");
        private int LesserPotionCount => _session.Data.Inventory.Count("LesserHealingPotion");


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
            _session.Data.Inventory.OnChanged += OnInventoryChanged;
            health.SetHealth(_session.Data.HP);

            UpdateHeroWeaopn();
        }

        private void OnInventoryChanged(string id, int value)
        {
            if (id == "Sword")
                UpdateHeroWeaopn();
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override float CalculateYVelocity()
        {

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
                _allowDoubleJump = false;
                DoJumpVfx();
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
            if (CoinCount > 0)
            {
                SpawnCoins();
            }
        }

        private void SpawnCoins()
        {
            var numCoinsToDispose = Mathf.Min(CoinCount, 5);
            _session.Data.Inventory.Remove("Coin", numCoinsToDispose);

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
            if (SwordCount <= 0) return;

            base.Attack();
        }

        private void UpdateHeroWeaopn()
        {
            Animator.runtimeAnimatorController = SwordCount > 0  ? _armed : _unarmed;
        }

        public void AddInInventory(string id, int value)
        {
            _session.Data.Inventory.Add(id, value);
        }

        public void OnDoThrow()
        {
            Sounds.Play("Range");
            _particles.Spawn("Throw");
            _session.Data.thrownNumber--;
        }

        public void Throw()
        {
            if (_session.Data.thrownNumber <= 1) return;

            if (_throwCooldown.IsReady)
            {
                Animator.SetTrigger(ThrowKey);
                _throwCooldown.Reset();
            }

        }

        public void TrippleThrow()
        {
            if (_session.Data.thrownNumber <= 1) return;
            if (!_throwCooldown.IsReady) return;

            if (_session.Data.thrownNumber <= 3)
            {
                Throw();
            }
            else
            {
                _thrownShooted = 0;
                StartCoroutine(DoTrippleThrow());
            }
        }

        private IEnumerator DoTrippleThrow()
        {
            while (_thrownShooted < 3)
            {
                OnDoThrow();
                _thrownShooted++;
                yield return new WaitForSeconds(0.15f);
            }

            yield return null;
        }

        private void OnDestroy()
        {
            _session.Data.Inventory.OnChanged -= OnInventoryChanged;
        }

        public void UseHealingPotion()
        {
            if (LesserPotionCount <= 0) return;

            _session.Data.Inventory.Remove("LesserHealingPotion", 1);
            var hp = GetComponent<HealthComponent>();
            hp.ModifyHP(5);
        }

    }
}
