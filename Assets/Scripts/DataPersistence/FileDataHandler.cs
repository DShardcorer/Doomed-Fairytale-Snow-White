using System;
using System.IO;
using DataPersistence.Data;
using Newtonsoft.Json;
using UnityEngine;

namespace DataPersistence
{
    public class FileDataHandler
    {
        private string dataDirectoryPath;
        private string dataFileName;
        private bool useEncryption = false;
        private readonly string encryptionKey = "doomed_fairytale:snow_white";


        public FileDataHandler(string dataDirectoryPath, string dataFileName, bool useEncryption = false)
        {
            this.dataDirectoryPath = dataDirectoryPath;
            this.dataFileName = dataFileName;
            this.useEncryption = useEncryption;
        }

        public GameData Load()
        {
            string fullPath = Path.Combine(dataDirectoryPath, dataFileName);
            GameData loadedGameData = null;

            if (File.Exists(fullPath))
            {
                try
                {
                    string dataToLoad = File.ReadAllText(fullPath);
                    if (useEncryption)
                    {
                        dataToLoad = EncryptDecrypt(dataToLoad);
                    }

                    // Deserialize with Newtonsoft
                    loadedGameData = JsonConvert.DeserializeObject<GameData>(dataToLoad);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error occurred while trying to load file: {fullPath}\n{e}");
                }
            }
            

            return loadedGameData;
        }

        public void Save(GameData gameData)
        {
            string fullPath = Path.Combine(dataDirectoryPath, dataFileName);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                // Serialize with Newtonsoft (Indented for readability)
                string dataToStore = JsonConvert.SerializeObject(gameData, Formatting.Indented);
                if (useEncryption)
                {
                    dataToStore = EncryptDecrypt(dataToStore);
                }

                File.WriteAllText(fullPath, dataToStore);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error occurred while trying to save file: {fullPath}\n{e}");
            }
        }

        private string EncryptDecrypt(string data)
        {
            string modifiedString = String.Empty;

            for (int i = 0; i < data.Length; i++)
            {
                modifiedString += (char)(data[i] ^ encryptionKey[i % encryptionKey.Length]);
            }

            return modifiedString;
        }
    }
}