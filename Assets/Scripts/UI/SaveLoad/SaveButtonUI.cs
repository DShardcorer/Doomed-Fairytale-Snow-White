using System;
using DataPersistence;
using UnityEngine;

namespace DefaultNamespace.UI.SaveLoad
{
    public class SaveButtonUI: MonoBehaviour
    {
        private void Start()
        {
            // Add a listener to the button to call the SaveGame method when clicked
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(SaveGame);
        }

        private void SaveGame()
        {
            DataPersistenceManager.Instance.SaveGame(); 
        }
    }
}