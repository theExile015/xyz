﻿using PixelCrew.Model;
using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions;
using PixelCrew.Utils.Disposables;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.HUD.QuickInventory
{
    public class InventoryItemWidget : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private GameObject _selection;
        [SerializeField] private Text _value;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private int _index;

        private void Start()
        {
            var session = FindObjectOfType<GameSession>();
            var index = session.QuickInventory.SelectedIndex;
            _trash.Retain(index.SubscribeAndInvoke(OnIndexChanged));
        }

        private void OnIndexChanged(int newValue, int oldValue)
        {
            _selection.SetActive(_index == newValue);
        }

        public void SetData(InventoryItemData item, int index)
        {
            _index = index;
            var def = DefsFacade.I.Items.Get(item.Id);
            _icon.sprite = def.Icon;
            _value.text = def.HashTag(ItemTag.Stackable) ? item.Value.ToString() : string.Empty;

        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}