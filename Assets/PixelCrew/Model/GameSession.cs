﻿using PixelCrew.Components.LevelManagement;
using PixelCrew.Model.Models;
using PixelCrew.Model.Player;
using PixelCrew.Utils.Disposables;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

namespace PixelCrew.Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private int _levelIndex;
        [SerializeField] private PlayerData _data;
        [SerializeField] private string _defaultCheckPoint;

        public static GameSession Instance { get; private set; }

        public PlayerData Data => _data;
        private PlayerData _save;
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        public QuickInventoryModel QuickInventory { get; private set; }
        public BaseInventoryModel BaseInventory { get; private set; }
        public PerksModel PerksModel { get; private set; }
        public StatsModel StatsModel { get; private set; }

        private List<string> _checkPoints = new List<string>();

        private void Awake()
        {
            var existsSession = GetExistsSession();
            if (existsSession != null)
            {
                existsSession.StartSession(_defaultCheckPoint, _levelIndex);
                Destroy(gameObject);
            }
            else
            {
                Save();
                InitModels();
                DontDestroyOnLoad(this);
                Instance = this;
                StartSession(_defaultCheckPoint, _levelIndex);
            }
        }

        private void StartSession(string defaultCheckPoint, int levelIndex)
        {
            SetChecked(defaultCheckPoint);
            TrackSessionStart(levelIndex);

            LoadUIs();
            SpawnHero();
        }

        private void TrackSessionStart(int levelIndex)
        {
            var eventParams = new Dictionary<string, object>
            {
                {"level_index", levelIndex }
            };
            AnalyticsEvent.Custom("level_start", eventParams);
        }

        private void SpawnHero()
        {
            var checkpoints = FindObjectsOfType<CheckPointComponent>();
            var lastCheckPoint = _checkPoints.Last();
            foreach (var checkPoint in checkpoints)
            {
                if (checkPoint.Id == lastCheckPoint)
                {
                    checkPoint.SpawnHero();
                    break;
                }
            }
        }

        private void InitModels()
        {
            QuickInventory = new QuickInventoryModel(_data);
            _trash.Retain(QuickInventory);

            BaseInventory = new BaseInventoryModel(_data);
            _trash.Retain(BaseInventory);

            PerksModel = new PerksModel(_data);
            _trash.Retain(PerksModel);

            StatsModel = new StatsModel(_data);
            _trash.Retain(StatsModel);

            _data.HP.Value = (int)StatsModel.GetValue(StatId.Hp);
        }

        private void LoadUIs()
        {
            SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
            LoadOnScreenControls();
        }

        [Conditional("USE_ONSCREEN_CONTROLS")]
        private void LoadOnScreenControls()
        {
            SceneManager.LoadScene("Controls", LoadSceneMode.Additive);
        }

        private GameSession GetExistsSession()
        {
            var sessions = FindObjectsOfType<GameSession>();
            foreach (var gameSession in sessions)
            {
                if (gameSession != this)
                {
                    return gameSession;
                }
            }

            return null;
        }

        public void Save()
        {
            _save = Data.Clone();
        }

        public void LoadLastSave()
        {
            _data = _save.Clone();

            _trash.Dispose();
            InitModels();
        }

        public void SetChecked(string id)
        {
            if (!_checkPoints.Contains(id))
            {
                Save();
                _checkPoints.Add(id);
            }
        }

        public bool IsChecked(string id)
        {
            return _checkPoints.Contains(id);
        }

        public void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
            _trash.Dispose();
        }

        private List<string> _removedItems = new List<string>();

        public bool RestoreState(string id)
        {
            return _removedItems.Contains(id);
        }

        public void StoreState(string id)
        {
            if (!_removedItems.Contains(id))
                _removedItems.Add(id);
        }
    }
}