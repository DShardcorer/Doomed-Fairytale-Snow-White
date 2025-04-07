=== collectCoinsFinish ===
{CollectCoinsQuestState:
    - "FINISHED": ->finished
    - else: ->default
}
    


= finished
Thank you !
->END
= default
Hm ? What do you want?
*[Just checking on you.]
    ...Creep.
->END
*{CollectCoinsQuestState == "CAN_FINISH"} [Here are the coins.]
    ~ FinishQuest(CollectCoinsQuestId)
    Oh. The other guy sent you eh ? Thanks.
    Here are the rewards.
->END



-> END