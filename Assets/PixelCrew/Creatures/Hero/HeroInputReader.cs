﻿using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew.Creatures.Hero
{

    public class HeroInputReader : MonoBehaviour
    {
        [SerializeField] private Hero _hero;


        public void OnSaySomething(InputAction.CallbackContext context)
        {
            if (context.performed)
            {

            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.Interact();
            }
        }

        public void OnAxisMovement(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();
            _hero.SetDirection(direction);
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.Attack();
            }
        }

        public void OnThrow(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _hero.StartThrowing();
            }

            if (context.canceled)
            {
                _hero.PerformThrowing();
            }
        }

        public void OnPotionUse(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.UsePotion();
            }
        }

        public void OnNextItem(InputAction.CallbackContext context)
        {
            if (context.performed)
                _hero.NextItem();
        }
    }
}

