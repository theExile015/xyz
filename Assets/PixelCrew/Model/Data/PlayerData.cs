using PixelCrew.Model.Data;
using System;
using UnityEngine;

namespace PixelCrew.Model

{   
    [Serializable]
    public class PlayerData
    {
        [SerializeField] private InventoryData _inventory;
        public int Coins;
        public int HP;
        public int thrownNumber;
        public bool IsArmed;

        public PlayerData Clone()
        {
            var json = JsonUtility.ToJson(this);
            return JsonUtility.FromJson<PlayerData>(json);
        }
    }

}