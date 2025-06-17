using System.Collections.Generic;
using EntitySystems.PlayerSystems;
using EntitySystems.Skill;
using GeneralManagers;
using Unity.VisualScripting;
using UnityEngine;

namespace UI.Player.Skill
{
    public class PlayerHotbarUI : MonoBehaviour, ILifecycle<UIManager>
    {
        [SerializeField] private Transform hotbarContainer;
        [SerializeField] private GameObject hotbarSlotPrefab;
        [SerializeField] private int numberOfSlots = 10;
        
        private UIManager _uiManager;
        private PlayerEquippedSkillSystem _equippedSkillSystem;
        private List<HotbarSlotUI> _hotbarSlots = new List<HotbarSlotUI>();
        
        // Map slot indices to keyboard keys
        private readonly string[] _keyBindings = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
        private readonly KeyCode[] _keyCodes = { 
            KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, 
            KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0 
        };
        
        public void Initialize(UIManager parent)
        {
            _uiManager = parent;
            _equippedSkillSystem = GameManager.Instance.PlayerManager.Player.EquippedSkillSystem;
            
            if (_equippedSkillSystem == null)
            {
                Debug.LogError("PlayerEquippedSkillSystem not found on player!");
                return;
            }
            
            _equippedSkillSystem.OnEquippedSkillsChanged += UpdateHotbarUI;
            
            CreateHotbarSlots();
        }
        
        private void CreateHotbarSlots()
        {
            int actualSlots = Mathf.Min(numberOfSlots, _keyBindings.Length);
            
            for (int i = 0; i < actualSlots; i++)
            {
                GameObject slotObj = Instantiate(hotbarSlotPrefab, hotbarContainer);
                HotbarSlotUI slot = slotObj.GetComponent<HotbarSlotUI>();
                
                if (slot != null)
                {
                    slot.Initialize(this, i, _keyBindings[i]);
                    _hotbarSlots.Add(slot);
                }
            }
        }
        
        private void Update()
        {
            // Check for hotkey presses
            for (int i = 0; i < _hotbarSlots.Count && i < _keyCodes.Length; i++)
            {
                if (UnityEngine.Input.GetKeyDown(_keyCodes[i]))
                {
                    TriggerHotbarSkill(i);
                }
            }
        }
        
        private void TriggerHotbarSkill(int slotIndex)
        {
            _equippedSkillSystem?.TriggerSkill(slotIndex);
        }
        
        public void EquipSkill(int slotIndex, ActiveSkill skill)
        {
            _equippedSkillSystem?.EquipSkill(slotIndex, skill);
        }
        
        private void UpdateHotbarUI(Dictionary<int, ActiveSkill> equippedSkills)
        {
            foreach (var slot in _hotbarSlots)
            {
                if (equippedSkills.TryGetValue(slot.SlotIndex, out var skill))
                {
                    slot.UpdateUI(skill);
                }
                else
                {
                    slot.UpdateUI(null);
                }
            }
        }
        
        public void Dispose()
        {
            if (_equippedSkillSystem != null)
                _equippedSkillSystem.OnEquippedSkillsChanged -= UpdateHotbarUI;
                
            _uiManager = null;
            _equippedSkillSystem = null;
            _hotbarSlots.Clear();
        }
    }
}