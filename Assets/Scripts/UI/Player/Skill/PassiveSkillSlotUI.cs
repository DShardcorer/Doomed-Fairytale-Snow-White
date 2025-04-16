using EntitySystems.Skill;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Player.Skill
{
    public class PassiveSkillSlotUI : MonoBehaviour
    {
        private PassiveSkill _passiveSkill;
        public PassiveSkill PassiveSkill => _passiveSkill;
        [SerializeField] private Image _icon;
        [SerializeField] private TMPro.TextMeshProUGUI _skillName;
        [SerializeField] private TMPro.TextMeshProUGUI _descriptionText;

        public void UpdateUI(PassiveSkill skill)
        {
            _passiveSkill = skill;
            if (skill != null)
            {
                _icon.sprite = skill.SkillInfo.SkillIcon;
                _skillName.text = skill.SkillInfo.SkillName;
                _descriptionText.text = skill.SkillInfo.SkillDescription;
            }
            else
            {
                _icon = null;
                _skillName.text = string.Empty;
                _descriptionText.text = string.Empty;
            }
        }
        
    }
}