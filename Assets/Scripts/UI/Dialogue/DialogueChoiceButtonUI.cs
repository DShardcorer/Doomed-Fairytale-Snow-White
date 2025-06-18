using System;
using EventBus.Dialogue;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

namespace UI.Dialogue
{
    public class DialogueChoiceButtonUI : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [Header("Components")]
        [SerializeField] private Button choiceButton;
        [SerializeField] private TextMeshProUGUI choiceText;
        [SerializeField] private GameObject selectedArrow;

        [Header("Animation Settings")]
        [SerializeField] private float moveAmount = 10f;
        [SerializeField] private float moveDuration = 0.5f;

        private int choiceIndex = -1;
        private Tween arrowTween;
        private Vector2 arrowOriginalPos;

        public int ChoiceIndex => choiceIndex;

        private void Awake()
        {
            // Cache original arrow position and hide arrow
            RectTransform arrowTransform = selectedArrow.GetComponent<RectTransform>();
            arrowOriginalPos = arrowTransform.anchoredPosition;
            selectedArrow.SetActive(false);
        }

        public void OnSelect(BaseEventData eventData)
        {
            // Show the arrow and start the animation
            selectedArrow.SetActive(true);
            StartArrowAnimation();

            // Notify the dialogue system
            DialogueEventSystem.InvokeUpdateChoiceIndex(
                new DialogueEventSystem.UpdateChoiceIndexEventArgs(choiceIndex)
            );
        }

        public void OnDeselect(BaseEventData eventData)
        {
            // Hide arrow and stop animation
            StopArrowAnimation();
            selectedArrow.SetActive(false);
        }

        private void OnDisable()
        {
            selectedArrow.SetActive(false);
        }

        private void StartArrowAnimation()
        {
            // Kill any existing tween
            if (arrowTween != null && arrowTween.IsActive())
                arrowTween.Kill();

            // Animate arrow moving left and right indefinitely
            RectTransform arrowTransform = selectedArrow.GetComponent<RectTransform>();

            arrowTween = arrowTransform
                .DOAnchorPosX(arrowOriginalPos.x + moveAmount, moveDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }

        private void StopArrowAnimation()
        {
            if (arrowTween != null && arrowTween.IsActive())
            {
                arrowTween.Kill();
                arrowTween = null;
            }

            // Reset arrow to its original position
            RectTransform arrowTransform = selectedArrow.GetComponent<RectTransform>();
            arrowTransform.anchoredPosition = arrowOriginalPos;
        }

        public void SelectButton()
        {
            choiceButton.Select();
        }

        public void SetChoiceText(string text)
        {
            choiceText.text = text;
        }

        public void SetChoiceIndex(int index)
        {
            choiceIndex = index;
        }
    }
}
