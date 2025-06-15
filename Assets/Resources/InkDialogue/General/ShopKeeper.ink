INCLUDE ../global_variables.ink

INCLUDE ../global_methods.ink

==ShopKeeper==

Oh, a customer? Welcome, welcome! What are you looking for today? #speaker:Shopkeeper #sprite:HoshikoSchool_smile #layout:left

* [Just browsing, thanks.]

    No pressure at all. Take your time. #sprite:HoshikoSchool_smile

* [Got anything... special in stock?]

    Special? Hah, depends how deep your pockets are. #sprite:HoshikoSchool_smile
    ~StartBarter()

* [You're quite charming for a shopkeeper... (Charisma)]

    ~ temp charismaReq = 18

    {PlayerCharisma < charismaReq:

        Nice try, but flattery doesn’t get you a discount. #sprite:HoshikoSchool_upset

    - else:

        Oh my... You're not so bad yourself, darling. #sprite:HoshikoSchool_embarassed2

    }

-->END
