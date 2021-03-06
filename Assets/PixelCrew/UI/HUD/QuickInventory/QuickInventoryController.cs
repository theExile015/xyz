using PixelCrew.Model;
using PixelCrew.Model.Data;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils.Disposables;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.UI.HUD.QuickInventory
{
    public class QuickInventoryController : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private InventoryItemWidget _prefub;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private GameSession _session;
        private List<InventoryItemWidget> _createdItem = new List<InventoryItemWidget>();

        private DataGroup<InventoryItemData, InventoryItemWidget> _dataGroup;

        private void Start()
        {
            _dataGroup = new DataGroup<InventoryItemData, InventoryItemWidget>(_prefub, _container);
            _session = GameSession.Instance;
            _trash.Retain(_session.QuickInventory.Subscribe(Rebuild));
            _trash.Retain(_session.BaseInventory.InterfaceUpdateTrigger.Subscribe(OnAccessChange));
            Rebuild();
        }

        private void OnAccessChange(int oldValue, int newValue)
        {
            _session.QuickInventory.OnForcedRebuild();
        }

        private void Rebuild()
        {
            var inventory = _session.QuickInventory.QuickInventory;
            _dataGroup.SetData(inventory);
        }


        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}