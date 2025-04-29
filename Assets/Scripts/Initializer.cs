using UnityEngine;

namespace DefaultNamespace
{
    public static class Initializer
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            if (GameObject.Find("PERSISTOBJECTS(Clone)") == null)
            {
                var prefab = UnityEngine.Resources.Load<GameObject>("PERSISTOBJECTS");
                if (prefab != null)
                {
                    Object.DontDestroyOnLoad(Object.Instantiate(prefab));
                }
                else
                {
                    Debug.LogError("Failed to load PERSISTOBJECTS from Resources.");
                }
            }
        }
    }
}