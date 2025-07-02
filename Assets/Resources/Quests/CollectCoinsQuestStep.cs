using System.Runtime.CompilerServices;
using EventBus.Misc;
using QuestSystem;
using UnityEngine;

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
            Debug.LogWarning($"Coin collected! Total coins: {coinsCollected}/{coinsToCollect}");
            if (coinsCollected >= coinsToCollect)
            {
                FinishQuestStep();
                Debug.LogWarning($"Quest step completed: {coinsCollected} coins collected.");
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
