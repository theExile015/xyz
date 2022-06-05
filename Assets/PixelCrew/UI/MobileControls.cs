using PixelCrew.Creatures.Hero;
using System;
using System.Collections;
using UnityEngine;

namespace PixelCrew.UI
{
    public class MobileControls : MonoBehaviour
    {
        [SerializeField] private MobileControl _control;

        private Hero _hero;

        private void TryFindHero()
        {
            if (_hero != null) return;
            var hero = GameObject.FindObjectOfType<Hero>();
            _hero = hero.GetComponent<Hero>();
        }

        public void OnControlDown()
        {
            TryFindHero();
            var direction = _hero._Direction;
            switch (_control)
            {
                case MobileControl.ControlLeft:
                    direction.x = -1;
                    _hero.SetDirection(direction);
                    break;

                case MobileControl.ControlRight:
                    direction.x = 1;
                    _hero.SetDirection(direction);
                    break;

                case MobileControl.ControlA:
                    direction.y = 1;
                    _hero.SetDirection(direction);
                    break;

                case MobileControl.ControlB:
                    _hero.Attack();
                    break;

                case MobileControl.ControlX:
                    _hero.Interact();
                    break;

                case MobileControl.ControlY:
                    _hero.StartThrowing();
                    break;

                default:
                    break;
            }


        }

        public void OnControlUp()
        {
            TryFindHero();
            var direction = _hero._Direction;
            switch (_control)
            {
                case MobileControl.ControlLeft:
                    direction.x = 0;
                    _hero.SetDirection(direction);
                    break;

                case MobileControl.ControlRight:
                    direction.x = 0;
                    _hero.SetDirection(direction);
                    break;

                case MobileControl.ControlA:
                    direction.y = 0;
                    _hero.SetDirection(direction);
                    break;

                default:
                    break;
            }
        }

        private void OnDestroy()
        {
            _hero = null;
        }

        [Serializable]
        public enum MobileControl
        {
            ControlLeft,
            ControlRight,
            ControlA,
            ControlB,
            ControlX,
            ControlY,
        }
    }


}