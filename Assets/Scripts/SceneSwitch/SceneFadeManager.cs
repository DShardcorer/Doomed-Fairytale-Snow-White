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
        
        private string _targetSceneName;
        private int _targetSceneIndex = -1;
        private bool _useSceneName = true;

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
                    // Change scene when fully black
                    if (_useSceneName)
                        SceneManager.LoadScene(_targetSceneName);
                    else
                        SceneManager.LoadScene(_targetSceneIndex);
                    
                    // Start fade in automatically
                    StartFadeIn();
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

        public void FadeToScene(string sceneName)
        {
            _targetSceneName = sceneName;
            _useSceneName = true;
            StartFadeOut();
        }

        public void FadeToScene(int sceneIndex)
        {
            _targetSceneIndex = sceneIndex;
            _useSceneName = false;
            StartFadeOut();
        }

        public void StartFadeOut()
        {
            _fadeOutImage.color = _fadeOutStartColor;
            IsFadingOut = true;
            GameManager.Instance.InputManager.DisablePlayerControls();
        }

        public void StartFadeIn()
        {
            _fadeOutStartColor.a = 1f;
            _fadeOutImage.color = _fadeOutStartColor;
            IsFadingIn = true;
        }
    }
}