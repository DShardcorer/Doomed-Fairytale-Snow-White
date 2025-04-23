using System.Collections;
using System.Text.RegularExpressions;
using EventSystem.Dialogue;
using GeneralManagers;
using Ink.InkLibs.InkRuntime;
using Input;
using TMPro;
using UnityEngine;

namespace UI.Dialogue
{
    public class DialogueUI : MonoBehaviour
    {
        [Header("Dialogue Box Settings")] [SerializeField]
        private float textSpeed = 0.05f; // Speed of text display

        private float _originalTextSpeed;
        [Header("Dialogue Box UI")]
        // [SerializeField] private GameObject dialogueBoxUI;
        [SerializeField]
        private TextMeshProUGUI dialogueText;

        [SerializeField] private GameObject canContinueIcon;
        [SerializeField] private DialogueChoiceButtonUI[] choiceButtons;

        [Header("Dialogue Box Sprites")] [SerializeField]
        private Animator leftSpriteImageAnimator;

        [SerializeField] private Animator rightSpriteImageAnimator;

        [Header("Speaker Name")] [SerializeField]
        private TextMeshProUGUI speakerNameText;

        [Header("Sound Settings")] [SerializeField]
        private int textTypingSoundInterval = 2; // Interval for playing typing sound

        [Range(-3, 3)] [SerializeField] private float minPitch = 0.5f;

        [Range(-3, 3)] [SerializeField] private float maxPitch = 1f;


        private Coroutine _displayLineCoroutine;
        private bool _canContinueToNextLine;
        private bool _skipTyping;

        [SerializeField] private AudioClip[] textTypingSounds;
        private AudioSource _audioSource;


        private void Awake()
        {
            DialogueEventSystem.OnEnterDialogue += OnEnterDialogue;
            DialogueEventSystem.OnExitDialogue += OnExitDialogue;
            DialogueEventSystem.OnDialogueContinue += OnDialogueContinue;
            DialogueEventSystem.OnUpdateSpeakerName += OnUpdateSpeakerName;
            DialogueEventSystem.OnUpdateSpeakerSprite += OnUpdateSpeakerSprite;
            GameManager.Instance.InputManager.uiSubmitInputted += OnUISubmitInputted;
            _audioSource = gameObject.AddComponent<AudioSource>();
            _originalTextSpeed = textSpeed;
            gameObject.SetActive(false);
        }

        private void OnUISubmitInputted(InputEventContext context)
        {
            _skipTyping = true;
        }

        private void OnUpdateSpeakerSprite(DialogueEventSystem.UpdateSpeakerSpriteEventArgs args)
        {
            string animationSubStateMachineName = args.SpeakerSpriteName.Split('_')[0];
            if (args.Layout == "left")
            {
                leftSpriteImageAnimator.gameObject.SetActive(true);
                leftSpriteImageAnimator.Play(animationSubStateMachineName + "." + args.SpeakerSpriteName);
                leftSpriteImageAnimator.GetComponent<CanvasGroup>().alpha = 1;
                rightSpriteImageAnimator.GetComponent<CanvasGroup>().alpha = 0.5f;
            }
            else if (args.Layout == "right")
            {
                rightSpriteImageAnimator.gameObject.SetActive(true);
                rightSpriteImageAnimator.Play(animationSubStateMachineName + "." + args.SpeakerSpriteName);
                rightSpriteImageAnimator.GetComponent<CanvasGroup>().alpha = 1;
                leftSpriteImageAnimator.GetComponent<CanvasGroup>().alpha = 0.5f;
            }
        }

        private void OnUpdateSpeakerName(DialogueEventSystem.UpdateSpeakerNameEventArgs args)
        {
            speakerNameText.text = args.SpeakerName;
        }

        private void OnEnterDialogue(DialogueEventSystem.EnterDialogueEventArgs args)
        {
            // dialogueBoxUI.SetActive(true);
            gameObject.SetActive(true);
            dialogueText.text = string.Empty;
            speakerNameText.text = string.Empty;
            leftSpriteImageAnimator.gameObject.SetActive(false);
            rightSpriteImageAnimator.gameObject.SetActive(false);
        }

        private void OnExitDialogue()
        {
            dialogueText.text = string.Empty;
            speakerNameText.text = string.Empty;
            gameObject.SetActive(false);
        }

