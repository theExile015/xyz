using UnityEngine;


namespace PixelCrew.Model.Data.Properties
{
    public abstract class PrefsPresistentProperty<TPropertyType> : PresistentProperty<TPropertyType>
    {
        protected string Key;
        protected PrefsPresistentProperty(TPropertyType defaultValue, string key): base(defaultValue)
        {
            Key = key;
        }
    }
}