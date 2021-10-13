using System;
using UnityEngine;

namespace PixelCrew.Components
{

    public class LootSpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform _spawnTarget;
        [SerializeField] private LootTableItem[] _lootTable;

        public void SpawnLoot()
        {
            var _currentSum = 0;
            System.Random rnd = new System.Random();
            int lootRandom = rnd.Next(0, 100);

            foreach (var lootItem in _lootTable)
            {
                _currentSum += lootItem.DropChance;
                if (_currentSum > 100) return;

                if (lootRandom < _currentSum)
                {
                    var instantiate = Instantiate(lootItem._prefub, _spawnTarget.position, Quaternion.identity);
                    instantiate.transform.localScale = _spawnTarget.lossyScale;
                    return;
                }
            }
        }

        [Serializable]
        public class LootTableItem
        {
            [SerializeField] private string _name;
            [SerializeField] private int _dropChance;
            [SerializeField] public GameObject _prefub;

            public string Name => _name;
            public int DropChance => _dropChance;
        }
    }
}
