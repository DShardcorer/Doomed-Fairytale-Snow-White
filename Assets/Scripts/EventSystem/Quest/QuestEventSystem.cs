using System;
using UnityEngine;

public static class QuestEventSystem
{
    public static Action<string> OnQuestStarted;

    public static void InvokeQuestStarted(string questName)
    {
        if (OnQuestStarted != null)
        {
            OnQuestStarted.Invoke(questName);
        }
        else
        {
            Debug.LogWarning($"No listeners for OnQuestStarted event. Quest name: {questName}");
        }
    }

    public static Action<string> OnQuestAdvanced;

    public static void InvokeQuestAdvanced(string questName)
    {
        if (OnQuestAdvanced != null)
        {
            OnQuestAdvanced.Invoke(questName);
        }
        else
        {
            Debug.LogWarning($"No listeners for OnQuestAdvanced event. Quest name: {questName}");
        }
    }

    public static Action<string> OnQuestCompleted;

    public static void InvokeQuestCompleted(string questName)
    {
        if (OnQuestCompleted != null)
        {
            OnQuestCompleted.Invoke(questName);
        }
        else
        {
            Debug.LogWarning($"No listeners for OnQuestCompleted event. Quest name: {questName}");
        }
    }

    public static Action<Quest> OnQuestStateChanged;
    public static void InvokeQuestStateChanged(Quest quest)
    {
        if (OnQuestStateChanged != null)
        {
            OnQuestStateChanged.Invoke(quest);
        }
        else
        {
            Debug.LogWarning($"No listeners for OnQuestStateChanged event. Quest name: {quest.questInfo.QuestName}");
        }
    }



    public class QuestStepStateChangedEventArgs : EventArgs
    {
        public string QuestName;
        public int QuestStepIndex;
        public QuestStepState QuestStepState;
        public QuestStepStateChangedEventArgs(string questName, int questStepIndex, QuestStepState questStepState)
        {
            QuestName = questName;
            QuestStepIndex = questStepIndex;
            QuestStepState = questStepState;
        }
    }

    public static Action<QuestStepStateChangedEventArgs> OnQuestStepStateChanged;

    public static void InvokeQuestStepStateChanged(QuestStepStateChangedEventArgs e)
    {
        if (OnQuestStepStateChanged != null)
        {
            OnQuestStepStateChanged.Invoke(e);
        }
        else
        {
            Debug.LogWarning($"No listeners for OnQuestStepStateChanged event. Quest name: {e.QuestName}, Step index: {e.QuestStepIndex}");
        }
    }
}
