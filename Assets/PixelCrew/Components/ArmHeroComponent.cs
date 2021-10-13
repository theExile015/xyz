﻿using System.Collections;
using UnityEngine;
using PixelCrew;

namespace Assets.PixelCrew.Components
{
    public class ArmHeroComponent : MonoBehaviour
    {
        public void ArmHero(GameObject go)
        {
            var hero = go.GetComponent<Hero>();
            if (hero != null)
            {
                hero.ArmHero();
            }

        }
    }
}