using PixelCrew.Creatures;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew
{

    public class HeroInputReader : MonoBehaviour
    {
        [SerializeField] private Hero _hero;

        private float _timeStarted;

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
                _timeStarted = Time.time;               
            }

            if (context.canceled)
            {
                var duration = Time.time - _timeStarted;
                if (duration < 1f)
                {
                    _hero.Throw();
                }
                else
                {
                    _hero.TrippleThrow();
                }
            } 
        }
    }
}

