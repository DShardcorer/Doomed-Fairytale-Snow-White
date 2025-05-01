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

        [SerializeField] private GameObject dialogueHolder;

        [SerializeField] private TypewriterByCharacter typewriter;
        [SerializeField] private GameObject canContinueIcon;
        [SerializeField] private DialogueChoiceButtonUI[] choiceButtons;

        [FormerlySerializedAs("leftSpriteImageAnimator")] [Header("Dialogue Box Sprites")] [SerializeField]
        private Image leftSpriteImage;

        [FormerlySerializedAs("rightSpriteImageAnimator")] [SerializeField]
        private Image rightSpriteImage;

        [SerializeField] private GameObject backCGLayer;
        [SerializeField] private GameObject frontCGLayer;
        [SerializeField] private GameObject mainCGLayer;

        private string cgPath;

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
            mainCGLayer.gameObject.SetActive(false);
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
            DialogueEventSystem.OnUpdateCG += OnUpdateCG;
            DialogueEventSystem.OnUpdateCGPath += OnUpdateCGPath;
            GameManager.Instance.InputManager.uiSubmitInputted += OnUISubmitInputted;
        }

        private void UnsubscribeEvents()
        {
            DialogueEventSystem.OnEnterDialogue -= OnEnterDialogue;
            DialogueEventSystem.OnExitDialogue -= OnExitDialogue;
            DialogueEventSystem.OnDialogueContinue -= OnDialogueContinue;
            DialogueEventSystem.OnUpdateSpeakerName -= OnUpdateSpeakerName;
            DialogueEventSystem.OnUpdateSpeakerSprite -= OnUpdateSpeakerSprite;
            DialogueEventSystem.OnUpdateCG -= OnUpdateCG;
            DialogueEventSystem.OnUpdateCGPath -= OnUpdateCGPath;
            GameManager.Instance.InputManager.uiSubmitInputted -= OnUISubmitInputted;
        }

        private void OnUpdateCGPath(DialogueEventSystem.UpdateCGPathEventArgs obj)
        {
            cgPath = obj.CGPath;

            //Destroy children of back and front
            foreach (Transform child in backCGLayer.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (Transform child in frontCGLayer.transform)
            {
                Destroy(child.gameObject);
            }

            // Load prefabs
            GameObject backPrefab =
                UnityEngine.Resources.Load<GameObject>(HelperResourcePath.CGPath + cgPath + "/Back");
            GameObject frontPrefab =
                UnityEngine.Resources.Load<GameObject>(HelperResourcePath.CGPath + cgPath + "/Front");

            // Handle back layer
            if (backPrefab)
            {
                backCGLayer = Instantiate(backPrefab, backCGLayer.transform);
                backCGLayer.SetActive(true);
            }
            else
            {
                backCGLayer.SetActive(false);
                Debug.Log("No backCgLayer found at " + HelperResourcePath.CGPath + cgPath + "/Back");
            }

            // Handle front layer
            if (frontPrefab)
            {
                frontCGLayer = Instantiate(frontPrefab, frontCGLayer.transform);
                frontCGLayer.SetActive(true);
            }
            else
            {
                frontCGLayer.SetActive(false);
                Debug.Log("No frontCgLayer found at " + HelperResourcePath.CGPath + cgPath + "/Front");
            }
        }

        private void OnUpdateCG(DialogueEventSystem.UpdateCGEventArgs obj)
        {
            if (String.Equals("null", obj.CGName))
            {
                mainCGLayer.gameObject.SetActive(false);
            }
            else
            {
                mainCGLayer.gameObject.SetActive(true);
                GameObject cgPrefab =
                    UnityEngine.Resources.Load<GameObject>(HelperResourcePath.CGPath + cgPath + "/" + obj.CGName);
                if (cgPrefab)
                {
                    foreach (Transform child in mainCGLayer.transform)
                    {
                        Destroy(child.gameObject);
                    }

                    GameObject cg = Instantiate(cgPrefab, mainCGLayer.transform);
                    cg.SetActive(true);
                }
                else
                {
                    mainCGLayer.gameObject.SetActive(false);
                    Debug.Log("No CG found at " + HelperResourcePath.CGPath + cgPath + "/" + obj.CGName);
                }
            }
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
            if (args.Delay != 0)
            {
                StartCoroutine(DelayedDisplayLine(args));
            }
            else
            {
                DisplayLine(args);
            }
        }

        private IEnumerator DelayedDisplayLine(DialogueEventSystem.DialogueContinueEventArgs args)
        {
            dialogueHolder.SetActive(false);
            yield return new WaitForSeconds(args.Delay);
            dialogueHolder.SetActive(true);
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