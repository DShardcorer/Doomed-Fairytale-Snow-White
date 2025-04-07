
using System;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [Header("Dialogue Box UI")]
    // [SerializeField] private GameObject dialogueBoxUI;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private DialogueChoiceButtonUI[] choiceButtons;

    [Header("Dialogue Box Sprites")]
    [SerializeField] private Animator leftSpriteImageAnimator;
    [SerializeField] private Animator rightSpriteImageAnimator;

    [Header("Speaker Name")]
    [SerializeField] private TextMeshProUGUI speakerNameText;




    private void Awake()
    {
        // dialogueBoxUI.SetActive(false);
        DialogueEventSystem.OnEnterDialogue += OnEnterDialogue;
        DialogueEventSystem.OnExitDialogue += OnExitDialogue;
        DialogueEventSystem.OnDialogueContinue += OnDialogueContinue;
        DialogueEventSystem.OnUpdateSpeakerName += OnUpdateSpeakerName;
        DialogueEventSystem.OnUpdateSpeakerSprite += OnUpdateSpeakerSprite;
        // foreach (DialogueChoiceButtonUI button in choiceButtons)
        // {
        //     button.gameObject.SetActive(false);
        // }
        // leftSpriteImageAnimator.gameObject.SetActive(false);
        // rightSpriteImageAnimator.gameObject.SetActive(false);
        // speakerNameText.gameObject.SetActive(false);
        gameObject.SetActive(false);
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
        dialogueText.text = args.DialogueText;

        foreach (DialogueChoiceButtonUI button in choiceButtons)
        {
            button.gameObject.SetActive(false);
        }

        //enable and set info for buttons based on ink choices

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


}
