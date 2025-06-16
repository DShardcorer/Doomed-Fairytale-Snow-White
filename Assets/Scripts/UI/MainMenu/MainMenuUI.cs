using GeneralManagers;
using SceneSwitch;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI.MainMenu
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button continueButton;
        [SerializeField] private Button optionsButton;
        [SerializeField] private Button exitButton;
        [SerializeField] private SceneField prologueScene;
        private void Start()
        {
            startButton.onClick.AddListener(OnStartButtonClick);
            continueButton.onClick.AddListener(OnContinueButtonClick);
            optionsButton.onClick.AddListener(OnOptionsButtonClick);

            exitButton.onClick.AddListener(OnExitButtonClick);
        }


        private void OnStartButtonClick()
        {
            SceneSwitchManager.Instance.SwitchSceneSpecial(prologueScene);
        }

        private void OnContinueButtonClick()
        {
            // Logic to continue the game, e.g., load the last saved scene or player state
        }

        private void OnOptionsButtonClick()
        {
        }

        private void OnExitButtonClick()
        {
            // Exit the application
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }
}