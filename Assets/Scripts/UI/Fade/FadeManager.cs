using System;
using System.Collections;
using System.Collections.Generic;
using EventSystem.UI;
using Febucci.UI;
using GeneralManagers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Fade
{
    public class FadeManager : MonoBehaviour
    {
        private UIManager _uiManager;

        [Header("Floating Text")] [SerializeField]
        private TextAnimator_TMP floatingText;

        [SerializeField] private TypewriterByCharacter typewriter;
        private List<string> _stringsToWrite;
        private List<float> _textSpeed;

        [Header("Fade Panel")]
        [SerializeField] private Image fadePanel;
        private float _fadeInDuration = 2;
        private float _fadeOutDuration = 2;
        private int _stringIndex = 0;
        private Color _originalColor;
        private Action _onComplete;
        private Action _onTextStartDissapearing;

        private void Awake()
        {
            typewriter.onTextShowed.AddListener(OnTextShowed);
            typewriter.onTextDisappeared.AddListener(OnTextDissapeared);
            FadeEventSystem.OnFade += OnFade;
            _originalColor = fadePanel.color;
        }

        private void OnDestroy()
        {
            typewriter.onTextShowed.RemoveListener(OnTextShowed);
            typewriter.onTextDisappeared.RemoveListener(OnTextDissapeared);
            FadeEventSystem.OnFade -= OnFade;
        }

        private void OnFade(FadeEventSystem.FadeEventArgs obj)
        {
            Setup(obj.FadeOutDuration, obj.FadeInDuration, obj.OnComplete, obj.OnTextStartDissapearing, 
          obj.StringsToWrite, obj.TextSpeed, obj.SkipFadeOut);
        }

        public void Setup(float fadeOutDuration, float fadeInDuration, Action onComplete = null, Action onTextStartDissapearing = null,
            List<string> stringsToWrite = null, List<float> textSpeed = null, bool skipFadeOut = false)
        {
            _stringIndex = 0;
            _fadeOutDuration = fadeOutDuration;
            _fadeInDuration = fadeInDuration;
            _stringsToWrite = stringsToWrite;
            _onComplete = onComplete;
            _onTextStartDissapearing = onTextStartDissapearing;
            
            // Reset panel color
            Color color = fadePanel.color;
            color.a = 0;
            fadePanel.color = color;
            fadePanel.gameObject.SetActive(true);
            
            // Skip fadeout if requested
            if (skipFadeOut)
            {
                // Set panel to fully faded out state immediately
                color.a = 1;
                fadePanel.color = color;
                
                // Show text if any, otherwise start fade in
                if (_stringsToWrite != null)
                {
                    if (_textSpeed != null)
                    {
                        typewriter.SetTypewriterSpeed(_textSpeed[_stringIndex]);
                    }
                    else
                    {
                        typewriter.SetTypewriterSpeed(0.05f);
                    }

                    typewriter.ShowText(_stringsToWrite[_stringIndex]);
                }
                else
                {
                    // If no text to show, immediately start fade in
                    StartCoroutine(FadeIn());
                }
            }
            else
            {
                // Normal fadeout process
                StartCoroutine(FadeOut());
            }
        }

        private IEnumerator FadeOut()
        {
            while (fadePanel.color.a <= 1)
            {
                Color color = fadePanel.color;
                color.a += UnityEngine.Time.deltaTime / _fadeOutDuration;
                fadePanel.color = color;
                yield return null;
            }

            if (_stringsToWrite != null)
            {
                if (_textSpeed != null)
                {
                    typewriter.SetTypewriterSpeed(_textSpeed[_stringIndex]);
                }
                else
                {
                    typewriter.SetTypewriterSpeed(0.05f);
                }

                typewriter.ShowText(_stringsToWrite[_stringIndex]);
            }
        }

        private IEnumerator FadeIn()
        {
            while (fadePanel.color.a >= 0)
            {
                Color color = fadePanel.color;
                color.a -= UnityEngine.Time.deltaTime / _fadeInDuration;
                fadePanel.color = color;
                yield return null;
            }
            fadePanel.gameObject.SetActive(false);
            _onComplete?.Invoke();
        }

        private void OnTextShowed()
        {
            if (_stringsToWrite != null)
            {
                if (_stringIndex < _stringsToWrite.Count - 1)
                {
                    _stringIndex++;
                    if (_textSpeed != null)
                    {
                        typewriter.SetTypewriterSpeed(_textSpeed[_stringIndex]);
                    }

                    typewriter.ShowText(_stringsToWrite[_stringIndex]);
                }
                else
                {
                    _onTextStartDissapearing?.Invoke();
                    typewriter.StartDisappearingText();
                }
            }
            else
            {
                StartCoroutine(FadeIn());
            }
        }

        private void OnTextDissapeared()
        {
            //Clear all texts
            floatingText.textFull = "";
            StartCoroutine(FadeIn());
        }
    }
}