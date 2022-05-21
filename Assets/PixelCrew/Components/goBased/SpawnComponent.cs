using PixelCrew.Utils;
using PixelCrew.Utils.ObjectPool;
using UnityEngine;

namespace PixelCrew.Components.goBased
{
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform _spawnTarget;
        [SerializeField] private GameObject _particlesPrefub;
        [SerializeField] private bool _usePool;

        [ContextMenu("Spawn")]

        public void Spawn()
        {
            SpawnInstance();
        }

        public GameObject SpawnInstance()
        {
            var targetPosition = _spawnTarget.position;

            var instance = _usePool 
                ? Pool.Instance.Get(_particlesPrefub, targetPosition)
                : SpawnUtils.Spawn(_particlesPrefub, targetPosition);

            var scale = _spawnTarget.lossyScale;
            instance.transform.localScale = scale;
            instance.SetActive(true);
            return instance;
        }

        public void SetPrefub(GameObject prefub)
        {
            _particlesPrefub = prefub;
        }
    }
}

