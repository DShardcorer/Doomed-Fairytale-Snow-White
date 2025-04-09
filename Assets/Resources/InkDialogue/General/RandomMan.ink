INCLUDE ../global_variables.ink
INCLUDE ../global_methods.ink

==RandomMan==
~ temp charismaReq = 10

Oh, hello? What do you need?

* [Nothing]
    Okay...?

* [Just wondering why you are walking around naked?]
    Why are you walking around in clothes? You're really thick-faced, mister.

* [Just that you look very handsome... (Charisma)]
    {PlayerCharisma < charismaReq:
        ...get the fuck out, creep.
    - else:
        Oh-..Really? Thank you. This made my day.
    }
-->END

    
    

