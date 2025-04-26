
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class Initializer : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            string sceneName = SceneManager.GetActiveScene().name;
            if (sceneName != "IntroScene" && sceneName != "MainMenuScene" && GameObject.Find("PERSISTOBJECTS(Clone)") == null)
            {
                DontDestroyOnLoad(Instantiate(UnityEngine.Resources.Load("PERSISTOBJECTS")));
            }
        }
    }
}