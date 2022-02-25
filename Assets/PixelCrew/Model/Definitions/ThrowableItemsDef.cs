using System;
using System.Collections;
using UnityEngine;

namespace PixelCrew.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/ThrowableItems", fileName = "ThrowableItems")]
    public class ThrowableItemsDef : ScriptableObject
    {
        [SerializeField] private ThrowableDef[] _items;

        public ThrowableDef Get(string id)
        {
            foreach (var itemDef in _items)
            {
                if (itemDef.ID == id)
                    return itemDef;
            }

            return default;
        }

        [Serializable]
        public struct ThrowableDef
        {
            [InventoryId] [SerializeField] private string _id;
            [SerializeField] private GameObject _projectile;

            public string ID => _id;

            public GameObject Projectile => _projectile;
        }
    }
}