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
        [SerializeField] private TextMeshProUGUI descriptionText;
        public void UpdateUI(ActiveSkill skill)
        {
            _activeSkill = skill;
            if (skill != null)
            {
                // icon.sprite = skill.Icon;
                // skillName.text = skill.Name;
                // cooldownText.text = skill.Cooldown.ToString();
                // descriptionText.text = skill.Description;
            }
            else
            {
                icon.sprite = null;
                skillName.text = string.Empty;
                cooldownText.text = string.Empty;
                descriptionText.text = string.Empty;
            }
        }
    }
}