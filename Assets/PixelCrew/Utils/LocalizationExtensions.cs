using PixelCrew.Model.Definitions.Localization;
using System.Collections;
using UnityEngine;

namespace PixelCrew.Utils
{
    public static class LocalizationExtensions
    {
        public static string Localize(this string key)
        {
            return LocalizationManager.I.Localize(key);
        }
    }
}