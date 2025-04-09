
using System;
using System.Collections;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [Header("Dialogue Box Settings")]
    [SerializeField] private float textSpeed = 0.05f; // Speed of text display
    [Header("Dialogue Box UI")]
    // [SerializeField] private GameObject dialogueBoxUI;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject canContinueIcon;
    [SerializeField] private DialogueChoiceButtonUI[] choiceButtons;

    [Header("Dialogue Box Sprites")]
    [SerializeField] private Animator leftSpriteImageAnimator;
    [SerializeField] private Animator rightSpriteImageAnimator;

    [Header("Speaker Name")]
    [SerializeField] private TextMeshProUGUI speakerNameText;

    [Header("Sound Settings")]
    [SerializeField] private int textTypingSoundInterval = 2; // Interval for playing typing sound

    [Range(-3,3)]
    [SerializeField] private float minPitch = 0.5f;

    [Range(-3,3)]
    [SerializeField] private float maxPitch = 1f;


    private Coroutine displayLineCoroutine;
    private bool canContinueToNextLine = false;
    private bool skipTyping = false;

    [SerializeField]
    private AudioClip[] textTypingSounds;
    private AudioSource audioSource;




    private void Awake()
    {
        DialogueEventSystem.OnEnterDialogue += OnEnterDialogue;
        DialogueEventSystem.OnExitDialogue += OnExitDialogue;
        DialogueEventSystem.OnDialogueContinue += OnDialogueContinue;
        DialogueEventSystem.OnUpdateSpeakerName += OnUpdateSpeakerName;
        DialogueEventSystem.OnUpdateSpeakerSprite += OnUpdateSpeakerSprite;
        GameManager.Instance.InputManager.uiSubmitInputted += OnUISubmitInputted;
        audioSource = gameObject.AddComponent<AudioSource>();
        gameObject.SetActive(false);
    }

    private void OnUISubmitInputted(InputEventContext context)
    {
        skipTyping = true;
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
        if (displayLineCoroutine != null)
        {
            StopCoroutine(displayLineCoroutine);
        }
        canContinueToNextLine = false;
        DialogueEventSystem.InvokeUpdateCanContinueToNextLine(new DialogueEventSystem.UpdateCanContinueToNextLineEventArgs(canContinueToNextLine));
        displayLineCoroutine = StartCoroutine(DisplayLine(args));
    }
    private IEnumerator DisplayLine(DialogueEventSystem.DialogueContinueEventArgs args)
    {
        skipTyping = false;
        HideChoiceButtons();
        canContinueIcon.SetActive(canContinueToNextLine);
        dialogueText.text = string.Empty;
        bool isAddingRichTextTag = false;
        string line = args.DialogueText;
        int currentDisplayedLetterIndex = 0;
        foreach (char letter in line.ToCharArray())
        {
            if (skipTyping)
            {
                dialogueText.text = line;
                break;
            }
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
        canContinueToNextLine = true;
        DialogueEventSystem.InvokeUpdateCanContinueToNextLine(new DialogueEventSystem.UpdateCanContinueToNextLineEventArgs(canContinueToNextLine));
        canContinueIcon.SetActive(canContinueToNextLine);
        DisplayChoiceButtons(args);

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
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(textTypingSounds[audioClipIndex]);
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
                DialogueEventSystem.InvokeUpdateChoiceIndex(new DialogueEventSystem.UpdateChoiceIndexEventArgs(inkChoiceIndex));
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
