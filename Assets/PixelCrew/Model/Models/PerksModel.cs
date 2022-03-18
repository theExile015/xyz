using PixelCrew.Model.Data.Properties;
using PixelCrew.Model.Definitions;
using System;
using System.Collections;
using UnityEngine;

namespace PixelCrew.Model.Models
{
    public class PerksModel : IDisposable
    {
        private readonly PlayerData _data;
        public readonly StringProperty InterfaceSelection = new StringProperty();
        public PerksModel(PlayerData data)
        {
            _data = data;
        }

        public void Unlock(string id)
        {
            var def = DefsFacade.I.Perks.Get(id);
            var isEnoughResources = _data.Inventory.IsEnough(def.Price);

            if (isEnoughResources)
            {
                _data.Inventory.Remove(def.Price.ItemId, def.Price.Count);
                _data.Perks.AddPerk(id);
            }
        }

        public void UsePerk(string selected)
        {
            _data.Perks.Used.Value = selected;
        }

        public void Dispose()
        {
        }

    }
}