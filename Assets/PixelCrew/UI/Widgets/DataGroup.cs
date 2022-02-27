﻿using PixelCrew.UI.HUD.QuickInventory;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PixelCrew.UI.Widgets
{
    public class DataGroup<TDataType, TItemType>
        where TItemType : MonoBehaviour, IItemRenderer<TDataType>
    {
        private List<TItemType> _createdItem = new List<TItemType>();
        private TItemType _prefub;
        private Transform _container;

        public DataGroup(TItemType prefub, Transform container)
        {
            _prefub = prefub;
            _container = container;
        }

        public void SetData(IList<TDataType> data)
        {
            for (var i = _createdItem.Count; i < data.Count; i++)
            {
                var item = Object.Instantiate(_prefub, _container);
                _createdItem.Add(item);
            }

            // update data and activate
            for (var i = 0; i < data.Count; i++)
            {
                _createdItem[i].SetData(data[i], i);
                _createdItem[i].gameObject.SetActive(true);
            }

            // hide unused items
            for (var i = data.Count; i < _createdItem.Count; i++)
            {
                _createdItem[i].gameObject.SetActive(false);
            }
        }
    }

    public interface IItemRenderer<TDataType>
    {
        void SetData(TDataType data, int index);
    }
}