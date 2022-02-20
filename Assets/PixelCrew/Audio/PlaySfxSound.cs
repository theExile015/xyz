using Assets.PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Audio
{
    public class PlaySfxSound : MonoBehaviour
    {
        [SerializeField] private AudioClip _clip;
        private AudioSource _source;

        public void Play()
        {
            if (_source == null)
                _source = AudioUtils.FindSfxSound();

            _source.PlayOneShot(_clip);
        }

    }
}