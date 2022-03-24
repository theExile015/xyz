using PixelCrew.Model;
using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Repositories.Items;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Windows.BaseInventory
{
    public class BaseInventoryItemWidget : MonoBehaviour, IItemRenderer<InventoryItemData>
    {
        [SerializeField] private int _id;
        [SerializeField] private Image _icon;
        [SerializeField] private GameObject _isUsed;
        [SerializeField] private GameObject _isSelected;
        [SerializeField] private Text _count;

        private bool IsEmpty => _id > _session.Data.Inventory.GetAll(ItemTag.Visible).Length;
        private GameSession _session;
        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _isSelected.GetComponent<Image>().color = Color.yellow;
            var index = _session.BaseInventory.InterfaceSelection;
            var trigger = _session.BaseInventory.InterfaceUpdateTrigger;
            _trash.Retain(index.SubscribeAndInvoke(UpdadeView));
            _trash.Retain(trigger.Subscribe(UpdadeView));
        }

        private void UpdadeView(int newValue, int oldValue)
        {
            var items = _session.Data.Inventory.GetAll(ItemTag.Visible);

            _icon.gameObject.SetActive(!IsEmpty);
            _isUsed.gameObject.SetActive(!IsEmpty);
            _count.gameObject.SetActive(!IsEmpty);
            _isSelected.gameObject.SetActive(_session.BaseInventory.InterfaceSelection.Value == _id);
            
            if(!IsEmpty)
            {
                var def = DefsFacade.I.Items.Get(items[_id - 1].Id);
                _icon.sprite = def.Icon;
                _isUsed.SetActive(items[_id - 1].HasQuickAccess);
                _count.text = items[_id - 1].Value.ToString();
            }
        }

        public void OnSelect()
        {
            _session.BaseInventory.InterfaceSelection.Value = _id;
        }

        public void SetData(InventoryItemData data, int index)
        {
            
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}