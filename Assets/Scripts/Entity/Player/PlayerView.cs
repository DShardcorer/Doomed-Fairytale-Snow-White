using DataPersistence;
using DataPersistence.Data;
using GeneralManagers;
using SceneSwitch;
using UnityEngine;

namespace Entity.Player
{
    public class PlayerView : EntityView, IDataPersistence
    {
        private Player _player;
        public Player Player => _player;

        public void Initialize(Player controller)
        {
            base.Initialize(controller);
            _player = controller;
            ((IDataPersistence)this).AddDataPersistenceObject();
        }

        public void LoadData(GameData saveData)
        {
            if (saveData.PlayerPositionSaveData != null)
            {
                SaveLoadHelper.LoadFromSaveData(this, saveData.PlayerPositionSaveData,
                   SceneSwitchManager.Instance);
            }
        }

        public void SaveData(ref GameData data)
        {
            data.PlayerPositionSaveData = SaveLoadHelper.CreateSaveData(this, 
                SceneSwitchManager.Instance);
        }
    }
}