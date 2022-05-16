using PixelCrew.Model;
using UnityEngine;

namespace PixelCrew.Components
{
    public class FillFuelComponent : MonoBehaviour
    {
        [SerializeField] private float _value;

        private GameSession _session;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
        }

        public void AddFuel()
        {
            _session.Data.Fuel.Value = 100;
        }
    }
}