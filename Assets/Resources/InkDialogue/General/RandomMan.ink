INCLUDE ../global_variables.ink
INCLUDE ../global_methods.ink

==RandomMan==
~ temp charismaReq = 20

Oh, hello? What do you need? #speaker:Random Man #sprite:hoshi_school_smile #layout:left

* [Nothing]
    <speed=2> Okay...? </speed> This is awkward. #sprite:hoshi_school_surprised

* [Just wondering why you are walking around naked?]
    Why are you walking around in clothes? You're really thick-faced, mister. #sprite:hoshi_school_upset

* [Just that you look very handsome... (Charisma)]
    {PlayerCharisma < charismaReq:
        ...get the fuck out, creep. #sprite:hoshi_school_upset
    - else:
        Oh-..Really? Thank you. This made my day. #sprite:hoshi_school_embarassed2
    }
-->END

    
    

