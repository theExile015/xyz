using System;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.ColliderBased
{
    public class EnterCollisionComponent : MonoBehaviour
    {
        [SerializeField] private string[] _tags;
        [SerializeField] private EnterEvent _action;

        private void OnCollisionEnter2D(Collision2D other)
        {
            var isInTags = false;
            foreach (string tag in _tags)
            {
                isInTags = other.gameObject.CompareTag(tag);
                if (isInTags) break;
            }

            if (isInTags)
            {
                _action?.Invoke(other.gameObject);
            }
        }

        [Serializable]
        public class EnterEvent : UnityEvent<GameObject>
        {

        }

    }

}


