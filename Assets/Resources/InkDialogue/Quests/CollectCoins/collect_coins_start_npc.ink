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
Uh. I still have standards you know ? Shoo, shoo. #speaker:Hoshiko #sprite:HoshikoSchool_upset #layout:left
-> END


= canStart
Oh, Hello ? #speaker:Hoshiko #sprite:HoshikoSchool_smile #layout:left
Will you please collect 3 coins and bring them to my clone over there ? This is for experimentation, don't ask why i make such a dumb request.
This is Monika by the way. #cg:monika
So, what is your answer ?  #cg:null
*[No]
*[...]
-Pretty please ?I'll show you my <b><color=\#FF1493>swimsuit</color></> #speaker:Hoshiko #sprite:HoshikoSchool_embarrassed1 #layout:left
*[Yes]
    ~StartQuest("CollectCoinsQuest")
    Cool!
    Feast your eyes upon my glorious self ! #speaker:Hoshiko clone in swimsuit #sprite:HoshikoSwim_embarrassed2 #layout:right
    Alright, that's enough, clone. Now go fetch me, i mean my clone some coins !#speaker:Hoshiko #sprite:HoshikoSchool_embarrassed1 #layout:left
    
    
*[No]
    Come back if you change your mind. Which i know you will.
--> END

= inProgress
How is the coins collecting going ?#speaker:Hoshiko #sprite:HoshikoSchool_smile #layout:left
-> END

= canFinish
You're done ? Cool ! Go talk to my clone over there to get the rewards.#speaker:Hoshiko #sprite:HoshikoSchool_smile #layout:left
-> END

= finished
Thanks. The rewards are over there though.#speaker:Hoshiko #sprite:HoshikoSchool_smile #layout:left
-> END