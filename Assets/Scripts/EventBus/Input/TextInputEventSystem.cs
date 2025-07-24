using System;
using Utilities.Input;

namespace DefaultNamespace.EventSystem.Input
{
    public class TextInputEventSystem
    {
        public class TextInputCompleteEventArgs : EventArgs
        {
            public string InputText;

            public TextInputCompleteEventArgs(string inputText)
            {
                InputText = inputText;
            }
        }

        public static event Action<TextInputCompleteEventArgs> OnTextInputComplete;

        public static void InvokeTextInputComplete(string inputText)
        {
            OnTextInputComplete?.Invoke(new TextInputCompleteEventArgs(inputText));
        }

        public class OpenTextInputterEventArgs : EventArgs
        {
            public string PlaceholderText;
            public InputFieldGrabber.InputPurpose InputPurpose;

            public OpenTextInputterEventArgs(string placeholderText, InputFieldGrabber.InputPurpose inputPurpose)
            {
                PlaceholderText = placeholderText;
                InputPurpose = inputPurpose;
            }
        }

        public static event Action<OpenTextInputterEventArgs> OnOpenTextInputter;

        public static void InvokeOpenTextInputter(string placeholderText, string inputPurpose)
        {
            OnOpenTextInputter?.Invoke(new OpenTextInputterEventArgs(placeholderText,
                (InputFieldGrabber.InputPurpose)Enum.Parse(typeof(InputFieldGrabber.InputPurpose), inputPurpose)));
        }
    }
}