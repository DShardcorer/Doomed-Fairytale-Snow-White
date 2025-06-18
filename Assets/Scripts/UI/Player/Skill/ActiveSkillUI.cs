using System.Collections.Generic;
using EntitySystems.Skill;
using EventBus.Player;
using GeneralManagers;
using Helpers;
using Pool;
using UnityEngine;

namespace UI.Player.Skill
{
    public class ActiveSkillUI : MonoBehaviour, ILifecycle<PlayerSkillUI>
    {
        private PlayerSkillUI _playerSkillUI;
        private List<ActiveSkillSlotUI> activeSkillSlots = new List<ActiveSkillSlotUI>();
        private PoolManager _poolManager;

        public void Initialize(PlayerSkillUI parent)
        {
            _playerSkillUI = parent;
            _poolManager = GameManager.Instance.PoolManager;
            PlayerSkillEventSystem.OnActiveSkillListChanged += PlayerSkillEventSystem_OnActiveSkillListChanged;
        }

        private void PlayerSkillEventSystem_OnActiveSkillListChanged(PlayerSkillEventSystem.ActiveSkillListChangedEventArgs obj)
        {
            Debug.Log("PlayerSkillEventSystem_OnActiveSkillListChanged");
            DisplaySkills(obj.activeSkills);
        }

        public void Dispose()
        {
            _playerSkillUI = null;
            _poolManager = null;
            PlayerSkillEventSystem.OnActiveSkillListChanged -= PlayerSkillEventSystem_OnActiveSkillListChanged;
        }

        public void DisplaySkills(List<ActiveSkill> skills)
        {
            AdjustActiveSkillSlotsCount(skills.Count);
            for (int i = 0; i < skills.Count; i++)
            {
                activeSkillSlots[i].UpdateUI(skills[i]);
            }
        }

        private void AdjustActiveSkillSlotsCount(int count)
        {
            while (activeSkillSlots.Count < count)
            {
                ActiveSkillSlotUI newSlot = _poolManager.GetObject(HelperUIName.ActiveSkillSlotUI)
                    .GetComponent<ActiveSkillSlotUI>();
                if (newSlot == null)
                {
                    Debug.LogError("Failed to get ActiveSkillSlotUI from pool.");
                    return;
                }
                newSlot.transform.SetParent(transform, false);
                activeSkillSlots.Add(newSlot);
            }

            while (activeSkillSlots.Count > count)
            {
                ActiveSkillSlotUI slotToRemove = activeSkillSlots[activeSkillSlots.Count - 1];
                activeSkillSlots.RemoveAt(activeSkillSlots.Count - 1);
                _poolManager.ReturnObject(HelperUIName.ActiveSkillSlotUI, slotToRemove.gameObject);
            }
        }
    }
}
