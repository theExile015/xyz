using PixelCrew.Creatures.Weapons;
using PixelCrew.Utils;
using System;
using System.Collections;
using UnityEngine;

namespace PixelCrew.Components.goBased
{
    public class CircularProjectileSpawner : MonoBehaviour
    {
        [SerializeField] private CircularProjectileSettings[] _settings;
        public int Stage { get; set; }

        [ContextMenu("Launch!")]
        public void LaunchProjectile()
        {
            StartCoroutine(SpawnProjectiles());
        }

        private IEnumerator SpawnProjectiles()
        {
            var settings = _settings[Stage];
            var sectorStep = 2 * Mathf.PI / settings.BurstCount;
            for (int i = 0; i < settings.BurstCount; i++)
            {
                var angle = sectorStep * i;
                var direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

                var instance = SpawnUtils.Spawn(settings.Prefub.gameObject, transform.position);
                var projectile = instance.GetComponent<DirectionalProjectile>();
                projectile.Launch(direction);
                yield return new WaitForSeconds(settings.Delay);
            }
        }
    }

    [Serializable]
    public struct CircularProjectileSettings
    {
        [SerializeField] private DirectionalProjectile _prefub;
        [SerializeField] private int _burstCount;
        [SerializeField] private float _delay;

        public DirectionalProjectile Prefub => _prefub;
        public int BurstCount => _burstCount;
        public float Delay => _delay;
        
    }
}