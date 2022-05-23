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
            switch (_control)
            {
                case MobileControl.ControlLeft:
                    _hero.SetDirection(Vector2.left);
                    break;

                case MobileControl.ControlRight:
                    _hero.SetDirection(Vector2.right);
                    break;

                case MobileControl.ControlA:
                    _hero.SetDirection(Vector2.up);
                    break;

                default:
                    break;
            }
        }

        public void OnControlUp()
        {
            TryFindHero();
            _hero.SetDirection(Vector2.zero);
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