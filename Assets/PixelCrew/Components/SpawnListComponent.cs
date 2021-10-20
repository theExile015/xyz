using System;
using System.Linq;
using UnityEngine;

namespace PixelCrew.Components
{
    public class SpawnListComponent : MonoBehaviour
    {
        [SerializeField] private SpawnData[] _spawners;

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