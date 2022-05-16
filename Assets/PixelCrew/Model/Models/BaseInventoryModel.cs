using PixelCrew.Model.Data;
using PixelCrew.Model.Data.Properties;
using PixelCrew.Model.Definitions.Repositories.Items;
using PixelCrew.Utils.Disposables;
using System;

namespace PixelCrew.Model.Models
{
    public class BaseInventoryModel : IDisposable
    {
        private readonly PlayerData _data;
        public readonly IntProperty InterfaceSelection = new IntProperty();

        // переменная для реализации костыля. На её изменение будет попдписаны виджеты, чтобы отрисовывать/убирать галочку         
        public readonly IntProperty InterfaceUpdateTrigger = new IntProperty();


        public InventoryItemData[] BaseInventory { get; private set; }

        public event Action OnChanged;

        private int selected;

        public BaseInventoryModel(PlayerData data)
        {
            _data = data;

            BaseInventory = _data.Inventory.GetAll(ItemTag.Visible);
            _data.Inventory.OnChanged += OnChangedInventory;
        }

        public IDisposable Subscribe(Action call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
        }

        private void OnChangedInventory(string id, int value)
        {
            BaseInventory = _data.Inventory.GetAll(ItemTag.Visible);
            OnChanged?.Invoke();
        }

        public bool IsInQuickInventory(InventoryItemData item)
        {
            return item.HasQuickAccess;
        }

        public void Dispose()
        {
            _data.Inventory.OnChanged -= OnChangedInventory;
        }
    }
}