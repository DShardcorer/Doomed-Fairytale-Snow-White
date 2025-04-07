using System;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class QuestPoint : MonoBehaviour, IInteractable
{
    [Header("Dialogue (Optional)")]
    [SerializeField] private string knotName;

    [Header("Quest Info")]
    [SerializeField] private QuestInfoSO questInfoForPoint;

    private QuestState currentQuestState;
    private QuestIcon questIcon;

    [Header("Config")]
    [SerializeField] private bool isStartPoint;
    [SerializeField] private bool isFinishPoint;

    private void Awake()
    {
        questIcon = GetComponentInChildren<QuestIcon>();
    }

    private void OnEnable()
    {
        QuestEventSystem.OnQuestStateChanged += OnQuestStateChanged;
    }
    private void OnDisable()
    {
        QuestEventSystem.OnQuestStateChanged -= OnQuestStateChanged;
    }

    private void OnQuestStateChanged(Quest quest)
    {
        if (quest.questInfo.QuestName == questInfoForPoint.QuestName)
        {
            currentQuestState = quest.questState;
            questIcon.SetState(currentQuestState, isStartPoint, isFinishPoint);
        }
    }


    public void Interact(Player player)
    {
        if (!knotName.Equals(string.Empty))
        {
            Debug.Log($"Starting dialogue: {knotName}");
            DialogueEventSystem.InvokeEnterDialogue(new DialogueEventSystem.EnterDialogueEventArgs(knotName));
        }
        else
        {

            if (currentQuestState == QuestState.CAN_START && isStartPoint)
            {
                Debug.Log($"Starting quest: {questInfoForPoint.QuestName}");
                QuestEventSystem.InvokeQuestStarted(questInfoForPoint.QuestName);
            }
            else if (currentQuestState == QuestState.CAN_FINISH && isFinishPoint)
            {
                Debug.Log($"Finishing quest: {questInfoForPoint.QuestName}");
                QuestEventSystem.InvokeQuestFinished(questInfoForPoint.QuestName);
            }
            else
            {
                Debug.Log($"Cannot interact with quest point: {questInfoForPoint.QuestName}. Current state: {currentQuestState}");
            }
        }

    }
}
