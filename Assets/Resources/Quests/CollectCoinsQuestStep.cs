using EventBus.Misc;
using QuestSystem;

namespace Resources.Quests
{
    public class CollectCoinsQuestStep : QuestStep
    {
        private int coinsCollected = 0;

        private int coinsToCollect = 3;


        private void OnEnable()
        {
            MiscEventSystem.CoinCollected += OnCoinCollected;
        }

        private void OnCoinCollected()
        {
            coinsCollected++;
            UpdateState();
            if (coinsCollected >= coinsToCollect)
            {
                FinishQuestStep();
            }
        }
        private void OnDisable()
        {
            MiscEventSystem.CoinCollected -= OnCoinCollected;
        }

        private void UpdateState()
        {
            string state = coinsCollected.ToString();
            QuestStepState questStepState = new QuestStepState(state);
            ChangeState(questStepState);
        }
    }
}
