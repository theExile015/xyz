using System;
using System.Linq;
using UnityEngine;

namespace PixelCrew.Model.Definitions.Repositories.Items
{
    [CreateAssetMenu(menuName = "Defs/Items", fileName = "Items")]
    public class ItemsRepository : DefRepository<ItemDef>
    {
        [SerializeField] private ItemDef[] _items;

        private void OnEnable()
        {
            
        }

        public ItemDef Get(string id)
        {
            foreach (var itemDef in _items)
            {
                if (itemDef.Id == id)
                    return itemDef;
            }

            return default;
        }
#if UNITY_EDITOR
        public ItemDef[] ItemsForEditor => _items;
#endif

    }

    [Serializable]
    public struct ItemDef : IHaveId
    {
        [SerializeField] private string _id;
        [SerializeField] private Sprite _icon;
        [SerializeField] private ItemTag[] _tags;
        public string Id => _id;

        public bool IsVoid => string.IsNullOrEmpty(_id);

        public Sprite Icon => _icon;

        public bool HashTag(ItemTag tag)
        {
            return _tags.Contains(tag);
        }
    }

}
