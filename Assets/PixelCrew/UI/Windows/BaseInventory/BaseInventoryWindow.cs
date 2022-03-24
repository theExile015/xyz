using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Localization;
using PixelCrew.Model.Definitions.Repositories.Items;
using PixelCrew.UI.HUD.QuickInventory;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Windows.BaseInventory
{
    public class BaseInventoryWindow : AnimatedWindow
    {
        [SerializeField] private Button _accessButton;
        [SerializeField] private Text _info;
        [SerializeField] private Transform _inventoryContainer;

        private GameSession _session;
        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private float _defaultTimeScale;

        protected override void Start()
        {
            base.Start();

            _session = FindObjectOfType<GameSession>();
            _session.BaseInventory.InterfaceSelection.Value = 1;

            _trash.Retain(_session.BaseInventory.Subscribe(OnBaseInventoryChanged));
            _trash.Retain(_session.BaseInventory.InterfaceSelection.Subscribe(OnSelectionChanged));
            _trash.Retain(_accessButton.onClick.Subscribe(OnAddToQuickInventory));

            OnBaseInventoryChanged();
        }

        private void OnSelectionChanged(int oldValue, int newValue)
        {
            OnBaseInventoryChanged();
        }

        private void OnBaseInventoryChanged()
        {
            var selected = _session.BaseInventory.InterfaceSelection.Value;
            if (selected <= _session.Data.Inventory.GetAll(ItemTag.Visible).Length)
            {
                var item = _session.Data.Inventory.GetAll(ItemTag.Visible)[selected - 1];
                var def = DefsFacade.I.Items.Get(item.Id);
                _info.text = LocalizationManager.I.Localize(def.Id);
                _accessButton.interactable = def.HasTag(ItemTag.Usable);
            } 
            else
            {
                _accessButton.interactable = false;
            }
        }

        public void OnAddToQuickInventory()
        {
            var items = _session.Data.Inventory.GetAll(ItemTag.Visible);
            var totalQuickUsed = (int)0;
            foreach (var item in items)
            {
                if (item.HasQuickAccess)
                    totalQuickUsed++;
            }

            if (totalQuickUsed > 3) return;
            if ((totalQuickUsed == 3) && (!items[_session.BaseInventory.InterfaceSelection.Value - 1].HasQuickAccess)) return;

            items[_session.BaseInventory.InterfaceSelection.Value - 1].HasQuickAccess = !items[_session.BaseInventory.InterfaceSelection.Value - 1].HasQuickAccess;
            OnBaseInventoryChanged();


            // костыль, чтобы обновить все галочки в инвентаре и перестроить быстрый инвентарь. Ничего умнее придумать не смог :)
            _session.BaseInventory.InterfaceUpdateTrigger.Value++;
        }


        protected override void OnDestroy()
        {
            _trash.Dispose();
            base.OnDestroy();
        }
    }
}