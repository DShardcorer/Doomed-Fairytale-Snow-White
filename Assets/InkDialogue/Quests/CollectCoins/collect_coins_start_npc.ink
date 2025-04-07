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
Uh. I still have standards you know ? Shoo, shoo.
-> END


= canStart
Oh, Hello ? #speaker:Hoshiko #sprite:hoshi_school_smile #layout:left
Will you please collect 3 coins and bring them to my clone over there ? This is for experimentation, don't ask why i make such a dumb request.
So, what is your answer ? 
*[No]
*[...]
-Pretty please ?I'll show you my swimsuit ? #speaker:Hoshiko #sprite:hoshi_school_embarassed1 #layout:left
*[Yes]
    ~StartQuest("CollectCoinsQuest")
    Cool!
    Feast your eyes upon my glorious self ! #speaker:Hoshiko clone in swimsuit #sprite:hoshi_swim_embarassed2 #layout:right
    Alright, that's enough, clone. Now go fetch me, i mean my clone some coins !#speaker:Hoshiko #sprite:hoshi_school_embarassed1 #layout:left
    
    
*[No]
    Come back if you change your mind. Which i know you will.
--> END

= inProgress
How is the coins collecting going ?
-> END

= canFinish
You're done ? Cool ! Go talk to my clone over there to get the rewards.
-> END

= finished
Thanks. The rewards are over there though.
-> END