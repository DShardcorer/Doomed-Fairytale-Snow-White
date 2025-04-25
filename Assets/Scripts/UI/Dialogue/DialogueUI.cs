using System;
using System.Collections;
using System.Text.RegularExpressions;
using EventSystem.Dialogue;
using Febucci.UI;
using GeneralManagers;
using Helpers;
using Ink.InkLibs.InkRuntime;
using Input;
using TMPro;
using UI.Dialogue.Sprites;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Dialogue
{
    public class DialogueUI : MonoBehaviour
    {
        [Header("Dialogue Box UI")] [SerializeField]
        private TextAnimator_TMP dialogueText;

        [SerializeField] private TypewriterByCharacter typewriter;
        [SerializeField] private GameObject canContinueIcon;
        [SerializeField] private DialogueChoiceButtonUI[] choiceButtons;

        [FormerlySerializedAs("leftSpriteImageAnimator")] [Header("Dialogue Box Sprites")] [SerializeField]
        private Image leftSpriteImage;

        [FormerlySerializedAs("rightSpriteImageAnimator")] [SerializeField]
        private Image rightSpriteImage;

        [Header("Dialogue Box Sprite Database")] [SerializeField]
        private DialogueSpriteDatabase dialogueSpriteDatabase;

        [Header("Speaker Name")] [SerializeField]
        private TextMeshProUGUI speakerNameText;

        [Header("Sound Settings")] [SerializeField]
        private int textTypingSoundInterval = 2;

        [Range(-3, 3)] [SerializeField] private float minPitch = 0.5f;
        [Range(-3, 3)] [SerializeField] private float maxPitch = 1f;
        [SerializeField] private AudioClip[] textTypingSounds;

        private AudioSource _audioSource;
        private Coroutine _displayLineCoroutine;
        private bool _canContinueToNextLine;
        private DialogueEventSystem.DialogueContinueEventArgs currentDialogueEventArgs;
        private bool _isSkippingTypewriter = false;
        private int currentDisplayedLetterIndex = 0;

        private void Awake()
        {
            SubscribeEvents();
            _audioSource = gameObject.AddComponent<AudioSource>();
            typewriter.onCharacterVisible.AddListener(OnCharacterVisible);
            typewriter.onTextShowed.AddListener(OnTypewriterComplete);
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
            typewriter.onCharacterVisible.RemoveListener(OnCharacterVisible);
            typewriter.onTextShowed.RemoveListener(OnTypewriterComplete);
        }

        private void SubscribeEvents()
        {
            DialogueEventSystem.OnEnterDialogue += OnEnterDialogue;
            DialogueEventSystem.OnExitDialogue += OnExitDialogue;
            DialogueEventSystem.OnDialogueContinue += OnDialogueContinue;
            DialogueEventSystem.OnUpdateSpeakerName += OnUpdateSpeakerName;
            DialogueEventSystem.OnUpdateSpeakerSprite += OnUpdateSpeakerSprite;
            GameManager.Instance.InputManager.uiSubmitInputted += OnUISubmitInputted;
        }

        private void UnsubscribeEvents()
        {
            DialogueEventSystem.OnEnterDialogue -= OnEnterDialogue;
            DialogueEventSystem.OnExitDialogue -= OnExitDialogue;
            DialogueEventSystem.OnDialogueContinue -= OnDialogueContinue;
            DialogueEventSystem.OnUpdateSpeakerName -= OnUpdateSpeakerName;
            DialogueEventSystem.OnUpdateSpeakerSprite -= OnUpdateSpeakerSprite;
            GameManager.Instance.InputManager.uiSubmitInputted -= OnUISubmitInputted;
        }

        private void OnEnterDialogue(DialogueEventSystem.EnterDialogueEventArgs args)
        {
            gameObject.SetActive(true);
            dialogueText.textFull = string.Empty;
            speakerNameText.text = string.Empty;
            leftSpriteImage.gameObject.SetActive(false);
            rightSpriteImage.gameObject.SetActive(false);
        }

        private void OnExitDialogue()
        {
            gameObject.SetActive(false);
        }

        private void OnDialogueContinue(DialogueEventSystem.DialogueContinueEventArgs args)
        {
            _canContinueToNextLine = false;
            DialogueEventSystem.InvokeUpdateCanContinueToNextLine(
                new DialogueEventSystem.UpdateCanContinueToNextLineEventArgs(_canContinueToNextLine));

            DisplayLine(args);
        }

        private void DisplayLine(DialogueEventSystem.DialogueContinueEventArgs args)
        {
            HideChoiceButtons();
            currentDisplayedLetterIndex = 0;
            canContinueIcon.SetActive(_canContinueToNextLine);
            currentDialogueEventArgs = args;
            typewriter.ShowText(args.DialogueText);
        }

        private void OnTypewriterComplete()
        {
            StartCoroutine(AllowNextLineCoroutine());
        }

        private IEnumerator AllowNextLineCoroutine()
        {
            yield return new WaitForSeconds(0.3f);
            _canContinueToNextLine = true;
            DialogueEventSystem.InvokeUpdateCanContinueToNextLine(
                new DialogueEventSystem.UpdateCanContinueToNextLineEventArgs(_canContinueToNextLine));
            canContinueIcon.SetActive(_canContinueToNextLine);
            DisplayChoiceButtons(currentDialogueEventArgs);
        }

        private void OnCharacterVisible(char character)
        {
            currentDisplayedLetterIndex++;
            PlayTextTypingSound(currentDisplayedLetterIndex, character);
        }

        private void PlayTextTypingSound(int currentDisplayedLetterIndex, char currentCharacter)
        {
            //play sound if time or textTypingSoundInterval is reached
            if (currentDisplayedLetterIndex % textTypingSoundInterval == 0 ||
                lastTextTypingSoundTime >= textTypingSoundTimeInterval
                )
            {
                lastTextTypingSoundTime = 0;
                int audioClipIndex = currentCharacter.GetHashCode() % textTypingSounds.Length;
                int maxPitchInt = Mathf.FloorToInt(maxPitch * 100);
                int minPitchInt = Mathf.FloorToInt(minPitch * 100);
                int predictablePitchInt = (audioClipIndex % (maxPitchInt - minPitchInt)) + minPitchInt;
                float pitch = (float)predictablePitchInt / 100f;

                _audioSource.pitch = pitch;
                _audioSource.PlayOneShot(textTypingSounds[audioClipIndex]);
            }
        }

        private float textTypingSoundTimeInterval = 0.3f;
        private float lastTextTypingSoundTime = 0;
        private void Update()
        {
            lastTextTypingSoundTime += Time.deltaTime;
            
        }

        private void OnUISubmitInputted(InputEventContext context)
        {
            if (!_canContinueToNextLine && !_isSkippingTypewriter)
            {
                _isSkippingTypewriter = true;
                typewriter.SkipTypewriter();

                // Reset the flag after a short delay
                StartCoroutine(ResetSkippingFlag());
            }
        }

        private IEnumerator ResetSkippingFlag()
        {
            yield return new WaitForSeconds(0.2f);
            _isSkippingTypewriter = false;
        }

        private void OnUpdateSpeakerName(DialogueEventSystem.UpdateSpeakerNameEventArgs args)
        {
            speakerNameText.text = args.SpeakerName;
        }

        private void OnUpdateSpeakerSprite(DialogueEventSystem.UpdateSpeakerSpriteEventArgs args)
        {
            var raw = args.SpeakerSpriteName;
            var parts = raw.Split('_');
            string characterId = parts[0];
            string characterEmotion = parts[1];

            if (args.Layout == "left")
            {
                leftSpriteImage.gameObject.SetActive(true);
                leftSpriteImage.sprite = UnityEngine.Resources.Load<Sprite>(HelperResourcePath.DialogueSpritePath +
                                                                            characterId + "/" +
                                                                            characterEmotion);
                leftSpriteImage.GetComponent<CanvasGroup>().alpha = 1;
                rightSpriteImage.GetComponent<CanvasGroup>().alpha = 0.5f;
            }
            else if (args.Layout == "right")
            {
                rightSpriteImage.gameObject.SetActive(true);
                rightSpriteImage.sprite = UnityEngine.Resources.Load<Sprite>(HelperResourcePath.DialogueSpritePath +
                                                                             characterId + "/" +
                                                                             characterEmotion);
                rightSpriteImage.GetComponent<CanvasGroup>().alpha = 1;
                leftSpriteImage.GetComponent<CanvasGroup>().alpha = 0.5f;
            }
        }

        private void DisplayChoiceButtons(DialogueEventSystem.DialogueContinueEventArgs args)
        {
            for (int inkChoiceIndex = 0; inkChoiceIndex < args.Choices.Count; inkChoiceIndex++)
            {
                Choice choice = args.Choices[inkChoiceIndex];
                DialogueChoiceButtonUI choiceButton = choiceButtons[inkChoiceIndex];

                choiceButton.gameObject.SetActive(true);
                choiceButton.SetChoiceText(choice.text);
                choiceButton.SetChoiceIndex(inkChoiceIndex);

                if (inkChoiceIndex == 0)
                {
                    choiceButton.SelectButton();
                    DialogueEventSystem.InvokeUpdateChoiceIndex(
                        new DialogueEventSystem.UpdateChoiceIndexEventArgs(inkChoiceIndex));
                }
            }
        }

        private void HideChoiceButtons()
        {
            foreach (DialogueChoiceButtonUI button in choiceButtons)
                button.gameObject.SetActive(false);
        }
    }
}