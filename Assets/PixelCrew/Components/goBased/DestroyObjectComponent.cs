﻿using PixelCrew.Model;
using UnityEngine;

namespace PixelCrew.Components.goBased
{

    public class DestroyObjectComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _objectToDestroy;
        [SerializeField] RestoreStateComponent _state;
        public void DestroyObject()
        {
            Destroy(_objectToDestroy);
            if (_state != null)
                GameSession.Instance.StoreState(_state.ID);
        }

    }

}




