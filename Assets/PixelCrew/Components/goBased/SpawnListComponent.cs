using PixelCrew.Components.goBased;
using System;
using System.Linq;
using UnityEngine;

namespace PixelCrew.Components.goBased
{
    public class SpawnListComponent : MonoBehaviour
    {
        [SerializeField] private SpawnData[] _spawners;

        public void SpawnAll()
        {
            foreach (var spawnData in _spawners)
            {
                spawnData.Component.Spawn();
            }
        }

        public void Spawn(string id)
        {
            var spawner = _spawners.FirstOrDefault(element => element.id == id);
            spawner?.Component.Spawn();
        }

        [Serializable]
        public class SpawnData
        {
            public string id;
            public SpawnParticlesComponent Component;
        }
    }
}