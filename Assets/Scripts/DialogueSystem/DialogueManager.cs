using UnityEngine;
using Ink.Runtime;
using System;
using static DialogueEventSystem;
public class DialogueManager : MonoBehaviour, ILifecycle<GameManager>
{
    private GameManager gameManager;

    [Header("Ink Story")]
    [SerializeField] private TextAsset inkJsonAsset;

    private Story story;
    private InkExternalFunctions inkExternalFunctions;
    private InkDialogueVariables inkDialogueVariables;

    private bool isDialoguePlaying = false;
    private int currentChoiceIndex = -1;

    private void Awake()
    {
        story = new Story(inkJsonAsset.text);
        inkExternalFunctions = new InkExternalFunctions();
        inkExternalFunctions.Bind(story);
        inkDialogueVariables = new InkDialogueVariables(story);

    }

    public void Initialize(GameManager parent)
    {
        gameManager = parent;
        DialogueEventSystem.OnEnterDialogue += OnEnterDialogue;
        GameManager.Instance.InputManager.uiSubmitInputted += OnUISubmitInputted;
        DialogueEventSystem.OnUpdateChoiceIndex += OnUpdateChoiceIndex;
        DialogueEventSystem.OnUpdateInkDialogueVariable += OnUpdateInkDialogueVariable;
        QuestEventSystem.OnQuestStateChanged += OnQuestStateChanged;
    }

    private void OnQuestStateChanged(Quest quest)
    {
        DialogueEventSystem.InvokeUpdateInkDialogueVariable(
            new UpdateInkDialogueVariableEventArgs(quest.questInfo.QuestName + "State", new StringValue(quest.questState.ToString()) ));
        
    }

    private void OnUpdateInkDialogueVariable(UpdateInkDialogueVariableEventArgs args)
    {
        inkDialogueVariables.UpdateVariablesState(args.VariableName, args.VariableValue);
    }
    private void OnUpdateChoiceIndex(UpdateChoiceIndexEventArgs args)
    {
        currentChoiceIndex = args.ChoiceIndex;
    }

    private void OnUISubmitInputted(InputEventContext context)
    {
        if (context != InputEventContext.DIALOGUE) return;
        if (isDialoguePlaying)
        {
            Debug.Log($"Submit input received in context: {context}");
            ContinueOrExitStory();
        }
        else
        {
            Debug.Log("No dialogue is currently playing.");
        }
    }

    public void Dispose()
    {
        gameManager = null;
        DialogueEventSystem.OnEnterDialogue -= OnEnterDialogue;
        GameManager.Instance.InputManager.uiSubmitInputted -= OnUISubmitInputted;
        DialogueEventSystem.OnUpdateChoiceIndex -= OnUpdateChoiceIndex;
        DialogueEventSystem.OnUpdateInkDialogueVariable -= OnUpdateInkDialogueVariable;
        QuestEventSystem.OnQuestStateChanged -= OnQuestStateChanged;
    }
    private void OnDestroy()
    {
        inkExternalFunctions.Unbind(story);
    }

    private void OnEnterDialogue(EnterDialogueEventArgs args)
    {
        if (isDialoguePlaying) return;

        isDialoguePlaying = true;
        GameManager.Instance.InputManager.SetInputEventContext(InputEventContext.DIALOGUE);
        if (!args.KnotName.Equals(string.Empty))
        {
            story.ChoosePathString(args.KnotName);
        }
        else
        {
            Debug.LogWarning("Knot name is empty. Cannot start dialogue.");
            return;
        }

        inkDialogueVariables.SyncVariablesAndStartListening(story);
        ContinueOrExitStory();
    }

    private void ContinueOrExitStory()
    {
        Debug.Log("Continuing story...");
        if (story.currentChoices.Count > 0 && currentChoiceIndex != -1)
        {
            story.ChooseChoiceIndex(currentChoiceIndex);
            currentChoiceIndex = -1; // Reset the choice index after making a choice
        }
        if (story.canContinue)
        {
            string dialogue = story.Continue();
            while (IsLineBlank(dialogue) && story.canContinue)
            {
                dialogue = story.Continue();
            }
            //Handle cases where last line of dialogue is blank
            if (IsLineBlank(dialogue) && !story.canContinue)
            {
                ExitDialogue();
            }
            else
            {
                InvokeDialogueContinue(new DialogueContinueEventArgs(dialogue, story.currentChoices));
            }
        }
        else if (story.currentChoices.Count == 0)
        {
            ExitDialogue();
        }

    }

    private void ExitDialogue()
    {
        isDialoguePlaying = false;
        GameManager.Instance.InputManager.SetInputEventContext(InputEventContext.DEFAULT);
        DialogueEventSystem.InvokeExitDialogue();
        // Add any additional logic for exiting the dialogue, such as UI updates or state changes.
        inkDialogueVariables.StopListening(story);
        story.ResetState();
    }

    private bool IsLineBlank(string dialogueLine)
    {
        return string.IsNullOrWhiteSpace(dialogueLine) || dialogueLine.Equals(" ") || dialogueLine.Equals("\n") || dialogueLine.Equals("\r\n");
    }

}
