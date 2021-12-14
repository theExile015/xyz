using System.Collections;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs.Patrlolling
{
    public abstract class Patrol : MonoBehaviour
    {
        public abstract IEnumerator DoPatrol();
    }
}