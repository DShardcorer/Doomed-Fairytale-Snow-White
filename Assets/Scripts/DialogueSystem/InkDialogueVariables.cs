using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;

public class InkDialogueVariables
{
    private Dictionary<string, Ink.Runtime.Object> variables;

    public InkDialogueVariables(Story story)
    {
        variables = new Dictionary<string, Ink.Runtime.Object>();
        foreach (string name in story.variablesState)
        {
            Ink.Runtime.Object value = story.variablesState.GetVariableWithName(name);
            variables.Add(name, value);

        }
    }
    public void SyncVariablesAndStartListening(Story story)
    {
        SyncVariablesToStory(story);
        story.variablesState.variableChangedEvent += UpdateVariablesState;

    }
    public void StopListening(Story story)
    {
        story.variablesState.variableChangedEvent -= UpdateVariablesState;
    }
    public void UpdateVariablesState(string name, Ink.Runtime.Object value)
    {
        //only maintain the variables initialized from the ink file
        if (!variables.ContainsKey(name))
        {
            return;
        }

        variables[name] = value;
        Debug.Log($"Updated dialogue variable: {name} to {value}");

    }

    public void SyncVariablesToStory(Story story)
    {
        foreach (KeyValuePair<string, Ink.Runtime.Object> kvp in variables)
        {
            story.variablesState.SetGlobal(kvp.Key, kvp.Value);
        }
    }





}
