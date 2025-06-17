using System.Collections.Generic;
using DataPersistence;
using DataPersistence.Data;

namespace EntitySystems.Skill
{
    public class PlayerPassiveSkillSystem : PassiveSkillSystem, IDataPersistence
    {
        public PlayerPassiveSkillSystem(List<PassiveSkill> skills) : base(skills)
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
            EventSystem.Player.PlayerSkillEventSystem.InvokePassiveSkillListChanged(passiveSkills);
        }

        public override bool AddSkill(PassiveSkill skill)
        {
            if (base.AddSkill(skill))
            {
                EventSystem.Player.PlayerSkillEventSystem.InvokePassiveSkillListChanged(passiveSkills);
                EventSystem.Player.PlayerSkillEventSystem.InvokePassiveSkillGained(skill);
                return true;
            }
            return false;
        }

        public override bool RemoveSkill(PassiveSkill skill)
        {
            if (base.RemoveSkill(skill))
            {
                EventSystem.Player.PlayerSkillEventSystem.InvokePassiveSkillListChanged(passiveSkills);
                return true;
            }
            return false;
        }

        public void LoadData(GameData saveData)
        {
            if (saveData.PlayerPassiveSkillSystemSaveData != null)
            {
                SaveLoadHelper.LoadFromSaveData(this, saveData.PlayerPassiveSkillSystemSaveData);
            }
            InvokeInitialEvents();
        }

        public void SaveData(ref GameData data)
        {
            data.PlayerPassiveSkillSystemSaveData = SaveLoadHelper.CreateSaveData(this);
        }
    }
}