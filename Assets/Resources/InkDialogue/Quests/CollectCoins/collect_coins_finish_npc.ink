=== collectCoinsFinish ===
{CollectCoinsQuestState:
    - "FINISHED": ->finished
    - else: ->default
}
    


= finished
Thank you !#speaker:Hoshiko #sprite:HoshikoSchool_smile #layout:left
->END
= default
Hm ? What do you want?#speaker:Hoshiko #sprite:HoshikoSchool_smile #layout:left
*[Just checking on you.]
    ...Creep.
->END
*{CollectCoinsQuestState == "CAN_FINISH"} [Here are the coins.]
    ~ FinishQuest(CollectCoinsQuestId)
    Oh. The other guy sent you eh ? Thanks.
    Here are the rewards.
->END



-> END