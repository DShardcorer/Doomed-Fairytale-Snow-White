using System;
using DefaultNamespace.EventSystem.Input;
using EventSystem.Dialogue;
using GeneralManagers;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Utility.Input
{
    public class InputFieldGrabber : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private TextMeshProUGUI placeholderText;
        [FormerlySerializedAs("inputFieldPurpose")] [SerializeField] private InputPurpose inputFieldInputPurpose = InputPurpose.PlayerName;

        public enum InputPurpose
        {
            PlayerName
        }

        private string _input;
        public string Input => _input;

        private void Awake()
        {
            TextInputEventSystem.OnOpenTextInputter += OpenTextInputter;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            TextInputEventSystem.OnOpenTextInputter -= OpenTextInputter;
        }

        private void OpenTextInputter(TextInputEventSystem.OpenTextInputterEventArgs obj)
        {
            Debug.LogWarning("OpenTextInputter called");
            gameObject.SetActive(true);
            SetPlaceholderText(obj.PlaceholderText);
            inputFieldInputPurpose = obj.InputPurpose;
            DialogueEventSystem.InvokePauseDialogue();
        }

        private void SetPlaceholderText(string text)
        {
            placeholderText.text = text;
        }

        private void OnEnable()
        {
            inputField.onEndEdit.AddListener(OnInputFieldEndEdit);
        }

        private void OnDisable()
        {
            inputField.onEndEdit.RemoveListener(OnInputFieldEndEdit);
        }


        public void OnInputFieldEndEdit(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return;
            }

            _input = input;
            if (inputFieldInputPurpose == InputPurpose.PlayerName)
            {
                GameManager.Instance.PlayerManager.Player.Profile.SetName(_input);
            }
            DialogueEventSystem.InvokeResumeDialogue();
            gameObject.SetActive(false);
        }
    }
}