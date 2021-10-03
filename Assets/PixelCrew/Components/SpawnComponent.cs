using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixeCrew.Components
{

    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _prefub;

        [ContextMenu("Spawn")]

        public void Spawn()
        {
            Instantiate(_prefub, _target);
        }
    }
}
