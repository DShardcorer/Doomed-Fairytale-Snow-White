using EntitySystems.PlayerSystems;
using Item.Inventory;

namespace DataPersistence.Data
{
    public class GameData
    {
        //Currently testing out the saving system so we will only work with saving the stamina point for now
        public float stamina;
        
        public InventorySaveData playerInventorySaveData;
        public GameData()
        {
            
        }
    }
}