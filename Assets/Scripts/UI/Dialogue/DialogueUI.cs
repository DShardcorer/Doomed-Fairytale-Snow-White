
using System;
using Ink.Runtime;
using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    [Header("Dialogue Box UI")]
    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private DialogueChoiceButtonUI[] choiceButtons;


    private void Awake()
    {
        dialogueUI.SetActive(false);
        DialogueEventSystem.OnEnterDialogue += OnEnterDialogue;
        DialogueEventSystem.OnExitDialogue += OnExitDialogue;
        DialogueEventSystem.OnDialogueContinue += OnDialogueContinue;
        foreach (DialogueChoiceButtonUI button in choiceButtons)
        {
            button.gameObject.SetActive(false);
        }
    }
    private void OnEnterDialogue(DialogueEventSystem.EnterDialogueEventArgs args)
    {
        dialogueUI.SetActive(true);
    }
    private void OnExitDialogue()
    {
        dialogueUI.SetActive(false);
        dialogueText.text = string.Empty;
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
            if(inkChoiceIndex == 0)
            {
                choiceButton.SelectButton();
                DialogueEventSystem.InvokeUpdateChoiceIndex(new DialogueEventSystem.UpdateChoiceIndexEventArgs(inkChoiceIndex));
            }
            


        }
    }


}