        private void OnDialogueContinue(DialogueEventSystem.DialogueContinueEventArgs args)
        {
            if (_displayLineCoroutine != null)
            {
                StopCoroutine(_displayLineCoroutine);
            }

            _canContinueToNextLine = false;
            DialogueEventSystem.InvokeUpdateCanContinueToNextLine(
                new DialogueEventSystem.UpdateCanContinueToNextLineEventArgs(_canContinueToNextLine));
            _displayLineCoroutine = StartCoroutine(DisplayLine(args));
        }

        private IEnumerator DisplayLine(DialogueEventSystem.DialogueContinueEventArgs args)
        {
            _skipTyping = false;
            HideChoiceButtons();
            canContinueIcon.SetActive(_canContinueToNextLine);
            dialogueText.text = string.Empty;
            bool isAddingRichTextTag = false;
            bool isCollectingTextSpeedTag = false;
            string textSpeedTagValue = "";
            string line = args.DialogueText;
            int currentDisplayedLetterIndex = 0;

            textSpeed = _originalTextSpeed; // Reset to original speed at start of line

            for (int i = 0; i < line.Length; i++)
            {
                char letter = line[i];
                
                if (_skipTyping)
                {
                    dialogueText.text = CleanSelfDefinedRichTextTags(line);
                    break;
                }

                // Check for text speed tag start
                if (i + 12 <= line.Length && line.Substring(i, 11) == "<textSpeed=" && !isAddingRichTextTag)
                {
                    isCollectingTextSpeedTag = true;
                    textSpeedTagValue = "";
                    i += 10; // Skip to the value part
                    continue;
                }
                
                // Collect the speed value
                if (isCollectingTextSpeedTag)
                {
                    if (letter == '>')
                    {
                        isCollectingTextSpeedTag = false;
                        if (float.TryParse(textSpeedTagValue, out float newSpeed))
                        {
                            textSpeed = newSpeed;
                        }
                        continue;
                    }
                    textSpeedTagValue += letter;
                    continue;
                }

                // Check for the text speed tag end
                if (i + 11 <= line.Length && line.Substring(i, 11) == "</textSpeed>" && !isAddingRichTextTag)
                {
                    Debug.Log("Found textSpeed end tag");
                    textSpeed = _originalTextSpeed; // Reset to original speed
                    i += 10; // Skip the closing tag
                    continue;
                }

                // Handle regular rich text tags (existing code)
                if (letter == '<' || isAddingRichTextTag)
                {
                    isAddingRichTextTag = true;
                    dialogueText.text += letter;
                    if (letter == '>')
                    {
                        isAddingRichTextTag = false;
                    }
                }
                else
                {
                    PlayTextTypingSound(currentDisplayedLetterIndex, letter);
                    currentDisplayedLetterIndex++;
                    dialogueText.text += letter;
                    yield return new WaitForSeconds(textSpeed);
                }
            }

            _canContinueToNextLine = true;
            DialogueEventSystem.InvokeUpdateCanContinueToNextLine(
                new DialogueEventSystem.UpdateCanContinueToNextLineEventArgs(_canContinueToNextLine));
            canContinueIcon.SetActive(_canContinueToNextLine);
            DisplayChoiceButtons(args);
        }
        

        private string CleanSelfDefinedRichTextTags(string text)
        {
            // Match <textSpeed=X.Y> tags with any numeric value (including decimals)
            string cleaned = Regex.Replace(text, @"<textSpeed=[\d\.]+>|</textSpeed>", "");
            return cleaned;
        }

        private void PlayTextTypingSound(int currentDisplayedLetterIndex, char currentCharacter)
        {
            if (currentDisplayedLetterIndex % textTypingSoundInterval == 0)
            {
                int audioClipIndex = currentCharacter.GetHashCode() % textTypingSounds.Length;
                int maxPitchInt = Mathf.FloorToInt(maxPitch * 100);
                int minPitchInt = Mathf.FloorToInt(minPitch * 100);
                int predictablePitchInt = (audioClipIndex % (maxPitchInt - minPitchInt)) + minPitchInt;
                float pitch = (float)predictablePitchInt / 100f;
                _audioSource.pitch = pitch;
                _audioSource.PlayOneShot(textTypingSounds[audioClipIndex]);
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
            {
                button.gameObject.SetActive(false);
            }
        }
    }
}