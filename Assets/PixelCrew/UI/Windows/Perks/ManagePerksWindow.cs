using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Windows.Perks
{
    public class ManagePerksWindow : AnimatedWindow
    {
        [SerializeField] private Button _buyButton;
        [SerializeField] private Button _useButton;
        [SerializeField] private ItemWidget _price;
        [SerializeField] private Text _info;
        [SerializeField] private Transform _perksContainer;

        private PredefinedDataGroup<string, PerkWidget> _dataGroup;
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private GameSession _session;

        protected override void Start()
        {
            base.Start();

            _dataGroup = new PredefinedDataGroup<string, PerkWidget>(_perksContainer);
            _session = FindObjectOfType<GameSession>();

            _trash.Retain(_buyButton.onClick.Subscribe(OnBuy));
            _trash.Retain(_useButton.onClick.Subscribe(OnUse));      
        }

        private void OnPerksChanged()
        {
            _dataGroup.SetData(DefsFacade.I.Perks.All);
        }

        private void OnUse()
        {
            var selected = _session.PerksModel.InterfaceSelection.Value;
            _session.PerksModel.UsePerk(selected);
        }

        public void OnBuy()
        {
            var selected = _session.PerksModel.InterfaceSelection.Value;
            _session.PerksModel.Unlock(selected);
        }
    }
}