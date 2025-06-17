using EntitySystems.Stats;

namespace DataPersistence.Data
{
    [System.Serializable]
    public class AbilityStatBoardSaveData
    {
        public int strengthBase;
        public int dexterityBase;
        public int constitutionBase;
        public int intelligenceBase;
        public int wisdomBase;
        public int charismaBase;
    }

    [System.Serializable]
    public class StatSystemSaveData
    {
        public AbilityStatBoardSaveData abilityStats;
        public int unallocatedAbilityStatPoints;
        public AttackStatType preferredAttackStat;
    }
}