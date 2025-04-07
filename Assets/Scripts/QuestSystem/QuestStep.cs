using UnityEngine;


public abstract class QuestStep : MonoBehaviour
{
    private Quest _quest;
    public Quest Quest => _quest;

    private int stepIndex;
    private bool isFinished = false;

    public bool IsFinished => isFinished;

    public void Initialize(Quest quest)
    {
        _quest = quest;
        stepIndex = this._quest.CurrentQuestStepIndex;
        Debug.Log($"Quest: {_quest}");

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

    protected void ChangeState(QuestStepState newState)
    {
        QuestEventSystem.QuestStepStateChangedEventArgs e =
        new QuestEventSystem.QuestStepStateChangedEventArgs(_quest.questInfo.QuestName, stepIndex, newState);
        QuestEventSystem.InvokeQuestStepStateChanged(e);
    }

}
