using EntitySystems.Skill;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UI.Player.Skill
{
    public class HotbarSlotUI : MonoBehaviour, IDropHandler
    {
        [SerializeField] private Image icon;
        [SerializeField] private Image cooldownOverlay;
        [SerializeField] private TextMeshProUGUI hotkeyText;
        [SerializeField] private TextMeshProUGUI cooldownText;
        
        private int _slotIndex;
        private ActiveSkill _activeSkill;
        private PlayerHotbarUI _hotbarUI;
        
        public int SlotIndex => _slotIndex;
        public ActiveSkill ActiveSkill => _activeSkill;
        
        public void Initialize(PlayerHotbarUI hotbarUI, int slotIndex, string hotkeyBinding)
        {
            _hotbarUI = hotbarUI;
            _slotIndex = slotIndex;
            hotkeyText.text = hotkeyBinding;
            UpdateUI(null);
        }
        
        public void UpdateUI(ActiveSkill skill)
        {
            _activeSkill = skill;
            
            if (skill != null)
            {
                icon.sprite = skill.activeSkillInfo.SkillIcon;
                icon.color = Color.white;
            }
            else
            {
                icon.sprite = null;
                icon.color = new Color(0, 0, 0, 0.5f); // Semi-transparent for empty slot
                cooldownOverlay.fillAmount = 0;
                cooldownText.text = string.Empty;
            }
        }
        
        public void UpdateCooldown(float remainingCooldown, float totalCooldown)
        {
            if (remainingCooldown <= 0)
            {
                cooldownOverlay.fillAmount = 0;
                cooldownText.text = string.Empty;
            }
            else
            {
                cooldownOverlay.fillAmount = remainingCooldown / totalCooldown;
                cooldownText.text = Mathf.Ceil(remainingCooldown).ToString();
            }
        }
        
        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
                ActiveSkillSlotUI skillSlot = eventData.pointerDrag.GetComponent<ActiveSkillSlotUI>();
                if (skillSlot != null && skillSlot.ActiveSkill != null)
                {
                    _hotbarUI.EquipSkill(_slotIndex, skillSlot.ActiveSkill);
                }
            }
        }
    }
}