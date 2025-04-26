using System;
using EntitySystems.Skill;
using EntitySystems.Stats;
using UnityEngine;

namespace Entity.Player._PlayerCreation
{
    public class PlayerCreation : MonoBehaviour
    {
        private PlayerStartingInfo _playerStartingInfo;
        private AbilityStatboardSO _playerAbilityStatboardSo;


        private void Awake()
        {
            _playerStartingInfo = new PlayerStartingInfo();
            _playerStartingInfo.abilityStatboardSo = ScriptableObject.CreateInstance<AbilityStatboardSO>();
            _playerAbilityStatboardSo = _playerStartingInfo.abilityStatboardSo;
            _playerAbilityStatboardSo.name = "PlayerAbilityStatboard";

            IninializeBaseStats();
        }

        private void IninializeBaseStats()
        {
            //set all stats to 5
            _playerAbilityStatboardSo.Strength = 5;
            _playerAbilityStatboardSo.Dexterity = 5;
            _playerAbilityStatboardSo.Constitution = 5;
            _playerAbilityStatboardSo.Intelligence = 5;
            _playerAbilityStatboardSo.Wisdom = 5;
            _playerAbilityStatboardSo.Charisma = 5;
        }

        public void IncreaseStatPoint(StatType statType, int amount)
        {
            switch (statType)
            {
                case StatType.Strength:
                    _playerAbilityStatboardSo.Strength += amount;
                    break;
                case StatType.Dexterity:
                    _playerAbilityStatboardSo.Dexterity += amount;
                    break;
                case StatType.Constitution:
                    _playerAbilityStatboardSo.Constitution += amount;
                    break;
                case StatType.Intelligence:
                    _playerAbilityStatboardSo.Intelligence += amount;
                    break;
                case StatType.Wisdom:
                    _playerAbilityStatboardSo.Wisdom += amount;
                    break;
                case StatType.Charisma:
                    _playerAbilityStatboardSo.Charisma += amount;
                    break;
            }
        }

        public void DecreaseStatPoint(StatType statType, int amount)
        {
            switch (statType)
            {
                case StatType.Strength:
                    _playerAbilityStatboardSo.Strength -= amount;
                    break;
                case StatType.Dexterity:
                    _playerAbilityStatboardSo.Dexterity -= amount;
                    break;
                case StatType.Constitution:
                    _playerAbilityStatboardSo.Constitution -= amount;
                    break;
                case StatType.Intelligence:
                    _playerAbilityStatboardSo.Intelligence -= amount;
                    break;
                case StatType.Wisdom:
                    _playerAbilityStatboardSo.Wisdom -= amount;
                    break;
                case StatType.Charisma:
                    _playerAbilityStatboardSo.Charisma -= amount;
                    break;
            }
        }

        public void AddActiveSkill(string ActiveSkillId)
        {
            _playerStartingInfo.startingActiveSkillIds.Add(ActiveSkillId);
        }

        public void AddPassiveSkill(string PassiveSkillId)
        {
            _playerStartingInfo.startingPassiveSkillIds.Add(PassiveSkillId);
        }

        public void AddInventoryItem(string InventoryItemId, int amount)
        {
            _playerStartingInfo.startingInventoryItems.Add(new PlayerStartingInfo.startingInventoryItem
                { itemId = InventoryItemId, stackSize = amount });
        }
    }
}