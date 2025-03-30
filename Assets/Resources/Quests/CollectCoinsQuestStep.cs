using UnityEngine;
using Events.Misc;
using System;
public class CollectCoinsQuestStep : QuestStep
{
    private int _coinsCollected = 0;

    private int _coinsToCollect = 3;


    private void OnEnable()
    {
        MiscEventSystem.CoinCollected += OnCoinCollected;
    }

    private void OnCoinCollected()
    {
        _coinsCollected++;
        if (_coinsCollected >= _coinsToCollect)
        {
            FinishQuestStep();
        }
    }
    private void OnDisable()
    {
        MiscEventSystem.CoinCollected -= OnCoinCollected;
    }
}
