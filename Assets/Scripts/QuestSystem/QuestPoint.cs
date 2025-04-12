using DialogueSystem;
using Entity.Player;
using EventSystem.Dialogue;
using EventSystem.Quest;
using InteractInterface;
using UnityEngine;

namespace QuestSystem
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class QuestPoint : MonoBehaviour, IInteractable, IHasDialogue
    {
        [Header("Dialogue (Optional)")]
        [SerializeField] private string knotName;
        [SerializeField] private TextAsset inkDialogueFile;

        [Header("Quest Info")]
        [SerializeField] private QuestInfoSO questInfoForPoint;

        private QuestState currentQuestState;
        private QuestIcon questIcon;

        [Header("Config")]
        [SerializeField] private bool isStartPoint;
        [SerializeField] private bool isFinishPoint;

        public int Priority => 20;

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
            if (inkDialogueFile != null)
            {
                DialogueEventSystem.InvokeEnterDialogue(new DialogueEventSystem.EnterDialogueEventArgs(inkDialogueFile, knotName));
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

        public TextAsset GetInkDialogueFile()
        {
            return inkDialogueFile;
        }
    }
}
