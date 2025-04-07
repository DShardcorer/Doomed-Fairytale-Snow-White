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
Will you collect 5 coins and bring them to my clone over there ? This is for experimentation, don't ask why i make such a dumb request.
So, what is your answer ?
*[Yes]
    ~StartQuest("CollectCoinsQuest")
    Cool!
*[No]
    Come back if you change your mind. Which i know you will.
    
-This is a gather. It converges all the choices to a end. You dont have to write the "ending" for every one of them.
-> END

= inProgress
How is the coins collecting going ?
-> END

= canFinish
You're done ? Cool ! Go talk to my clone over there to get the rewards.
-> END

= finished
Thanks. The rewards are over there though.
-> END