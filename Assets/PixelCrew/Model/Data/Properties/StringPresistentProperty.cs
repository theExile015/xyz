using PixelCrew.Model.Data.Properties;
using System.Collections;
using UnityEngine;

namespace PixelCrew.Model.Data.Properties
{
    public class StringPresistentProperty : PrefsPresistentProperty<string>
    {
        public StringPresistentProperty(string defaultValue, string key) : base(defaultValue, key)
        {
            Init();
        }

        protected override string Read(string defaultValue)
        {
            return PlayerPrefs.GetString(Key, defaultValue);
        }

        protected override void Write(string value)
        {
            PlayerPrefs.SetString(Key, value);
        }
    }
}