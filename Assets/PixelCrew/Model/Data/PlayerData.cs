using PixelCrew.Model.Data;
using PixelCrew.Model.Data.Properties;
using System;
using UnityEngine;

namespace PixelCrew.Model

{   
    [Serializable]
    public class PlayerData
    {
        [SerializeField] private InventoryData _inventory;
        public IntProperty HP = new IntProperty();

        public InventoryData Inventory => _inventory;

        public PlayerData Clone()
        {
            var json = JsonUtility.ToJson(this);
            return JsonUtility.FromJson<PlayerData>(json);
        }
    }

}