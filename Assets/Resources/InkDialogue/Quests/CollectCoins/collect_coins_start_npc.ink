===collectCoinsStart===

{ CollectCoinsQuestState:

    - "REQUIREMENTS_NOT_MET": -> requirementsNotMet

    - "CAN_START": -> canStart

    - "IN_PROGRESS": -> inProgress

    - "CAN_FINISH": -> canFinish

    - "FINISHED": -> finished

    - else: ->END

}

= requirementsNotMet

Uh. I still have standards you know? Shoo, shoo. #speaker:VillageWoman #sprite:VillageWoman_disgust #layout:left

-> END

= canStart

Oh, Hello? #speaker:VillageWoman #sprite:VillageWoman_default #layout:left

Will you please collect 3 coins and bring them to my clone over there? This is for experimentation, don't ask why I make such a dumb request.

So, what is your answer?  #cg:null

*[No]

*[...]

-Pretty please? I'll show you my <b><color=\#FF1493>swimsuit</color></> #speaker:VillageWoman #sprite:VillageWoman_blush #layout:left

*[Yes]

    ~StartQuest("CollectCoinsQuest")

    Cool!

    Feast your eyes upon my glorious self! Yeah, I know. We don't have the budget for actual swimsuits. Just use your imagination. #speaker:VillageWoman clone in swimsuit #sprite:VillageWoman_blush #layout:right

    Alright, that's enough, clone. Now go fetch me, I mean my clone, some coins! #speaker:VillageWoman #sprite:VillageWoman_blush #layout:left

*[No]

    Come back if you change your mind. Which I know you will.

--> END

= inProgress

How is the coins collecting going? #speaker:VillageWoman #sprite:VillageWoman_default #layout:left

-> END

= canFinish

You're done? Cool! Go talk to my clone down the street to get the rewards. #speaker:VillageWoman #sprite:VillageWoman_default #layout:left

-> END

= finished

Thanks. The rewards are over there though. #speaker:VillageWoman #sprite:VillageWoman_laugh #layout:left

-> END
