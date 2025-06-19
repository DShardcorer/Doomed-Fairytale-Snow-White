using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AssetManagement;
using DG.Tweening;
using EventBus.Dialogue;
using Febucci.UI;
using GeneralManagers;
using Helpers;
using Ink.InkLibs.InkRuntime;
using Input;
using Pool;
using TMPro;
using UnityEngine;
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
        private Vector2 canContinueIconOriginalPosition;

        [Header("Choice Buttons")] [SerializeField]
        private GameObject ChoiceButtonsHolder;

        private List<DialogueChoiceButtonUI> choiceButtons = new List<DialogueChoiceButtonUI>();

        [Header("Dialogue Box Sprites")] [SerializeField]
        private Image leftSpriteImage;

        [SerializeField] private Image rightSpriteImage;

        [Header("CG Layers")] [SerializeField] private GameObject backCGLayer;
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

        // Private fields
        private AudioSource _audioSource;
        private bool _canContinueToNextLine;
        private DialogueEventSystem.DialogueContinueEventArgs currentDialogueEventArgs;
        private bool _isSkippingTypewriter = false;
        private int currentDisplayedLetterIndex = 0;
        private bool _dialogePaused = false;
        private PoolManager _poolManager;
        private Tween _continueIconTween;
        private float _textTypingSoundTimeInterval = 0.3f;
        private float _lastTextTypingSoundTime = 0;

        // Dictionary for caching CG load operations
        private Dictionary<string, GameObject> _cgCache = new Dictionary<string, GameObject>();

        private void Awake()
        {
            SubscribeEvents();
            SetupComponents();
        }

        private void SetupComponents()
        {
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

            // Clean up tweens
            if (_continueIconTween != null && _continueIconTween.IsActive())
            {
                _continueIconTween.Kill();
            }

            // Clear cache
            _cgCache.Clear();
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

            if (GameManager.Instance != null && GameManager.Instance.InputManager != null)
            {
                GameManager.Instance.InputManager.uiSubmitInputted -= OnUISubmitInputted;
            }
        }

        #region CGs

        private async void OnUpdateCGPath(DialogueEventSystem.UpdateCGPathEventArgs obj)
        {
            cgPath = obj.CGPath;

            // Load new CGs first before modifying existing layers
            GameObject newBackCG = null;
            GameObject newFrontCG = null;

            try
            {
                string backPath = HelperAddressablesGroup.CGs + cgPath + "/Back" + HelperExtension.PREFAB;
                string frontPath = HelperAddressablesGroup.CGs + cgPath + "/Front" + HelperExtension.PREFAB;

                // Load both simultaneously for better performance
                var backTask = AddressablesManager.Instance.LoadAndInstantiate(backPath);
                var frontTask = AddressablesManager.Instance.LoadAndInstantiate(frontPath);

                await Task.WhenAll(backTask, frontTask);
                newBackCG = backTask.Result;
                newFrontCG = frontTask.Result;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading CG path {cgPath}: {e.Message}");
            }

            // Now replace the existing content
            ClearCGLayer(backCGLayer);
            ClearCGLayer(frontCGLayer);

            if (newBackCG != null)
            {
                newBackCG.transform.SetParent(backCGLayer.transform, false);
                backCGLayer.SetActive(true);
            }
            else
            {
                backCGLayer.SetActive(false);
            }

            if (newFrontCG != null)
            {
                newFrontCG.transform.SetParent(frontCGLayer.transform, false);
                frontCGLayer.SetActive(true);
            }
            else
            {
                frontCGLayer.SetActive(false);
            }
        }

        private async void OnUpdateCG(DialogueEventSystem.UpdateCGEventArgs obj)
        {
            if (String.Equals("null", obj.CGName))
            {
                mainCGLayer.gameObject.SetActive(false);
                return;
            }

            // Load new CG first
            GameObject newCG = null;
            try
            {
                string path = HelperAddressablesGroup.CGs + cgPath + "/" + obj.CGName + HelperExtension.PREFAB;
                newCG = await AddressablesManager.Instance.LoadAndInstantiate(path);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading CG {obj.CGName}: {e.Message}");
            }

            // Now replace existing content
            ClearCGLayer(mainCGLayer);

            if (newCG != null)
            {
                mainCGLayer.gameObject.SetActive(true);
                newCG.transform.SetParent(mainCGLayer.transform, false);
                newCG.SetActive(true);
            }
            else
            {
                mainCGLayer.gameObject.SetActive(false);
            }
        }

        private async void OnUpdateCGFront(DialogueEventSystem.UpdateCGFrontEventArgs obj)
        {
            if (String.Equals("null", obj.CGFrontName))
            {
                frontCGLayer.gameObject.SetActive(false);
                return;
            }

            // Load new CG first
            GameObject newCG = null;
            try
            {
                string path = HelperAddressablesGroup.CGs + cgPath + "/" + obj.CGFrontName + HelperExtension.PREFAB;
                newCG = await AddressablesManager.Instance.LoadAndInstantiate(path);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading front CG {obj.CGFrontName}: {e.Message}");
            }

            // Now replace existing content
            ClearCGLayer(frontCGLayer);

            if (newCG != null)
            {
                frontCGLayer.gameObject.SetActive(true);
                newCG.transform.SetParent(frontCGLayer.transform, false);
                newCG.SetActive(true);
            }
            else
            {
                frontCGLayer.gameObject.SetActive(false);
            }
        }

        private async void OnUpdateCGBack(DialogueEventSystem.UpdateCGBackEventArgs obj)
        {
            if (String.Equals("null", obj.CGBackName))
            {
                backCGLayer.gameObject.SetActive(false);
                return;
            }

            // Load new CG first
            GameObject newCG = null;
            try
            {
                string path = HelperAddressablesGroup.CGs + cgPath + "/" + obj.CGBackName + HelperExtension.PREFAB;
                newCG = await AddressablesManager.Instance.LoadAndInstantiate(path);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading back CG {obj.CGBackName}: {e.Message}");
            }

            // Now replace existing content
            ClearCGLayer(backCGLayer);

            if (newCG != null)
            {
                backCGLayer.gameObject.SetActive(true);
                newCG.transform.SetParent(backCGLayer.transform, false);
                newCG.SetActive(true);
            }
            else
            {
                backCGLayer.gameObject.SetActive(false);
            }
        }

        private void ClearCGLayer(GameObject layer)
        {
            foreach (Transform child in layer.transform)
            {
                Destroy(child.gameObject);
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
            frontCGLayer.gameObject.SetActive(false);
            mainCGLayer.gameObject.SetActive(false);
            backCGLayer.gameObject.SetActive(false);
        }

        private void OnDialogueContinue(DialogueEventSystem.DialogueContinueEventArgs args)
        {
            _canContinueToNextLine = false;
            DialogueEventSystem.InvokeUpdateCanContinueToNextLine(
                new DialogueEventSystem.UpdateCanContinueToNextLineEventArgs(_canContinueToNextLine));

            if (args.Delay > 0)
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

        public void OnPauseDialogue() => _dialogePaused = true;
        public void OnResumeDialogue() => _dialogePaused = false;

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

        private IEnumerator AllowNextLineCoroutine()
        {
            yield return new WaitForSeconds(0.3f);
            _canContinueToNextLine = true;
            DialogueEventSystem.InvokeUpdateCanContinueToNextLine(
                new DialogueEventSystem.UpdateCanContinueToNextLineEventArgs(_canContinueToNextLine));

            if (canContinueIcon != null)
            {
                RectTransform iconTransform = canContinueIcon.GetComponent<RectTransform>();
                iconTransform.anchoredPosition = canContinueIconOriginalPosition;
                canContinueIcon.SetActive(true);
                StartContinueIconAnimation();
            }

            DisplayChoiceButtons(currentDialogueEventArgs);
        }

        private void StartContinueIconAnimation()
        {
            if (_continueIconTween != null && _continueIconTween.IsActive())
                _continueIconTween.Kill();

            RectTransform iconTransform = canContinueIcon.GetComponent<RectTransform>();
            iconTransform.anchoredPosition = canContinueIconOriginalPosition;
            float startY = iconTransform.anchoredPosition.y;

            _continueIconTween = iconTransform
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
            if (textTypingSounds.Length == 0) return;

            if (currentDisplayedLetterIndex % textTypingSoundInterval == 0 ||
                _lastTextTypingSoundTime >= _textTypingSoundTimeInterval)
            {
                _lastTextTypingSoundTime = 0;

                // Use deterministic approach for sound and pitch selection
                int soundIndex = Math.Abs(currentCharacter.GetHashCode() % textTypingSounds.Length);
                int pitchRange = Mathf.FloorToInt((maxPitch - minPitch) * 100);
                float pitch = minPitch + (Math.Abs(currentCharacter.GetHashCode() % pitchRange) / 100f);

                _audioSource.pitch = pitch;
                _audioSource.PlayOneShot(textTypingSounds[soundIndex]);
            }
        }

        private void Update()
        {
            _lastTextTypingSoundTime += Time.deltaTime;
        }

        private void OnUISubmitInputted(InputEventContext context)
        {
            if (!_canContinueToNextLine && !_isSkippingTypewriter)
            {
                _isSkippingTypewriter = true;
                typewriter.SkipTypewriter();
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
            if (parts.Length < 2) return;

            string characterId = parts[0];
            string characterEmotion = parts[1];
            string spritePath = HelperResourcePath.DialogueSpritePath + characterId + "/" + characterEmotion;

            if (args.Layout == "left")
            {
                UpdateSpriteImage(leftSpriteImage, rightSpriteImage, spritePath, true);
            }
            else if (args.Layout == "right")
            {
                UpdateSpriteImage(rightSpriteImage, leftSpriteImage, spritePath, false);
            }
        }

        private void UpdateSpriteImage(Image activeImage, Image inactiveImage, string spritePath, bool isLeft)
        {
            activeImage.gameObject.SetActive(true);
            activeImage.sprite = UnityEngine.Resources.Load<Sprite>(spritePath);
            activeImage.GetComponent<CanvasGroup>().alpha = 1;
            inactiveImage.GetComponent<CanvasGroup>().alpha = 0.5f;
        }

        private void DisplayChoiceButtons(DialogueEventSystem.DialogueContinueEventArgs args)
        {
            var eventSystem = UnityEngine.EventSystems.EventSystem.current;
            if (eventSystem) eventSystem.SetSelectedGameObject(null);

            if (args.Choices.Count == 0) return;

            for (int i = 0; i < args.Choices.Count; i++)
            {
                GameObject choiceButtonGameObject = _poolManager.GetObject(HelperUIName.DialogueChoiceButtonUI);
                choiceButtonGameObject.transform.SetParent(ChoiceButtonsHolder.transform, false);

                RectTransform rt = choiceButtonGameObject.GetComponent<RectTransform>();
                rt.localScale = Vector3.one;
                rt.anchoredPosition = Vector2.zero;

                DialogueChoiceButtonUI choiceButton = choiceButtonGameObject.GetComponent<DialogueChoiceButtonUI>();
                choiceButtons.Add(choiceButton);

                var choice = args.Choices[i];
                choiceButton.gameObject.SetActive(true);
                choiceButton.SetChoiceText(choice.text);
                choiceButton.SetChoiceIndex(i);
            }

            // Select first choice
            if (args.Choices.Count > 0)
            {
                choiceButtons[0].SelectButton();
                DialogueEventSystem.InvokeUpdateChoiceIndex(
                    new DialogueEventSystem.UpdateChoiceIndexEventArgs(0));
            }
        }

        private void HideChoiceButtons()
        {
            if (choiceButtons.Count == 0) return;

            foreach (DialogueChoiceButtonUI button in choiceButtons)
            {
                _poolManager.ReturnObject(HelperUIName.DialogueChoiceButtonUI, button.gameObject);
            }

            choiceButtons.Clear();
        }
    }
}