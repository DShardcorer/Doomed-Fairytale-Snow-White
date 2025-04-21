using EntitySystems.Skill;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Player.Skill
{
    public class ActiveSkillSlotUI: MonoBehaviour
    {
        private ActiveSkill _activeSkill;
        public ActiveSkill ActiveSkill=> _activeSkill;
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI skillName;
        [SerializeField] private TextMeshProUGUI cooldownText;
        [SerializeField] private TextMeshProUGUI vitalStatsCostText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        public void UpdateUI(ActiveSkill skill)
        {
            _activeSkill = skill;
            if (skill != null)
            {
                icon.sprite = skill.activeSkillInfo.SkillIcon;
                skillName.text = skill.activeSkillInfo.SkillName;
                cooldownText.text = "Cooldown: " + skill.activeSkillInfo.cooldown.ToString() + "s";
                descriptionText.text = skill.activeSkillInfo.SkillDescription;
                vitalStatsCostText.text = "Consume:";
                if (skill.activeSkillInfo.healthCost != 0)
                {
                    vitalStatsCostText.text += " HP:" + skill.activeSkillInfo.healthCost.ToString();
                }

                if (skill.activeSkillInfo.manaCost != 0)
                {
                    vitalStatsCostText.text += " MP:" + skill.activeSkillInfo.manaCost.ToString();
                }
                if (skill.activeSkillInfo.staminaCost != 0)
                {
                    vitalStatsCostText.text += " STA:" + skill.activeSkillInfo.staminaCost.ToString();
                }
            }
            else
            {
                icon.sprite = null;
                skillName.text = string.Empty;
                cooldownText.text = string.Empty;
                descriptionText.text = string.Empty;
                vitalStatsCostText.text = string.Empty;
            }
        }
    }
}