using EntitySystems.PlayerSystems;
using Item.Inventory;

namespace DataPersistence.Data
{
    public class GameData
    {
        
        public InventorySaveData PlayerInventorySaveData;
        public StatSystemSaveData PlayerStatSystemSaveData;
        public LevelSystemSaveData PlayerLevelSystemSaveData;
        public EquipmentSystemSaveData PlayerEquipmentSystemSaveData;
        public ActiveSkillSystemSaveData PlayerActiveSkillSystemSaveData;
        public PassiveSkillSystemSaveData PlayerPassiveSkillSystemSaveData;
        public EquippedSkillSystemSaveData PlayerEquippedSkillSystemSaveData;
        
        
        public HealthSystemSaveData PlayerHealthSystemSaveData;
        public ManaSystemSaveData PlayerManaSystemSaveData;
        public StaminaSystemSaveData PlayerStaminaSystemSaveData;
        
        public PlayerPositionSaveData PlayerPositionSaveData;
        
        public GameData()
        {
            
        }
    }
}