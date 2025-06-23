using EntitySystems.Stats;
using UnityEngine;

namespace Item
{
    [CreateAssetMenu(fileName = "Consumable Recovery ItemDataSO", menuName = "ItemData/Consumable Recovery")]
    public class ItemDataSOConsumableRecovery: ItemDataSOConsumable
    {
        public VitalStatType recoveryVitalStatType;
        public float recoveryAmount = 100f;
        public float recoveryDuration = 10f; // Duration in seconds
        public override void UseItem(EntityBase.Entity entity)
        {
            switch (recoveryVitalStatType)
            {
                case VitalStatType.Health:
                    entity.HealthSystem.RecoverOvertime(recoveryAmount, recoveryDuration);
                    break;
                case VitalStatType.Mana:
                    entity.ManaSystem.RecoverOvertime(recoveryAmount, recoveryDuration);
                    break;
                case VitalStatType.Stamina:
                    entity.StaminaSystem.RecoverOvertime(recoveryAmount, recoveryDuration);
                    break;
            }
        }
    }
}