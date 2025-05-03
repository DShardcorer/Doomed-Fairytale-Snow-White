using System;
using System.Collections.Generic;
using Ink.InkLibs.InkRuntime;
using UnityEngine;
using Object = Ink.InkLibs.InkRuntime.Object;

namespace EventSystem.Dialogue
{
    public static class DialogueEventSystem
    {
        public class EnterDialogueEventArgs : EventArgs
        {
            public TextAsset InkDialogueFile;
            public string KnotName;

            public EnterDialogueEventArgs(TextAsset inkDialogueFile, string knotName)
            {
                InkDialogueFile = inkDialogueFile;
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
            public float Delay;
            public bool Pause;

            public DialogueContinueEventArgs(string dialogueText, List<Choice> choices, float delay = 0, bool pause = false)
            {
                DialogueText = dialogueText;
                Choices = choices;
                Delay = delay;
                Pause = pause;
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
            public Object VariableValue;

            public UpdateInkDialogueVariableEventArgs(string variableName, Object variableValue)
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


        public class UpdateSpeakerSpriteEventArgs : EventArgs
        {
            public string SpeakerSpriteName;
            public string Layout;

            public UpdateSpeakerSpriteEventArgs(string speakerSpriteName, string layout = "left")
            {
                this.SpeakerSpriteName = speakerSpriteName;
                this.Layout = layout;
            }
        }

        public static event Action<UpdateSpeakerSpriteEventArgs> OnUpdateSpeakerSprite;

        public static void InvokeUpdateSpeakerSprite(UpdateSpeakerSpriteEventArgs e)
        {
            OnUpdateSpeakerSprite?.Invoke(e);
        }

        public class UpdateSpeakerNameEventArgs : EventArgs
        {
            public string SpeakerName;

            public UpdateSpeakerNameEventArgs(string speakerName)
            {
                this.SpeakerName = speakerName;
            }
        }

        public static event Action<UpdateSpeakerNameEventArgs> OnUpdateSpeakerName;

        public static void InvokeUpdateSpeakerName(UpdateSpeakerNameEventArgs e)
        {
            OnUpdateSpeakerName?.Invoke(e);
        }

        public class UpdateCGPathEventArgs : EventArgs
        {
            public string CGPath;
            
            public UpdateCGPathEventArgs(string cgPath)
            {
                this.CGPath = cgPath;
            }
        }
        
        public static event Action<UpdateCGPathEventArgs> OnUpdateCGPath;
        public static void InvokeUpdateCGPath(UpdateCGPathEventArgs e)
        {
            OnUpdateCGPath?.Invoke(e);
        }

        public class UpdateCGEventArgs : EventArgs
        {
            public string CGName;

            public UpdateCGEventArgs(string cgName)
            {
                this.CGName = cgName;
            }
        }

        public static event Action<UpdateCGEventArgs> OnUpdateCG;

        public static void InvokeUpdateCG(UpdateCGEventArgs e)
        {
            OnUpdateCG?.Invoke(e);
        }
        public class UpdateCGBackEventArgs : EventArgs
        {
            public string CGBackName;

            public UpdateCGBackEventArgs(string cgBackName)
            {
                this.CGBackName = cgBackName;
            }
        }
        public static event Action<UpdateCGBackEventArgs> OnUpdateCGBack;
        public static void InvokeUpdateCGBack(UpdateCGBackEventArgs e)
        {
            OnUpdateCGBack?.Invoke(e);
        }
        
        public class UpdateCGFrontEventArgs : EventArgs
        {
            public string CGFrontName;

            public UpdateCGFrontEventArgs(string cgFrontName)
            {
                this.CGFrontName = cgFrontName;
            }
        }
        public static event Action<UpdateCGFrontEventArgs> OnUpdateCGFront;
        
        public static void InvokeUpdateCGFront(UpdateCGFrontEventArgs e)
        {
            OnUpdateCGFront?.Invoke(e);
        }
        

        public class UpdateTextSpeedEventArgs : EventArgs
        {
            public float TextSpeed;
            
            public UpdateTextSpeedEventArgs(float textSpeed)
            {
                TextSpeed = textSpeed;
            }
        }
        public static event Action<UpdateTextSpeedEventArgs> OnUpdateTextSpeed;
        public static void InvokeUpdateTextSpeed(UpdateTextSpeedEventArgs e)
        {
            OnUpdateTextSpeed?.Invoke(e);
        }

        public class UpdateCanContinueToNextLineEventArgs : EventArgs
        {
            public bool CanContinueToNextLine;

            public UpdateCanContinueToNextLineEventArgs(bool canContinueToNextLine)
            {
                CanContinueToNextLine = canContinueToNextLine;
            }
        }

        public static event Action<UpdateCanContinueToNextLineEventArgs> OnUpdateCanContinueToNextLine;

        public static void InvokeUpdateCanContinueToNextLine(UpdateCanContinueToNextLineEventArgs e)
        {
            OnUpdateCanContinueToNextLine?.Invoke(e);
        }
        
        public static event Action OnPauseDialogue;
        public static void InvokePauseDialogue()
        {
            OnPauseDialogue?.Invoke();
        }
        public static event Action OnResumeDialogue;
        public static void InvokeResumeDialogue()
        {
            OnResumeDialogue?.Invoke();
        }

    }
}