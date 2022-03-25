﻿using PixelCrew.Model;
using System.Collections;
using UnityEngine;

namespace PixelCrew.Components
{
    public class RestoreStateComponent : MonoBehaviour
    {
        [SerializeField] private string _id;

        private GameSession _session;

        public string ID => _id;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            var isDestroyed = _session.RestoreState(ID);
            if (isDestroyed)
                Destroy(gameObject);
        }
    }
}