using System;
using System.Collections;
using System.Collections.Generic;
using AssetManagement;
using DG.Tweening;
using EventSystem.Dialogue;
using Febucci.UI;
using GeneralManagers;
using Helpers;
using Ink.InkLibs.InkRuntime;
using Input;
using Pool;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UI.Dialogue
{
    public class DialogueUI : MonoBehaviour
    {
        [Header("Dialogue Box UI")] [SerializeField]
        private TextAnimator_TMP dialogueText;

        [SerializeField] private GameObject dialogueHolder;

        [SerializeField] private TypewriterByCharacter typewriter;
        [SerializeField] private GameObject canContinueIcon;
        private Vector2 canContinueIconOriginalPosition;

        [Header("Choice Buttons")] [SerializeField]
        private GameObject ChoiceButtonsHolder;

        private List<DialogueChoiceButtonUI> choiceButtons = new List<DialogueChoiceButtonUI>();

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
        private bool _dialogePaused = false;
        private PoolManager _poolManager;


        private void Awake()
        {
            SubscribeEvents();
            _audioSource = gameObject.AddComponent<AudioSource>();
            typewriter.onCharacterVisible.AddListener(OnCharacterVisible);
            typewriter.onTextShowed.AddListener(OnTypewriterComplete);
            mainCGLayer.gameObject.SetActive(false);
            gameObject.SetActive(false);

            // Cache the original position of the continue icon
            if (canContinueIcon != null)
            {
                canContinueIconOriginalPosition = canContinueIcon.GetComponent<RectTransform>().anchoredPosition;
            }
        }

        private void Start()
        {
            _poolManager = GameManager.Instance.PoolManager;
            //if pool manager is null, start a coroutine to wait for it to be initialized
            if (_poolManager == null)
            {
                StartCoroutine(WaitForPoolManager());
            }
        }

        private IEnumerator WaitForPoolManager()
        {
            while (_poolManager == null)
            {
                yield return null;
                _poolManager = GameManager.Instance.PoolManager;
            }
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
            DialogueEventSystem.OnUpdateCGBack += OnUpdateCGBack;
            DialogueEventSystem.OnUpdateCGFront += OnUpdateCGFront;
            DialogueEventSystem.OnUpdateCGPath += OnUpdateCGPath;
            DialogueEventSystem.OnPauseDialogue += OnPauseDialogue;
            DialogueEventSystem.OnResumeDialogue += OnResumeDialogue;
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
            DialogueEventSystem.OnUpdateCGBack -= OnUpdateCGBack;
            DialogueEventSystem.OnUpdateCGFront -= OnUpdateCGFront;
            DialogueEventSystem.OnUpdateCGPath -= OnUpdateCGPath;
            DialogueEventSystem.OnPauseDialogue -= OnPauseDialogue;
            DialogueEventSystem.OnResumeDialogue -= OnResumeDialogue;
            GameManager.Instance.InputManager.uiSubmitInputted -= OnUISubmitInputted;
        }

        #region CGs

        private async void OnUpdateCGPath(DialogueEventSystem.UpdateCGPathEventArgs obj)
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
            // GameObject backPrefab =
            //     UnityEngine.Resources.Load<GameObject>(HelperResourcePath.CGPath + cgPath + "/Back");
            // GameObject frontPrefab =
            //     UnityEngine.Resources.Load<GameObject>(HelperResourcePath.CGPath + cgPath + "/Front");
            backCGLayer =
                await AddressablesManager.Instance.LoadAndInstantiate(HelperAddressablesGroup.CGs +
                    cgPath +
                    "/Back"
                    +HelperExtension.PREFAB
                    , backCGLayer.transform);

            frontCGLayer =
                await AddressablesManager.Instance.LoadAndInstantiate(HelperAddressablesGroup.CGs +
                    cgPath +
                    "/Front"
                    +HelperExtension.PREFAB
                    , frontCGLayer.transform);
            // Handle back layer
            if (backCGLayer)
            {
                backCGLayer.SetActive(true);
            }
            else
            {
                backCGLayer.SetActive(false);
                Debug.Log("No backCgLayer found at " + HelperResourcePath.CGPath + cgPath + "/Back");
            }

            if (frontCGLayer)
            {
                frontCGLayer.SetActive(true);
            }
            else
            {
                frontCGLayer.SetActive(false);
                Debug.Log("No frontCgLayer found at " + HelperResourcePath.CGPath + cgPath + "/Front");
            }
        }

        private async void OnUpdateCG(DialogueEventSystem.UpdateCGEventArgs obj)
        {
            if (String.Equals("null", obj.CGName))
            {
                mainCGLayer.gameObject.SetActive(false);
            }
            else
            {
                mainCGLayer.gameObject.SetActive(true);
                foreach (Transform child in mainCGLayer.transform)
                {
                    Destroy(child.gameObject);
                }

                // GameObject cgPrefab =
                //     UnityEngine.Resources.Load<GameObject>(HelperResourcePath.CGPath + cgPath + "/" + obj.CGName);
                GameObject cg = await AddressablesManager.Instance.LoadAndInstantiate(
                    HelperAddressablesGroup.CGs + cgPath + "/" + obj.CGName + HelperExtension.PREFAB, mainCGLayer.transform);

                if (cg)
                {
                    cg.SetActive(true);
                }
                else
                {
                    mainCGLayer.gameObject.SetActive(false);
                    Debug.LogError("No CG found at " + HelperResourcePath.CGPath + cgPath + "/" + obj.CGName + HelperExtension.PREFAB);
                }
            }
        }

        private async void OnUpdateCGFront(DialogueEventSystem.UpdateCGFrontEventArgs obj)
        {
            if (String.Equals("null", obj.CGFrontName))
            {
                frontCGLayer.gameObject.SetActive(false);
            }
            else
            {
                foreach (Transform child in frontCGLayer.transform)
                {
                    Destroy(child.gameObject);
                }

                frontCGLayer.gameObject.SetActive(true);
                // GameObject cgPrefab =
                //     UnityEngine.Resources.Load<GameObject>(HelperResourcePath.CGPath + cgPath + "/" + obj.CGFrontName);

                GameObject cg = await AddressablesManager.Instance.LoadAndInstantiate(
                    HelperAddressablesGroup.CGs + cgPath + "/" + obj.CGFrontName+ HelperExtension.PREFAB, frontCGLayer.transform);
                if (cg)
                {
                    cg.SetActive(true);
                }
                else
                {
                    frontCGLayer.gameObject.SetActive(false);
                    Debug.LogError("No CG found at " + HelperResourcePath.CGPath + cgPath + "/" + obj.CGFrontName+ HelperExtension.PREFAB);
                }
            }
        }

        private async void OnUpdateCGBack(DialogueEventSystem.UpdateCGBackEventArgs obj)
        {
            if (String.Equals("null", obj.CGBackName))
            {
                backCGLayer.gameObject.SetActive(false);
            }
            else
            {
                foreach (Transform child in backCGLayer.transform)
                {
                    Destroy(child.gameObject);
                }

                backCGLayer.gameObject.SetActive(true);
                // GameObject cgPrefab =
                //     UnityEngine.Resources.Load<GameObject>(HelperResourcePath.CGPath + cgPath + "/" + obj.CGBackName);
                GameObject cg = await AddressablesManager.Instance.LoadAndInstantiate(
                    HelperAddressablesGroup.CGs + cgPath + "/" + obj.CGBackName + HelperExtension.PREFAB, backCGLayer.transform);
                if (cg)
                {
                    cg.SetActive(true);
                }
                else
                {
                    backCGLayer.gameObject.SetActive(false);
                    Debug.LogError("No CG found at " + HelperResourcePath.CGPath + cgPath + "/" + obj.CGBackName);
                }
            }
        }

        #endregion


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
            else if (!_dialogePaused)
            {
                DisplayLine(args);
            }
            else
            {
                StartCoroutine(PausedDisplayLine(args));
            }
        }

        public void OnPauseDialogue()
        {
            _dialogePaused = true;
        }

        public void OnResumeDialogue()
        {
            _dialogePaused = false;
        }

        private IEnumerator PausedDisplayLine(DialogueEventSystem.DialogueContinueEventArgs args)
        {
            dialogueHolder.SetActive(false);
            while (_dialogePaused)
            {
                yield return null;
            }

            dialogueHolder.SetActive(true);
            DisplayLine(args);
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

        private Tween continueIconTween;

        private IEnumerator AllowNextLineCoroutine()
        {
            yield return new WaitForSeconds(0.3f);
            _canContinueToNextLine = true;
            DialogueEventSystem.InvokeUpdateCanContinueToNextLine(
                new DialogueEventSystem.UpdateCanContinueToNextLineEventArgs(_canContinueToNextLine));

            // Reset icon position before enabling
            if (canContinueIcon != null)
            {
                RectTransform iconTransform = canContinueIcon.GetComponent<RectTransform>();
                iconTransform.anchoredPosition = canContinueIconOriginalPosition;
            }

            canContinueIcon.SetActive(_canContinueToNextLine);
            StartContinueIconAnimation();
            DisplayChoiceButtons(currentDialogueEventArgs);
        }

        private void StartContinueIconAnimation()
        {
            if (continueIconTween != null && continueIconTween.IsActive())
                continueIconTween.Kill();

            RectTransform iconTransform = canContinueIcon.GetComponent<RectTransform>();
            // Reset to cached start position each time animation starts
            iconTransform.anchoredPosition = canContinueIconOriginalPosition;
            float startY = iconTransform.anchoredPosition.y;
            continueIconTween = iconTransform
                .DOAnchorPosY(startY + 10f, 0.5f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
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
            var es = UnityEngine.EventSystems.EventSystem.current;
            if (es)
                es.SetSelectedGameObject(null);

            for (int i = 0; i < args.Choices.Count; i++)
            {
                GameObject choiceButtonGameObject = _poolManager.GetObject(HelperUIName.DialogueChoiceButtonUI);
                //Reset position
                choiceButtonGameObject.transform.SetParent(ChoiceButtonsHolder.transform, false);
                var rt = choiceButtonGameObject.GetComponent<RectTransform>();
                rt.localScale = Vector3.one;
                rt.anchoredPosition = Vector2.zero;
                DialogueChoiceButtonUI choiceButton = choiceButtonGameObject.GetComponent<DialogueChoiceButtonUI>();
                choiceButtons.Add(choiceButton);

                var choice = args.Choices[i];
                var button = choiceButtons[i];
                button.gameObject.SetActive(true);
                button.SetChoiceText(choice.text);
                button.SetChoiceIndex(i);
            }

            // Automatically select the first choice
            if (args.Choices.Count > 0)
            {
                choiceButtons[0].SelectButton();
                DialogueEventSystem.InvokeUpdateChoiceIndex(
                    new DialogueEventSystem.UpdateChoiceIndexEventArgs(0));
            }
        }

        private void HideChoiceButtons()
        {
            if (choiceButtons.Count == 0)
                return;
            foreach (DialogueChoiceButtonUI button in choiceButtons)
            {
                _poolManager.ReturnObject(HelperUIName.DialogueChoiceButtonUI, button.gameObject);
            }

            choiceButtons.Clear();
        }
    }
}