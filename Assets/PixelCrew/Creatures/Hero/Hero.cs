using PixelCrew.Components;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.goBased;
using PixelCrew.Components.Health;
using PixelCrew.Effects.CameraRelated;
using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Repositories.Items;
using PixelCrew.Model.Player;
using PixelCrew.Utils;
using System.Collections;
using UnityEngine;

namespace PixelCrew.Creatures.Hero
{

    public class Hero : Creature
    {
        [SerializeField] private float _slamDownVelocity;

        [SerializeField] private float _interactionRadius;
        [SerializeField] private LayerMask _interactionLayer;

        [SerializeField] private Cooldown _throwCooldown;
#if UNITY_EDITOR
        [SerializeField] private UnityEditor.Animations.AnimatorController _armed;
        [SerializeField] private UnityEditor.Animations.AnimatorController _unarmed;
#endif

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
        [SerializeField] private HeroShield _shield;
        [SerializeField] private HeroFlashLight _flashLight;

        private static readonly int ThrowKey = Animator.StringToHash("throw");

        private bool _allowDoubleJump;

        private Cooldown _hasteUpCooldown = new Cooldown();
        private float _additionalSpeed;
        private float _rangedDamage;

        private GameSession _session;
        private HealthComponent _health;
        private CameraShakeEffect _cameraShake;

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
            _cameraShake = FindObjectOfType<CameraShakeEffect>();
            _session = GameSession.Instance;
            _health = GetComponent<HealthComponent>();
            _session.Data.Inventory.OnChanged += OnInventoryChanged;
            _session.StatsModel.OnUpgraded += OnHeroUpgraded;

            _health.SetHealth(_session.Data.HP.Value);
            _rangedDamage = _session.StatsModel.GetValue(StatId.RangeDamage);

            //UpdateHeroWeaopn();
        }

        private void OnHeroUpgraded(StatId statId)
        {
            switch (statId)
            {
                case StatId.Hp:
                    var health = (int)_session.StatsModel.GetValue(statId);
                    _session.Data.HP.Value = health;
                    _health.SetHealth(health);
                    break;

                case StatId.RangeDamage:
                    var damage = (int)_session.StatsModel.GetValue(statId);
                    _rangedDamage = damage;
                    break;
            }
        }

        private void OnInventoryChanged(string id, int value)
        {
            if (id == SwordId)
            {
                // UpdateHeroWeaopn();
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

            var defaultSpeed = _session.StatsModel.GetValue(StatId.Speed);
            return defaultSpeed + _additionalSpeed;
        }

        private void SetFallSpeed()
        {
            if (_session.PerksModel.IsSlowFallSupported)
                Rigidbody.gravityScale = 0.5f;
            else
                Rigidbody.gravityScale = 2f;
        }

        protected override float CalculateJumpVelocity(float yVelocity)
        {
            if (yVelocity < 0)
                SetFallSpeed();
            else
                Rigidbody.gravityScale = 2f;

            if (!IsGrounded && _allowDoubleJump && _session.PerksModel.IsDoubleJumpSupported)
            {
                _session.PerksModel.Cooldown.Reset();
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
            //_cameraShake.Shake();
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
           // if (SwordCount <= 0) return;

            base.Attack();
        }

        private void UpdateHeroWeaopn()
        {
#if UNITY_EDITOR
            Animator.runtimeAnimatorController = SwordCount > 0 ? _armed : _unarmed;
#endif
        }

        public void AddInInventory(string id, int value)
        {
            _session.Data.Inventory.Add(id, value);
        }

        public void OnDoThrow()
        {
            if (_superThrow && _session.PerksModel.IsSuperThrowSupported)
            {
                _session.PerksModel.Cooldown.Reset();
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
            var instance = _throwSpawner.SpawnInstance();
            ApplyRangeDamageStat(instance);

            _session.Data.Inventory.Remove(throwableId, 1);
        }

        private void ApplyRangeDamageStat(GameObject projectile)
        {
            var hpModify = projectile.GetComponent<ModifyHPComponent>();
            var damageValue = (int)_session.StatsModel.GetValue(StatId.RangeDamage);
            damageValue = ModifyDamageByCrit(damageValue);
            hpModify.SetValue(-damageValue);
        }

        public int ModifyDamageByCrit(int damage)
        {
            var critChance = _session.StatsModel.GetValue(StatId.CriticalDamage);
            if (Random.value * 100 <= critChance)
            {
                return damage * 2;
            }

            return damage;
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
                    _health.SetHealth(_session.Data.HP.Value);
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

        public void UsePerk()
        {
            if (_session.PerksModel.IsMagicShieldSupported && _session.PerksModel.Cooldown.IsReady)
            {
                _shield.Use();
                _session.PerksModel.Cooldown.Reset();
            }
        }

        public void ToggleFlashLight()
        {
            Debug.Log("Inside");
            var isActive = _flashLight.gameObject.activeSelf;
            _flashLight.gameObject.SetActive(!isActive);
        }
    }
}
