using PixelCrew.Model.Data.Properties;
using UnityEngine;

namespace PixelCrew.Model.Data
{
    [CreateAssetMenu(menuName = "Data/GameSettings", fileName = "GameSettings")]
    public class GameSettings : ScriptableObject
    {
        [SerializeField] private FloatPresistentProperty _music;
        [SerializeField] private FloatPresistentProperty _sfx;

        public FloatPresistentProperty Music => _music;
        public FloatPresistentProperty Sfx => _sfx;

        private static GameSettings _instance;
        public static GameSettings I => _instance == null ? LoadGameSettings() : _instance;

        private static GameSettings LoadGameSettings()
        {
            return Resources.Load<GameSettings>("GameSettings");
        }

        private void OnEnable()
        {
            _music = new FloatPresistentProperty(1f, SoundSetting.Music.ToString());
            _sfx = new FloatPresistentProperty(1f, SoundSetting.Sfx.ToString());
        }

        private void OnValidate()
        {
            _music.Validate();
            _sfx.Validate();
        }
    }

    public enum SoundSetting
    {
        Music,
        Sfx
    }
}
