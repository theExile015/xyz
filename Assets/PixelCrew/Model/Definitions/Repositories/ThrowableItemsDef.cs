using PixelCrew.Model.Definitions.Repositories.Items;
using System;
using System.Collections;
using UnityEngine;

namespace PixelCrew.Model.Definitions.Repositories
{
    [CreateAssetMenu(menuName = "Defs/Throwable", fileName = "Throwable")]
    public class ThrowableItemsDef : DefRepository<ThrowableDef>
    {
        [SerializeField] private ThrowableDef[] _items;

        public ThrowableDef Get(string id)
        {
            foreach (var itemDef in _items)
            {
                if (itemDef.Id == id)
                    return itemDef;
            }

            return default;
        }
    }

    [Serializable]
    public struct ThrowableDef : IHaveId
    {
        [InventoryId] [SerializeField] private string _id;
        [SerializeField] private GameObject _projectile;

        public string Id => _id;

        public GameObject Projectile => _projectile;
    }
}