=== collectCoinsFinish ===

{CollectCoinsQuestState:

    - "FINISHED": ->finished

    - else: ->default

}

= finished

Thank you! #speaker:VillageWoman #sprite:VillageWoman_laugh #layout:left

-> END

= default

Hm? What do you want? #speaker:VillageWoman #sprite:VillageWoman_default #layout:left

*[Just checking on you.]

    ...Creep.

-> END

*{CollectCoinsQuestState == "CAN_FINISH"} [Here are the coins.]

    ~ FinishQuest(CollectCoinsQuestId)

    Oh. The other guy sent you, eh? Thanks.

    Here are the rewards. #speaker:VillageWoman #sprite:VillageWoman_laugh #layout:left

-> END
