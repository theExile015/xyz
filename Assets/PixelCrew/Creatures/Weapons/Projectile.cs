﻿using UnityEngine;

namespace PixelCrew.Creatures.Weapons
{

    public class Projectile : BaseProjectile
    {

        protected override void Start()
        {
            base.Start();
            Pull();
        }

        public void Pull()
        {
            var force = new Vector2(_speed * Direction, 0);
            Rigidbody.AddForce(force, ForceMode2D.Impulse);
        }
    }
}
