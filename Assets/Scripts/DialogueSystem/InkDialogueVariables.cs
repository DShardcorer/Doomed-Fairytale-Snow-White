using System.Collections.Generic;
using Ink.InkLibs.InkRuntime;
using UnityEngine;
using Object = Ink.InkLibs.InkRuntime.Object;

namespace DialogueSystem
{
    public class InkDialogueVariables
    {
        private Dictionary<string, Object> variables;
        private Story story;

        public InkDialogueVariables(Story inkGlobalsStory)
        {
            story = inkGlobalsStory;
            variables = new Dictionary<string, Object>();
            foreach (string name in inkGlobalsStory.variablesState)
            {
                Object value = inkGlobalsStory.variablesState.GetVariableWithName(name);
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

        public void UpdateVariablesState(string name, Object value)
        {
            //only maintain the variables initialized from the ink file
            if (!variables.ContainsKey(name))
            {
                return;
            }

            variables[name] = value;
            // Debug.Log($"Updated dialogue variable: {name} to {value}");
        }

        // public void UpdateVariablesState(string name, Object value)
        // {
        //     if (!variables.ContainsKey(name))
        //         return;
        //     variables[name] = value;
        //     story.variablesState.SetGlobal(name, value);
        // }

        public void SyncVariablesToStory(Story story)
        {
            foreach (KeyValuePair<string, Object> kvp in variables)
            {
                story.variablesState.SetGlobal(kvp.Key, kvp.Value);
            }
        }
    }
}