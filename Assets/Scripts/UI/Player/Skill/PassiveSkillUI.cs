using System.Collections.Generic;
using EntitySystems.Skill;
using EventBus.Player;
using GeneralManagers;
using Helpers;
using Pool;
using UnityEngine;

namespace UI.Player.Skill
{
    public class PassiveSkillUI : MonoBehaviour, ILifecycle<PlayerSkillUI>
    {
        private PlayerSkillUI _playerSkillUI;
        private List<PassiveSkillSlotUI> passiveSkillSlots = new List<PassiveSkillSlotUI>();
        private PoolManager _poolManager;

        public void Initialize(PlayerSkillUI parent)
        {
            _playerSkillUI = parent;
            _poolManager = GameManager.Instance.PoolManager;
            PlayerSkillEventSystem.OnPassiveSkillListChanged += PlayerSkillEventSystem_OnPassiveSkillListChanged;
        }

        private void PlayerSkillEventSystem_OnPassiveSkillListChanged(PlayerSkillEventSystem.PassiveSkillListChangedEventArgs obj)
        {
            Debug.Log("PlayerSkillEventSystem_OnPassiveSkillListChanged");
            DisplaySkills(obj.passiveSkills);
        }

        public void Dispose()
        {
            _playerSkillUI = null;
            _poolManager = null;
            PlayerSkillEventSystem.OnPassiveSkillListChanged -= PlayerSkillEventSystem_OnPassiveSkillListChanged;
        }

        public void DisplaySkills(List<PassiveSkill> skills)
        {
            AdjustPassiveSkillSlotsCount(skills.Count);
            for (int i = 0; i < skills.Count; i++)
            {
                passiveSkillSlots[i].UpdateUI(skills[i]);
            }
        }

        private void AdjustPassiveSkillSlotsCount(int count)
        {
            while (passiveSkillSlots.Count < count)
            {
                PassiveSkillSlotUI newSlot = _poolManager.GetObject(HelperUIName.PassiveSkillSlotUI)
                    .GetComponent<PassiveSkillSlotUI>();
                if (newSlot == null)
                {
                    Debug.LogError("Failed to get PassiveSkillSlotUI from pool.");
                    return;
                }
                newSlot.transform.SetParent(transform, false);
                passiveSkillSlots.Add(newSlot);
            }

            while (passiveSkillSlots.Count > count)
            {
                PassiveSkillSlotUI slotToRemove = passiveSkillSlots[passiveSkillSlots.Count - 1];
                passiveSkillSlots.RemoveAt(passiveSkillSlots.Count - 1);
                _poolManager.ReturnObject(HelperUIName.PassiveSkillSlotUI, slotToRemove.gameObject);
            }
        }
        
    }
}