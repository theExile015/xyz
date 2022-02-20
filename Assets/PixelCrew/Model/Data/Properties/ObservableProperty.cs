﻿using PixelCrew.Utils.Disposables;
using System;
using System.Collections;
using UnityEngine;

namespace PixelCrew.Model.Data.Properties
{
    public class ObservableProperty<TPropertyType>
    {
        [SerializeField] protected TPropertyType _value;

        public delegate void OnPropertyChanged(TPropertyType newValue, TPropertyType oldValue);

        public event OnPropertyChanged OnChanged;

        public IDisposable Subscribe(OnPropertyChanged call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
        }


        public virtual TPropertyType Value
        {
            get => _value;
            set
            {
                var isSame = _value.Equals(value);
                if (isSame) return;
                var oldValue = _value;
                InvokeChangedEvent(_value, oldValue);
                _value = value;
               
            }
        }

        protected void InvokeChangedEvent(TPropertyType newValue, TPropertyType oldValue)
        {
            OnChanged?.Invoke(newValue, oldValue);
        }

    }
}