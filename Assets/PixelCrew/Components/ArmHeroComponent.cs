using System.Collections;
using UnityEngine;
using PixelCrew.Creatures;

namespace Assets.PixelCrew.Components
{
    public class ArmHeroComponent : MonoBehaviour
    {
        [SerializeField] [Range(0, 10)] private int _thrownNumber;
        public void ArmHero(GameObject go)
        {
            var hero = go.GetComponent<Hero>();
            if (hero != null)
            {
                hero.ArmHero(_thrownNumber);
            }

        }
    }
}