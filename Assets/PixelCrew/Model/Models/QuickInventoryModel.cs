using PixelCrew.Model.Data;
using PixelCrew.Model.Data.Properties;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Repositories.Items;
using PixelCrew.Utils.Disposables;
using System;
using UnityEngine;

namespace PixelCrew.Model.Models
{
    public class QuickInventoryModel : IDisposable
    {
        private readonly PlayerData _data;

        public InventoryItemData[] QuickInventory { get; private set; }

        public readonly IntProperty SelectedIndex = new IntProperty();

        public event Action OnChanged;

        public InventoryItemData SelectedItem
        {
            get
            {
                if (QuickInventory.Length > 0 && QuickInventory.Length > SelectedIndex.Value)
                    return QuickInventory[SelectedIndex.Value];

                return null;
            }
        }

        public ItemDef SelectedDef => DefsFacade.I.Items.Get(SelectedItem?.Id);

        public QuickInventoryModel(PlayerData data)
        {
            _data = data;

            QuickInventory = _data.Inventory.GetQuickAccessItems();
            _data.Inventory.OnChanged += OnChangedInventory;
        }

        public IDisposable Subscribe(Action call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
        }

        public void OnForcedRebuild()
        {
            QuickInventory = _data.Inventory.GetQuickAccessItems();
            SelectedIndex.Value = Mathf.Clamp(SelectedIndex.Value, 0, QuickInventory.Length - 1);
            OnChanged?.Invoke();
        }

        private void OnChangedInventory(string id, int value)
        {
            QuickInventory = _data.Inventory.GetQuickAccessItems();
            SelectedIndex.Value = Mathf.Clamp(SelectedIndex.Value, 0, QuickInventory.Length - 1);
            OnChanged?.Invoke();
        }

        public void SetNextItem()
        {
            SelectedIndex.Value = (int)Mathf.Repeat(SelectedIndex.Value + 1, QuickInventory.Length);
        }

        public void Dispose()
        {
            _data.Inventory.OnChanged -= OnChangedInventory;
        }
    }
}