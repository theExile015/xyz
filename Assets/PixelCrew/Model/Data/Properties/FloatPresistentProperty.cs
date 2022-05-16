﻿using System;
using UnityEngine;

namespace PixelCrew.Model.Data.Properties
{
    [Serializable]
    public class FloatPresistentProperty : PrefsPresistentProperty<float>
    {
        public FloatPresistentProperty(float defaultValue, string key) : base(defaultValue, key)
        {
            Init();
        }

        protected override float Read(float defaultValue)
        {
            return PlayerPrefs.GetFloat(Key, defaultValue);
        }

        protected override void Write(float value)
        {
            PlayerPrefs.SetFloat(Key, value);
            PlayerPrefs.Save();
        }
    }
}