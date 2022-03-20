﻿using System.Collections;
using UnityEngine;

namespace PixelCrew.UI.Windows
{
    public class AnimatedWindow : MonoBehaviour
    {
        private Animator _animator;
        private static readonly int Show = Animator.StringToHash("Show");
        private static readonly int Hide = Animator.StringToHash("Hide");


        protected virtual void Start()
        {
            _animator = GetComponent<Animator>();

            _animator.SetTrigger(Show);
        }

        public void Close()
        {
            _animator.SetTrigger(Hide);
        }

        public void JustClose()
        {
            Destroy(gameObject);
        }


        public virtual void OnCloseAnimationComplete()
        {
            Destroy(gameObject);
        }
    }
}