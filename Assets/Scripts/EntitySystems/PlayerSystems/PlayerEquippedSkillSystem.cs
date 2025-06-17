using System;
using System.Collections.Generic;
using DataPersistence;
using DataPersistence.Data;
using EntitySystems.Skill;
using Entity.Player;
using GeneralManagers;
using UnityEngine;

namespace EntitySystems.PlayerSystems
{
    public class PlayerEquippedSkillSystem : ILifecycle<Player>, IDataPersistence
    {
        private Player _parent;
        private PlayerActiveSkillSystem _activeSkillSystem;

        // Store equipped skills by slot index
        private Dictionary<int, ActiveSkill> _equippedSkills = new Dictionary<int, ActiveSkill>();

        // Number of available hotbar slots
        private readonly int _maxSlots;

        public event Action<Dictionary<int, ActiveSkill>> OnEquippedSkillsChanged;

        public PlayerEquippedSkillSystem(int maxSlots = 10)
        {
            _maxSlots = maxSlots;
        }

        public void Initialize(Player parent)
        {
            _parent = parent;
            _activeSkillSystem = parent.ActiveSkillSystem as PlayerActiveSkillSystem;
            ((IDataPersistence)this).AddDataPersistenceObject();
        }

        public void Dispose()
        {
            _equippedSkills.Clear();
            _parent = null;
            _activeSkillSystem = null;
        }


        public bool EquipSkill(int slotIndex, ActiveSkill skill)
        {
            if (slotIndex < 0 || slotIndex >= _maxSlots)
            {
                Debug.LogError($"Invalid hotbar slot index: {slotIndex}");
                return false;
            }

            _equippedSkills[slotIndex] = skill;
            OnEquippedSkillsChanged?.Invoke(_equippedSkills);

            return true;
        }

        public bool UnequipSkill(int slotIndex)
        {
            if (!_equippedSkills.ContainsKey(slotIndex))
                return false;

            _equippedSkills.Remove(slotIndex);
            OnEquippedSkillsChanged?.Invoke(_equippedSkills);

            return true;
        }

        public ActiveSkill GetEquippedSkill(int slotIndex)
        {
            return _equippedSkills.TryGetValue(slotIndex, out var skill) ? skill : null;
        }

        public bool TriggerSkill(int slotIndex)
        {
            var skill = GetEquippedSkill(slotIndex);
            return skill?.TryUseSkill() ?? false;
        }

        public Dictionary<int, ActiveSkill> GetAllEquippedSkills()
        {
            return new Dictionary<int, ActiveSkill>(_equippedSkills);
        }
        // Existing methods remain the same...

        public void LoadData(GameData saveData)
        {
            if (saveData.PlayerEquippedSkillSystemSaveData != null && _activeSkillSystem != null)
            {
                SaveLoadHelper.LoadFromSaveData(this, saveData.PlayerEquippedSkillSystemSaveData,
                    _activeSkillSystem);
                // Notify UI or other systems of changes
                OnEquippedSkillsChanged?.Invoke(_equippedSkills);
            }
        }

        public void SaveData(ref GameData data)
        {
            data.PlayerEquippedSkillSystemSaveData = SaveLoadHelper.CreateSaveData(this);
        }
    }
}