INCLUDE ../global_variables.ink

INCLUDE ../global_methods.ink

==ShopKeeper==

Oh, a customer? Welcome, welcome! What are you looking for today? #speaker:VillageWoman #sprite:VillageWoman_default #layout:left

* [Just browsing, thanks.]

    No pressure at all. Take your time. #sprite:VillageWoman_default

* [Got anything... special in stock?]

    Special? Hah, depends how deep your pockets are. #sprite:VillageWoman_laugh

    ~StartBarter()

* [You're quite charming for a shopkeeper... (Charisma)]

    ~ temp charismaReq = 18

    {PlayerCharisma < charismaReq:

        Nice try, but flattery doesn’t get you a discount. #sprite:VillageWoman_disgust

    - else:

        Oh my... You're not so bad yourself, darling. #sprite:VillageWoman_blush

    }

--> END
