using System.Collections.Generic;
using DataPersistence;
using DataPersistence.Data;
using EventBus.Player;
using UnityEngine;

namespace EntitySystems.Skill
{
    public class PlayerActiveSkillSystem : ActiveSkillSystem, IDataPersistence
    {
        public PlayerActiveSkillSystem(List<ActiveSkill> skills) : base(skills)
        {
        }

        public override void Initialize(Entity.Entity parent)
        {
            base.Initialize(parent);
            ((IDataPersistence)this).AddDataPersistenceObject();
            InvokeInitialEvents();
        }

        public override void InvokeInitialEvents()
        {
            base.InvokeInitialEvents();
            PlayerSkillEventSystem.InvokeActiveSkillListChanged(activeSkills);
        }

        public override bool AddSkill(ActiveSkill skill)
        {
            if (base.AddSkill(skill))
            {
                PlayerSkillEventSystem.InvokeActiveSkillListChanged(activeSkills);
                PlayerSkillEventSystem.InvokeActiveSkillGained(skill);
                return true;
            }

            return false;
        }

        public override bool RemoveSkill(string skillName)
        {
            if (base.RemoveSkill(skillName))
            {
                PlayerSkillEventSystem.InvokeActiveSkillListChanged(activeSkills);
                return true;
            }

            return false;
        }

        public void LoadData(GameData saveData)
        {
            if (saveData.PlayerActiveSkillSystemSaveData != null)
            {
                SaveLoadHelper.LoadFromSaveData(this, saveData.PlayerActiveSkillSystemSaveData);
            }
            InvokeInitialEvents();
        }

        public void SaveData(ref GameData data)
        {
            data.PlayerActiveSkillSystemSaveData = SaveLoadHelper.CreateSaveData(this);
        }
    }
}