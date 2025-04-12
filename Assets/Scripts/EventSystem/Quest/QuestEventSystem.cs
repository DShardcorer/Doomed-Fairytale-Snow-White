using System;
using QuestSystem;

namespace EventSystem.Quest
{
    public static class QuestEventSystem
    {
        public static event Action<string> OnQuestStarted;
        public static void InvokeQuestStarted(string questId)
        {
            OnQuestStarted?.Invoke(questId);
        }

        public static event Action<string> OnQuestAdvanced;
        public static void InvokeQuestAdvanced(string questId)
        {
            OnQuestAdvanced?.Invoke(questId);
        }

        public static event Action<string> OnQuestFinished;
        public static void InvokeQuestFinished(string questId)
        {
            OnQuestFinished?.Invoke(questId);
        }

        public static event Action<QuestSystem.Quest> OnQuestStateChanged;
        public static void InvokeQuestStateChanged(QuestSystem.Quest quest)
        {
            OnQuestStateChanged?.Invoke(quest);
        }

        public class QuestStepStateChangedEventArgs : EventArgs
        {
            public string QuestId;
            public int QuestStepIndex;
            public QuestStepState QuestStepState;
            public QuestStepStateChangedEventArgs(string questId, int questStepIndex, QuestStepState questStepState)
            {
                QuestId = questId;
                QuestStepIndex = questStepIndex;
                QuestStepState = questStepState;
            }
        }

        public static event Action<QuestStepStateChangedEventArgs> OnQuestStepStateChanged;
        public static void InvokeQuestStepStateChanged(QuestStepStateChangedEventArgs e)
        {
            OnQuestStepStateChanged?.Invoke(e);
        }
    }
}
