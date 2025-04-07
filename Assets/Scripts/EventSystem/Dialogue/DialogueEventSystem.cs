using System;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;

public static class DialogueEventSystem
{
    public class EnterDialogueEventArgs : EventArgs
    {
        public string KnotName;
        public EnterDialogueEventArgs(string knotName)
        {
            KnotName = knotName;
        }
    }

    public static event Action<EnterDialogueEventArgs> OnEnterDialogue;
    public static void InvokeEnterDialogue(EnterDialogueEventArgs e)
    {
        OnEnterDialogue?.Invoke(e);
    }


    public static event Action OnExitDialogue;
    public static void InvokeExitDialogue()
    {
        OnExitDialogue?.Invoke();
    }

    public class DialogueContinueEventArgs : EventArgs
    {
        public string DialogueText;
        public List<Choice> Choices;
        public DialogueContinueEventArgs(string dialogueText, List<Choice> choices)
        {
            DialogueText = dialogueText;
            Choices = choices;
        }
    }
    public static event Action<DialogueContinueEventArgs> OnDialogueContinue;
    public static void InvokeDialogueContinue(DialogueContinueEventArgs e)
    {
        OnDialogueContinue?.Invoke(e);
    }

    public class UpdateChoiceIndexEventArgs : EventArgs
    {
        public int ChoiceIndex;
        public UpdateChoiceIndexEventArgs(int choiceIndex)
        {
            ChoiceIndex = choiceIndex;
        }
    }
    public static event Action<UpdateChoiceIndexEventArgs> OnUpdateChoiceIndex;


    public static void InvokeUpdateChoiceIndex(UpdateChoiceIndexEventArgs e)
    {
        OnUpdateChoiceIndex?.Invoke(e);
    }

    public class UpdateInkDialogueVariableEventArgs : EventArgs
    {
        public string VariableName;
        public Ink.Runtime.Object VariableValue;
        public UpdateInkDialogueVariableEventArgs(string variableName, Ink.Runtime.Object variableValue)
        {
            VariableName = variableName;
            VariableValue = variableValue;
        }
    }
    public static event Action<UpdateInkDialogueVariableEventArgs> OnUpdateInkDialogueVariable;

    public static void InvokeUpdateInkDialogueVariable(UpdateInkDialogueVariableEventArgs e)
    {
        OnUpdateInkDialogueVariable?.Invoke(e);
    }


}
