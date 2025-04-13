using DataPersistence.Data;

namespace DataPersistence
{
    public interface IDataPersistence
    {
        public void AddDataPersistenceObject()
        {
            DataPersistenceManager.Instance.AddDataPersistenceObject(this);
        }
        public void LoadData(GameData saveData);
        public void SaveData(ref GameData data);
    }
}