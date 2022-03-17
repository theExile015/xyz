﻿using PixelCrew.Model.Definitions.Repositories.Items;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.goBased;
using PixelCrew.Components.Health;
using PixelCrew.Model;
using PixelCrew.Model.Definitions;
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

        [Header("Super Throw")]
        [SerializeField] private Cooldown _superThrowCooldown;
        [SerializeField] private int _superThrowParticles = 3;
        [SerializeField] private float _superThrowDelay = 0.2f;
        private bool _superThrow;

        [Space]
        [Header("Particles")]
        [SerializeField] private SpawnComponent _attackParticles;

        [SerializeField] private ProbabilityDropComponent _hitDrop;
        [SerializeField] private SpawnComponent _throwSpawner;

        private static readonly int ThrowKey = Animator.StringToHash("throw");

        private bool _allowDoubleJump;

        private Cooldown _hasteUpCooldown = new Cooldown();
        private float _additionalSpeed;

        private GameSession _session;

        private const string SwordId = "Sword";
        private int CoinCount => _session.Data.Inventory.Count("Coin");
        private int SwordCount => _session.Data.Inventory.Count("Sword");
        private string SelectedItemId => _session.QuickInventory.SelectedItem.Id;
        private bool CanThrow
        {
            get
            {
                if (SelectedItemId == SwordId)
                    return SwordCount > 1;

                var def = DefsFacade.I.Items.Get(SelectedItemId);
                return def.HasTag(ItemTag.Throwable);
            }
        }

        protected override void Awake()
        {
            base.Awake();
        }

        public void OnHealthChanged(int _currentHealth)
        {
            _session.Data.HP.Value = _currentHealth;
        }

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            var health = GetComponent<HealthComponent>();
            _session.Data.Inventory.OnChanged += OnInventoryChanged;
            health.SetHealth(_session.Data.HP.Value);

            UpdateHeroWeaopn();
        }

        private void OnInventoryChanged(string id, int value)
        {
            if (id == SwordId)
            {
                UpdateHeroWeaopn();
            }
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

        protected override float CalculateSpeed()
        {
            if (_hasteUpCooldown.IsReady)
                _additionalSpeed = 0f;

            return base.CalculateSpeed() + _additionalSpeed;
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

            _hitDrop.SetCount(numCoinsToDispose);
            _hitDrop.CalculateDrop();
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
            if (_superThrow)
            {
                var throwableCount = _session.Data.Inventory.Count(SelectedItemId);
                var possibleCount = SelectedItemId == SwordId ? throwableCount - 1 : throwableCount;
                var numThrows = Mathf.Min(_superThrowParticles, possibleCount);
                StartCoroutine(DoSuperThrow(numThrows));
            }
            else
            {
                ThrowAndRemoveFromInventory();
            }
            _superThrow = false;
        }

        private void ThrowAndRemoveFromInventory()
        {
            Sounds.Play("Range");
            
            var throwableId = _session.QuickInventory.SelectedItem.Id;
            var throwableDef = DefsFacade.I.Throable.Get(throwableId);
            _throwSpawner.SetPrefub(throwableDef.Projectile);
            _throwSpawner.Spawn();

            _session.Data.Inventory.Remove(throwableId, 1);
        }


        private IEnumerator DoSuperThrow(int numThrows)
        {
            for (int i = 0; i < numThrows; i++)
            {
                ThrowAndRemoveFromInventory();
                yield return new WaitForSeconds(_superThrowDelay);
            }
        }

        private void OnDestroy()
        {
            _session.Data.Inventory.OnChanged -= OnInventoryChanged;
        }

        public void UsePotion()
        {
            var potion = DefsFacade.I.Potions.Get(SelectedItemId);

            switch (potion.Effect)
            {
                case Effect.AddHp:
                    _session.Data.HP.Value += (int)potion.Value;
                    break;

                case Effect.SpeedUp:
                    _hasteUpCooldown.Value = _hasteUpCooldown.TimeLasts + potion.Time;
                    _additionalSpeed = potion.Value;
                    _hasteUpCooldown.Reset();
                    break;
            }

            _session.Data.Inventory.Remove(potion.Id, 1);
        }


        public void StartThrowing()
        {
            _superThrowCooldown.Reset();
        }

        public void UseInventory()
        {
            if (IsSelectedItem(ItemTag.Throwable))
                PerfomThrowing();
            else if (IsSelectedItem(ItemTag.Potion))
                UsePotion();
        }

        private bool IsSelectedItem(ItemTag tag)
        {
            return _session.QuickInventory.SelectedDef.HasTag(tag); 
        }

        private void PerfomThrowing()
        {
            if (!_throwCooldown.IsReady || !CanThrow) return;

            if (_superThrowCooldown.IsReady) _superThrow = true;

            Animator.SetTrigger(ThrowKey);
            _throwCooldown.Reset();
        }

        public void NextItem()
        {
            _session.QuickInventory.SetNextItem();
        }

    }
}
