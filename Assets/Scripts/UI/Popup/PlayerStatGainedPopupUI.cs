using System;
using DG.Tweening;
using EventBus.Player;
using UnityEngine;

namespace UI.Popup
{
    public class PlayerStatGainedPopupUI : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI _statGainedText;
        private RectTransform _rectTransform;
        private Tween _popupTween;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            PlayerStatsEventSystem.OnStatPointGained += OnStatPointGained;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            // Unsubscribe from any events if necessary
            PlayerStatsEventSystem.OnStatPointGained -= OnStatPointGained;
        }

        private void OnStatPointGained(PlayerStatsEventSystem.StatPointGainedEventArgs obj)
        {
            ShowPopup(obj.StatType.ToString(), obj.Amount);
        }

        public void ShowPopup(string statName, int amount)
        {
            _statGainedText.text = "Gained <b>" + amount + "</b> <b>" + statName + "</b> !";
            gameObject.SetActive(true);

            // Kill any running tween first
            _popupTween?.Kill();

            // Start fully off-screen to the left
            _rectTransform.anchoredPosition =
                new Vector2(-_rectTransform.rect.width, _rectTransform.anchoredPosition.y);

            // Slide in
            _popupTween = _rectTransform
                .DOAnchorPosX(20f, 0.5f) // Adjust 20f based on desired visible position
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    // Slide out after a delay
                    DOVirtual.DelayedCall(1f, () =>
                    {
                        _popupTween = _rectTransform
                            .DOAnchorPosX(-_rectTransform.rect.width, 0.5f) // Slide out to the left
                            .SetEase(Ease.InBack)
                            .OnComplete(() => gameObject.SetActive(false));
                    });
                });
        }
    }
}