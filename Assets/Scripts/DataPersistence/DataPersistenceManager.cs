using System;
using System.Collections.Generic;
using DataPersistence.Data;
using UnityEngine;

namespace DataPersistence
{
    public class DataPersistenceManager: MonoBehaviour
    {
        
        private GameData gameData;
        private List<IDataPersistence> dataPersistenceObjects = new List<IDataPersistence>();
        //This is created before the Awake function is called
        
        [Header("File Storage Config")]
        [SerializeField] private string fileName = "gameData.json";
        [SerializeField] private bool useEncryption = false;
        private FileDataHandler fileDataHandler;
        public static DataPersistenceManager Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void Initialize()
        {
            fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
            LoadGame();
        }

        public void NewGame()
        {
            gameData = new GameData();
            // Initialize other game data as needed
        }
        
        public void LoadGame()
        {
            gameData = fileDataHandler.Load();
            //if no data is found, create a new game

            if (gameData == null)
            {
                Debug.Log("No game data was found. Initializing data to defaults" );
                gameData = new GameData();
            }
            foreach (IDataPersistence persistence in dataPersistenceObjects)
            {
                persistence.LoadData(gameData);
            }
        }
        
        public void SaveGame()
        {
            foreach (IDataPersistence persistence in dataPersistenceObjects)
            {
                persistence.SaveData(ref gameData);
            }
            fileDataHandler.Save(gameData);
        }

        private void OnApplicationQuit()
        {
            SaveGame(); 
        }

        public void AddDataPersistenceObject(IDataPersistence persistenceObject)
        {
            dataPersistenceObjects.Add(persistenceObject);
        }
        
    }
}