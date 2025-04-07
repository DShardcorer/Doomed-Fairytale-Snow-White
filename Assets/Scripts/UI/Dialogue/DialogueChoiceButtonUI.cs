using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueChoiceButtonUI : MonoBehaviour, ISelectHandler
{
    [Header("Components")]

    [SerializeField] private Button choiceButton;
    [SerializeField] private TextMeshProUGUI choiceText;
    private int choiceIndex = -1;
    public int ChoiceIndex => choiceIndex;

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log($"Selected choice button with index: {choiceIndex}");
        DialogueEventSystem.InvokeUpdateChoiceIndex(new DialogueEventSystem.UpdateChoiceIndexEventArgs(choiceIndex));
    }

    public void SelectButton()
    {
        choiceButton.Select();
    }

    public void SetChoiceText(string text)
    {
        choiceText.text = text;
    }
    public void SetChoiceIndex(int index)
    {
        choiceIndex = index;
    }


}
