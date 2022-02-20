using PixelCrew.Model;
using PixelCrew.Utils.Disposables;
using System.Collections;
using UnityEngine;

namespace PixelCrew.UI.HUD.QuickInventory
{
    public class QuickInventoryController : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private GameObject _prefub;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private GameSession _session;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
        }

    }
}