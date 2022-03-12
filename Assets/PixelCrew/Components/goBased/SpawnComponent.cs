using PixelCrew.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components.goBased
{
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform _spawnTarget;
        [SerializeField] private GameObject _particlesPrefub;

        [ContextMenu("Spawn")]

        public void Spawn()
        {
            var instance = SpawnUtils.Spawn(_particlesPrefub, _spawnTarget.position);

            var scale = _spawnTarget.lossyScale;
            instance.transform.localScale = scale;
            instance.SetActive(true);
        }

        internal void SetPrefub(GameObject prefub)
        {
            _particlesPrefub = prefub;
        }
    }
}

