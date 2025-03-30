using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{   private Quest _quest;
    public Quest Quest => _quest;
    private bool isFinished = false;

    public bool IsFinished => isFinished;

    public void Initialize(Quest quest)
    {
        _quest = quest;
    }
    protected void FinishQuestStep()
    {
        if (!isFinished)
        {
            isFinished = true;
            QuestEventSystem.InvokeQuestAdvanced(_quest.questInfo.QuestName);
            Destroy(gameObject);
        }
    }
}
