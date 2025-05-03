using System.Collections;
using GeneralManagers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SceneSwitch
{
    public class SceneFadeManager : MonoBehaviour
    {
        public static SceneFadeManager Instance;
        [SerializeField] private Image _fadeOutImage;
        [Range(0.1f, 10f)] private float _fadeOutSpeed = 2f;
        [Range(0.1f, 10f)] private float _fadeInSpeed = 2f;

        [SerializeField] private Color _fadeOutStartColor = Color.black;

        public bool IsFadingOut { get; private set; }
        public bool IsFadingIn { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            _fadeOutStartColor.a = 0f;
        }


        private void Update()
        {
            if (IsFadingOut)
            {
                if (_fadeOutImage.color.a < 1)
                {
                    _fadeOutStartColor.a += _fadeOutSpeed * Time.deltaTime;
                    _fadeOutImage.color = _fadeOutStartColor;
                }
                else
                {
                    IsFadingOut = false;
                }
            }

            if (IsFadingIn)
            {
                if (_fadeOutImage.color.a > 0)
                {
                    _fadeOutStartColor.a -= _fadeInSpeed * Time.deltaTime;
                    _fadeOutImage.color = _fadeOutStartColor;
                }
                else
                {
                    GameManager.Instance.InputManager.EnablePlayerControls();
                    IsFadingIn = false;
                }
            }
        }

        public void StartFadeOut()
        {
            _fadeOutImage.color = _fadeOutStartColor;
            IsFadingOut = true;
            GameManager.Instance.InputManager.DisablePlayerControls();
        }

        public void StartFadeIn()
        {
            StartCoroutine(FadeInCoroutine());
        }

        private IEnumerator FadeInCoroutine()
        {
            while (_fadeOutImage.color.a < 1)
            {
                yield return null;
            }

            _fadeOutImage.color = _fadeOutStartColor;
            IsFadingIn = true;
        }
    }
}