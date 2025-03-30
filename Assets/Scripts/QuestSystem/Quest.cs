using System;
using UnityEngine;

public class Quest : ILifecycle<QuestManager>
{
    private QuestManager _questManager;
    public QuestManager QuestManager => _questManager;

    public QuestInfoSO questInfo;

    public QuestState questState;

    private int currentQuestStepIndex;

    public Quest(QuestInfoSO questInfo)
    {
        this.questInfo = questInfo;

    }

    public void Initialize(QuestManager parent)
    {
        _questManager = parent;
        questState = QuestState.REQUIREMENTS_NOT_MET;
        currentQuestStepIndex = 0;
    }

    public void Dispose()
    {
        _questManager = null;
        questInfo = null;
        currentQuestStepIndex = 0;
    }

    public void MoveToNextStep()
    {
        currentQuestStepIndex++;
    }
    public bool CurrentQuestStepExists()
    {
        return currentQuestStepIndex < questInfo.questStepPrefabs.Length;
    }
    public void InstantiateCurrentQuestStep(Transform parent)
    {
        GameObject questStepPrefab = GetCurrentQuestStepPrefab();
        if (questStepPrefab != null)
        {
            GameObject questStepInstance = GameObject.Instantiate(questStepPrefab, parent);
            questStepInstance.GetComponent<QuestStep>().Initialize(this);

        }
    }

    private GameObject GetCurrentQuestStepPrefab()
    {
        GameObject questStepPrefab = null;
        if (CurrentQuestStepExists())
        {
            questStepPrefab = questInfo.questStepPrefabs[currentQuestStepIndex];
        }
        else
        {
            Debug.LogWarning("No more quest steps available");
        }
        return questStepPrefab;
    }

    public void GiveRewards()
    {
        //Put reward into inventory
        Debug.Log($"Quest {questInfo.QuestName} completed! Rewards given.");
    }
}
