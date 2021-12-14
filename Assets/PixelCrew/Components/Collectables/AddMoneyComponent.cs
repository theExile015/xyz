﻿using UnityEngine;
using PixelCrew.Creatures.Hero;

namespace PixelCrew.Components.Collectables
{

    public class AddMoneyComponent : MonoBehaviour
    {
        [SerializeField] private byte _value;
        private Hero _hero;


        private void Start()
        {
            _hero = FindObjectOfType<Hero>();
        }

        public void AddSomeMoney()
        {
            if (_value > 0)
            {
                _hero.AddSomeMoney(_value);
            }
        }

    }
}