﻿using Assets.PixelCrew.Utils;
using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions.Localization;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.HUD.Dialogs
{
    public class DialogBoxController : MonoBehaviour
    {
        [SerializeField] private GameObject _container;
        [SerializeField] private Animator _animator;

        [Space] [SerializeField] private float _textSpeed = 0.09f;

        [Header("Sounds")]
        [SerializeField] private AudioClip _typing;
        [SerializeField] private AudioClip _open;
        [SerializeField] private AudioClip _close;

        [Space][SerializeField] protected DialogContent _content;

        private static readonly int IsOpen = Animator.StringToHash("IsOpen");

        private DialogData _data;
        private int _currentSentence;
        private AudioSource _sfxSource;
        private Coroutine _typingRoutine;

        protected Sentence CurrentSentence => _data.Sentences[_currentSentence];

        private void Start()
        {
            _sfxSource = AudioUtils.FindSfxSound();
        }

        public void ShowDialog(DialogData data)
        {
            _data = data;
            _currentSentence = 0;
            CurrentContent.Text.text = string.Empty;

            _container.SetActive(true);
            _sfxSource.PlayOneShot(_open);
            _animator.SetBool(IsOpen, true);
        }

        private IEnumerator TypeDialogText()
        {
            CurrentContent.Text.text = String.Empty;
            var sentenceKey = _data.Sentences[_currentSentence];
            var sentence = LocalizationManager.I.Localize(sentenceKey.Value); //преобразуем ключ в текст из данных локализации
            CurrentContent.TrySetIcon(sentenceKey.Icon);

            foreach (var letter in sentence)
            {
                CurrentContent.Text.text += letter;
                _sfxSource.PlayOneShot(_typing);
                yield return new WaitForSeconds(_textSpeed);
            }

            _typingRoutine = null;
        }

        protected virtual DialogContent CurrentContent => _content;

        public void OnSkip()
        {
            if (_typingRoutine == null) return;

            StopTypingAnimation();
            CurrentContent.Text.text = _data.Sentences[_currentSentence].Value;
        }

        public void OnContinue()
        {
            StopTypingAnimation();
            _currentSentence++;

            var isDialogCompleted = _currentSentence >= _data.Sentences.Length;
            if (isDialogCompleted)
            {
                HideDialogBox();
            }
            else
            {
                OnStartDialogAnimation();
            }
        }

        private void HideDialogBox()
        {
            _animator.SetBool(IsOpen, false);
            _sfxSource.PlayOneShot(_close);
        }

        private void StopTypingAnimation()
        {
            if (_typingRoutine != null)
                StopCoroutine(_typingRoutine);
            _typingRoutine = null;
        }


        protected virtual void OnStartDialogAnimation()
        {
            _typingRoutine = StartCoroutine(TypeDialogText());
        }

        private void OnCloseAnimationComplete()
        {
            _container.SetActive(false);
        }
    }
}