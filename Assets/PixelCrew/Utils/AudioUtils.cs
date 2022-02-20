using UnityEngine;

namespace Assets.PixelCrew.Utils
{
    public class AudioUtils
    {
        public const string SfxSourceTag = "SfxAudioSource";
        public static AudioSource FindSfxSound()
        {
            return GameObject.FindWithTag(SfxSourceTag).GetComponent<AudioSource>();
        }

    }
}