using DataPersistence;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI.SaveLoad
{
    public class LoadButtonUI: MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnLoadButtonClicked);
        }

        private void OnLoadButtonClicked()
        {
            DataPersistenceManager.Instance.LoadGame();
        }
    }
}