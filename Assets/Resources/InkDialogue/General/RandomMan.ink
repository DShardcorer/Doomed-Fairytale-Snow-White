INCLUDE ../global_variables.ink
INCLUDE ../global_methods.ink

==RandomMan==
~ temp charismaReq = 20

Oh, hello? What do you need? #speaker:Random Man #sprite:HoshikoSchool_smile #layout:left
~OpenTextInputter("Your name ?", "PlayerName")
So, {PlayerName} ?. What can i do for ya ?
* [Nothing]
    <speed=2> Okay...? </speed> This is awkward. #sprite:HoshikoSchool_surprised

* [Just wondering why you are walking around naked?]
    Why are you walking around in clothes? You're really thick-faced, mister. #sprite:HoshikoSchool_upset

* [Just that you look very handsome... (Charisma)]
    {PlayerCharisma < charismaReq:
        ...get the fuck out, creep. #sprite:HoshikoSchool_upset
    - else:
        Oh-..Really? Thank you. This made my day. #sprite:HoshikoSchool_embarassed2
    }
-->END

    
    

