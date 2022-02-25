﻿using System.Collections;
using UnityEngine;
using PixelCrew.Model;
using System;
using UnityEngine.SceneManagement;
using PixelCrew.Model.Data;

namespace PixelCrew.Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;

        public PlayerData Data => _data;
        private PlayerData _save;
        public QuickInventoryModel QuickInventory { get; private set; }

        private void Awake()
        {
            LoadHUD();

            if (IsSessionExit())
            {
                Destroy(gameObject);
            }
            else
            {
                Save();
                InitModels();
                DontDestroyOnLoad(this);
            }
        }

        private void InitModels()
        {
            QuickInventory = new QuickInventoryModel(Data);            
        }

        private void LoadHUD()
        {
            SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
        }

        private bool IsSessionExit()
        {
            var sessions = FindObjectsOfType<GameSession>();
            foreach (var gameSession in sessions)
            {
                if (gameSession != this)
                {
                    return true;
                }
            }

            return false;
        }

        public void Save()
        {
            _save = Data.Clone();
        }

        public void LoadLastSave()
        {
            _data = _save.Clone();
        }

    }
}