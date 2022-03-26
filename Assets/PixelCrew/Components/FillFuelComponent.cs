using PixelCrew.Model;
using System.Collections;
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

        private void AddFuel()
        {
            _session.Data.Fuel.Value += _value;
        }
    }
}