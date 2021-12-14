using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components.goBased
{
    public class SpawnParticlesComponent : MonoBehaviour
    {
        [SerializeField] private Transform _spawnTarget;
        [SerializeField] private GameObject _particlesPrefub;

        [ContextMenu("Spawn")]

        public void Spawn()
        {
            var instance = Instantiate(_particlesPrefub, _spawnTarget.position, Quaternion.identity);
            var scale = _spawnTarget.lossyScale;
            instance.transform.localScale = scale;
            instance.SetActive(true);
        }
    }
}

