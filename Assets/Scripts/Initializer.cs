using UnityEngine;

namespace DefaultNamespace
{
    public class Initializer
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            Object.DontDestroyOnLoad(Object.Instantiate(UnityEngine.Resources.Load("PERSISTOBJECTS")));
        }
    }
}