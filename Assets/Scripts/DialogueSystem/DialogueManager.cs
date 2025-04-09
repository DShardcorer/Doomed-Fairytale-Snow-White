using UnityEngine;
using Ink.Runtime;
using System;
using static DialogueEventSystem;
using System.Collections.Generic;
public class DialogueManager : MonoBehaviour, ILifecycle<GameManager>
{
    private GameManager gameManager;
    private Story story;
    [SerializeField]
    private TextAsset inkGlobalVariablesTextAsset;
    private Story inkGlobalVariablesStory;
    private InkExternalFunctions inkExternalFunctions;
    private InkDialogueVariables inkDialogueVariables;
    private bool isDialoguePlaying = false;
    private int currentChoiceIndex = -1;

    private const string SPEAKER_TAG = "speaker";
    private const string SPRITE_TAG = "sprite";
    private const string LAYOUT_TAG = "layout";
    private bool canContinueToNextLine = false;


    private void Awake()
    {
        inkGlobalVariablesStory = new Story(inkGlobalVariablesTextAsset.text);
        inkDialogueVariables = new InkDialogueVariables(inkGlobalVariablesStory);
        inkExternalFunctions = new InkExternalFunctions();
    }

    private void BindStory(TextAsset inkDialogueFile)
    {
        story = new Story(inkDialogueFile.text);
    }

    public void Initialize(GameManager parent)
    {
        gameManager = parent;
        GameManager.Instance.InputManager.uiSubmitInputted += OnUISubmitInputted;
        DialogueEventSystem.OnEnterDialogue += OnEnterDialogue;
        DialogueEventSystem.OnUpdateChoiceIndex += OnUpdateChoiceIndex;
        DialogueEventSystem.OnUpdateInkDialogueVariable += OnUpdateInkDialogueVariable;
        DialogueEventSystem.OnUpdateCanContinueToNextLine += OnUpdateCanContinueToNextLine;
        QuestEventSystem.OnQuestStateChanged += OnQuestStateChanged;

    }
    private void OnUpdateCanContinueToNextLine(UpdateCanContinueToNextLineEventArgs args)
    {
        canContinueToNextLine = args.CanContinueToNextLine;
    }

    private void OnQuestStateChanged(Quest quest)
    {
        DialogueEventSystem.InvokeUpdateInkDialogueVariable(
            new UpdateInkDialogueVariableEventArgs(quest.questInfo.QuestName + "State", new StringValue(quest.questState.ToString())));

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
        if(!canContinueToNextLine) return;
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

    }

    private void OnEnterDialogue(EnterDialogueEventArgs args)
    {
        if (isDialoguePlaying) return;
        isDialoguePlaying = true;
        GameManager.Instance.InputManager.SetInputEventContext(InputEventContext.DIALOGUE);
        BindStory(args.InkDialogueFile);
        inkDialogueVariables.SyncVariablesAndStartListening(story);
        inkExternalFunctions.StartListening(story);
        if (!args.KnotName.Equals(string.Empty))
        {
            story.ChoosePathString(args.KnotName);
        }
        else
        {
            Debug.LogWarning("Knot name is empty. Cannot start dialogue.");
            return;
        }
        Debug.Log($"Starting dialogue with knot: {args.KnotName}");
        ContinueOrExitStory();
    }

    private void ContinueOrExitStory()
    {
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
                HandleTags(story.currentTags);
                InvokeDialogueContinue(new DialogueContinueEventArgs(dialogue, story.currentChoices));
            }
        }
        else if (story.currentChoices.Count == 0)
        {
            ExitDialogue();
        }

    }

    private void HandleTags(List<string> currentTags)
    {
        string speakerName = string.Empty;
        string speakerSprite = string.Empty;
        string layout = string.Empty;
        foreach (string tag in currentTags)
        {
            string[] tagParts = tag.Split(':');
            if (tagParts.Length != 2)
            {
                Debug.LogWarning($"Invalid tag format: {tag}. Expected format: 'key:value'.");
                continue;
            }
            string key = tagParts[0].Trim();
            string value = tagParts[1].Trim();
            switch (key.ToLower())
            {
                case SPEAKER_TAG:
                    speakerName = value;
                    break;
                case SPRITE_TAG:
                    speakerSprite = value;
                    break;
                case LAYOUT_TAG:
                    layout = value;
                    break;
                default:
                    Debug.LogWarning($"Unknown tag: {key}. Ignoring.");
                    break;
            }
        }
        if (!string.IsNullOrEmpty(speakerName))
        {
            DialogueEventSystem.InvokeUpdateSpeakerName(new UpdateSpeakerNameEventArgs(speakerName));
        }
        if (!string.IsNullOrEmpty(speakerSprite) && !string.IsNullOrEmpty(layout))
        {
            DialogueEventSystem.InvokeUpdateSpeakerSprite(new UpdateSpeakerSpriteEventArgs(speakerSprite, layout));
        }else{
            DialogueEventSystem.InvokeUpdateSpeakerSprite(new UpdateSpeakerSpriteEventArgs(speakerSprite));
        }

    }

    private void ExitDialogue()
    {
        isDialoguePlaying = false;
        GameManager.Instance.InputManager.SetInputEventContext(InputEventContext.DEFAULT);
        inkExternalFunctions.StopListening(story);
        inkDialogueVariables.StopListening(story);
        DialogueEventSystem.InvokeExitDialogue();
        // Add any additional logic for exiting the dialogue, such as UI updates or state changes.

        story.ResetState();
    }

    private bool IsLineBlank(string dialogueLine)
    {
        return string.IsNullOrWhiteSpace(dialogueLine) || dialogueLine.Equals(" ") || dialogueLine.Equals("\n") || dialogueLine.Equals("\r\n");
    }

}
