using EntitySystems.Skill;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UI.Player.Skill
{
    public class ActiveSkillSlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private ActiveSkill _activeSkill;
        public ActiveSkill ActiveSkill => _activeSkill;
        
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI skillName;
        [SerializeField] private TextMeshProUGUI cooldownText;
        [SerializeField] private TextMeshProUGUI vitalStatsCostText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        
        // Drag and drop variables
        private Canvas _canvas;
        private RectTransform _rectTransform;
        private CanvasGroup _canvasGroup;
        private Vector2 _originalPosition;
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
            
            // Find canvas
            Transform parent = transform.parent;
            while (parent != null)
            {
                if (parent.TryGetComponent(out Canvas canvas))
                {
                    _canvas = canvas;
                    break;
                }
                parent = parent.parent;
            }
        }
        
        public void UpdateUI(ActiveSkill skill)
        {
            // Existing code
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
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_activeSkill == null) return;
            
            _originalPosition = _rectTransform.anchoredPosition;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.alpha = 0.6f;
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            if (_activeSkill == null) return;
            
            if (_canvas != null && RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvas.transform as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localPoint))
            {
                _rectTransform.anchoredPosition = localPoint;
            }
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            if (_activeSkill == null) return;
            
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.alpha = 1.0f;
            _rectTransform.anchoredPosition = _originalPosition;
        }
    }
}