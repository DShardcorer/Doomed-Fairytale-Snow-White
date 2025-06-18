using DG.Tweening;
using EntitySystems.Skill;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.Popup
{
    public class PlayerSkillGainedPopupUI: MonoBehaviour
    {

        [SerializeField] private TMPro.TextMeshProUGUI _skillGainedText;
        private RectTransform _rectTransform;
        private Tween _popupTween;


        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();

            EventBus.Player.PlayerSkillEventSystem.OnActiveSkillGained += OnActiveSkillGained;
            EventBus.Player.PlayerSkillEventSystem.OnPassiveSkillGained += OnPassiveSkillGained;

            gameObject.SetActive(false);
        }


        private void OnDestroy()
        {
            EventBus.Player.PlayerSkillEventSystem.OnActiveSkillGained -= OnActiveSkillGained;
            EventBus.Player.PlayerSkillEventSystem.OnPassiveSkillGained -= OnPassiveSkillGained;
        }
        

        private void OnActiveSkillGained(EventBus.Player.PlayerSkillEventSystem.ActiveSkillGainedEventArgs args)
        {
            Debug.LogWarning("Active skill gained: " + args.activeSkill.activeSkillInfo.SkillName);
            ShowPopup(args.activeSkill.activeSkillInfo);
        }

        private void OnPassiveSkillGained(EventBus.Player.PlayerSkillEventSystem.PassiveSkillGainedEventArgs args)
        {
            Debug.LogWarning("Passive skill gained: " + args.passiveSkill.SkillInfo.SkillName);
            ShowPopup(args.passiveSkill.SkillInfo);
        }

        private void ShowPopup(SkillInfoSO skillInfo)
        {
            _skillGainedText.text = "Gained <b>" + skillInfo.SkillName + "</b> skill !";
            gameObject.SetActive(true);

            // Kill any running tween first
            _popupTween?.Kill();

            // Start fully off-screen to the left
            _rectTransform.anchoredPosition = new Vector2(-_rectTransform.rect.width, _rectTransform.anchoredPosition.y);

            // Slide in
            _popupTween = _rectTransform
                .DOAnchorPosX(20f, 0.5f) // Adjust 20f based on desired visible position
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    // Wait 2.5 seconds, then slide back
                    _popupTween = DOVirtual.DelayedCall(2.5f, () =>
                    {
                        _popupTween = _rectTransform
                            .DOAnchorPosX(-_rectTransform.rect.width, 0.5f)
                            .SetEase(Ease.InBack)
                            .OnComplete(() =>
                            {
                                gameObject.SetActive(false);
                            });
                    });
                });
        }

        private void OnDisable()
        {
            _popupTween?.Kill();
            _popupTween = null;
        }


        
    }
}