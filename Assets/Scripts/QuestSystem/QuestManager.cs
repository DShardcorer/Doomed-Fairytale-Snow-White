using System;
using System.Collections;
using System.Collections.Generic;
using Events.Player;
using UnityEngine;

public class QuestManager : MonoBehaviour, ILifecycle<GameManager>
{
    private GameManager gameManager;
    private Dictionary<string, Quest> questMap;

    private int currentLevel;
    private Coroutine updateQuestRequirementsCoroutine;

    private Dictionary<string, Quest> CreateQuestMap()
    {
        Dictionary<string, Quest> questMap = new Dictionary<string, Quest>();
        QuestInfoSO[] questInfoSOs = Resources.LoadAll<QuestInfoSO>(HelperResourcePath.Quests);
        foreach (QuestInfoSO questInfoSO in questInfoSOs)
        {
            Quest quest = new Quest(questInfoSO);
            questMap.Add(quest.questInfo.QuestName, quest);
        }
        return questMap;
    }
    public void Initialize(GameManager parent)
    {
        gameManager = parent;
        questMap = CreateQuestMap();
        QuestEventSystem.OnQuestStarted += StartQuest;
        QuestEventSystem.OnQuestAdvanced += AdvanceQuest;
        QuestEventSystem.OnQuestCompleted += FinishQuest;
        PlayerLevelEventSystem.OnLevelChanged += OnLevelChanged;
        PlayerLevelEventSystem.OnInitialLevelSet += OnLevelChanged;

        foreach (var quest in questMap.Values)
        {
            QuestEventSystem.InvokeQuestStateChanged(quest);
        }
        updateQuestRequirementsCoroutine = StartCoroutine(UpdateQuestRequirements());

    }
    public void Dispose()
    {
        gameManager = null;
        questMap.Clear();
        questMap = null;
        QuestEventSystem.OnQuestStarted -= StartQuest;
        QuestEventSystem.OnQuestAdvanced -= AdvanceQuest;
        QuestEventSystem.OnQuestCompleted -= FinishQuest;
        Destroy(gameObject);
    }


    private void OnLevelChanged(object sender, OnLevelChangedEventArgs e)
    {
        currentLevel = e.Level;
    }

    public void ChangeQuestState(string questName, QuestState newState)
    {
        if (questMap.TryGetValue(questName, out Quest quest))
        {
            quest.questState = newState;
            QuestEventSystem.InvokeQuestStateChanged(quest);
        }
        else
        {
            Debug.LogWarning($"Quest with name {questName} not found.");
        }
    }
    private bool IsRequirementMet(Quest quest)
    {
        bool isMet = true;

        if (quest.questInfo.levelRequirement > currentLevel)
        {
            isMet = false;
        }

        foreach (QuestInfoSO prerequisiteQuestInfo in quest.questInfo.questPrerequisites)
        {
            if (questMap.TryGetValue(prerequisiteQuestInfo.QuestName, out Quest prerequisiteQuest))
            {
                if (prerequisiteQuest.questState != QuestState.FINISHED)
                {
                    isMet = false;
                }
            }
            else
            {
                Debug.LogWarning($"Prerequisite quest {prerequisiteQuestInfo.QuestName} not found.");
            }
        }

        return isMet;

    }
    private IEnumerator UpdateQuestRequirements()
    {
        while (true)
        {
            foreach (var quest in questMap.Values)
            {
                if (quest.questState == QuestState.REQUIREMENTS_NOT_MET && IsRequirementMet(quest))
                {
                    quest.questState = QuestState.CAN_START;
                    QuestEventSystem.InvokeQuestStateChanged(quest);
                }
            }
            yield return new WaitForSecondsRealtime(2f);
        }
    }
    private void StartQuest(string questName)
    {
        Quest quest = GetQuestByQuestName(questName);
        if (quest != null)
        {
            quest.InstantiateCurrentQuestStep(transform);
            ChangeQuestState(questName, QuestState.IN_PROGRESS);
        }
        else
        {
            Debug.LogWarning($"Quest with name {questName} not found.");
        }

    }

    private void AdvanceQuest(string questName)
    {
        Quest quest = GetQuestByQuestName(questName);
        if (quest != null)
        {
            quest.MoveToNextStep();
            if (quest.CurrentQuestStepExists())
            {
                quest.InstantiateCurrentQuestStep(transform);
            }
            else
            {
                ChangeQuestState(questName, QuestState.CAN_FINISH);
            }
        }
        else
        {
            Debug.LogWarning($"Quest with name {questName} not found.");
        }
    }
    private void FinishQuest(string questName)
    {
        Quest quest = GetQuestByQuestName(questName);
        if (quest != null)
        {
            ChangeQuestState(questName, QuestState.FINISHED);
            quest.GiveRewards();
        }
        else
        {
            Debug.LogWarning($"Quest with name {questName} not found.");
        }
    }










    private Quest GetQuestByQuestName(string questName)
    {
        if (questMap.TryGetValue(questName, out Quest quest))
        {
            return quest;
        }
        else
        {
            Debug.LogWarning($"Quest with name {questName} not found.");
            return null;
        }
    }
}
