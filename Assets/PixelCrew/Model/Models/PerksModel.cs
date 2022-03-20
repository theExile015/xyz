﻿using PixelCrew.Model.Data.Properties;
using PixelCrew.Model.Definitions;
using PixelCrew.Utils;
using PixelCrew.Utils.Disposables;
using System;
using UnityEngine;

namespace PixelCrew.Model.Models
{
    public class PerksModel : IDisposable
    {
        private readonly PlayerData _data;
        public readonly StringProperty InterfaceSelection = new StringProperty();

        private readonly CompositeDisposable _trash = new CompositeDisposable();
        public event Action OnChanged;
        public readonly Cooldown Cooldown = new Cooldown();

        public string Used => _data.Perks.Used.Value;

        public bool IsSuperThrowSupported => _data.Perks.Used.Value == "super-throw" && Cooldown.IsReady;
        public bool IsDoubleJumpSupported => _data.Perks.Used.Value == "double-jump" && Cooldown.IsReady;
        public bool IsMagicShieldSupported => _data.Perks.Used.Value == "magic-shield";

        public PerksModel(PlayerData data)
        {
            _data = data;
            InterfaceSelection.Value = DefsFacade.I.Perks.All[0].Id;

            _trash.Retain(_data.Perks.Used.Subscribe((x, y) => OnChanged?.Invoke()));
            _trash.Retain(InterfaceSelection.Subscribe((x, y) => OnChanged?.Invoke()));
        }

        public IDisposable Subscribe(Action call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
        }

        public void Unlock(string id)
        {
            var def = DefsFacade.I.Perks.Get(id);
            var isEnoughResources = _data.Inventory.IsEnough(def.Price);

            if (isEnoughResources)
            {
                _data.Inventory.Remove(def.Price.ItemId, def.Price.Count);
                _data.Perks.AddPerk(id);

                OnChanged?.Invoke();
            }
        }

        public void UsePerk(string selected)
        {
            _data.Perks.Used.Value = selected;

            Cooldown.Value = DefsFacade.I.Perks.Get(selected).Cooldown;
            Cooldown.Reset();
        }

        public void Dispose()
        {
            _trash.Dispose();
        }

        public bool IsUsed(string perkId)
        {
            return _data.Perks.Used.Value == perkId;
        }

        public bool IsUnlocked(string perkId)
        {
            return _data.Perks.IsUnlocked(perkId);             
        }

        public bool CanBuy(string perkId)
        {
            var def = DefsFacade.I.Perks.Get(perkId);
            return _data.Inventory.IsEnough(def.Price);
        }
    }
}