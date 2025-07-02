INCLUDE ../global_variables.ink

INCLUDE ../global_methods.ink

==RandomMan==

Well now... didn’t expect to see anyone brave enough to stroll out this far. You lost, or just curious? #speaker:RandomMan #sprite:HoshikoSchool_default #layout:right

* [Just curious.]

    Curiosity, huh? That’s a dangerous trait around these parts. #sprite:HoshikoSchool_smile

    But I like it. Means you’re still alive inside.

* [Do you live out here?]

    Live? Survive is more accurate. But yeah. This patch of dirt is mine. No neighbors, no noise, just me and the occasional raccoon fight. #sprite:HoshikoSchool_sweat

* [You seem pretty sharp. What’s your story?]

    My story? Hah. You got a few hours? #sprite:HoshikoSchool_embarassed2

    Let’s just say I used to be someone else. Now I’m someone who avoids towns and keeps a knife under the pillow.

* [You look like you've seen things.]

    That obvious, huh? #sprite:HoshikoSchool_thoughtful

    Yeah. I’ve seen peace fall apart, friends turn, and promises rot. But I’ve also seen sunrises worth living for. Balance, I guess.

* [Got any advice for someone passing through?]

    Sure. Trust your gut, not your map. And don’t eat anything that hums. #sprite:HoshikoSchool_smile

    Also, don’t look too long into mirrors out here. Sometimes they look back.

* [You’re surprisingly well-spoken… for someone dressed like a rag pile. (Charisma)]

    ~ temp charismaReq = 17

    {PlayerCharisma < charismaReq:

        Ouch. You try to compliment me or insult me, traveler? #sprite:HoshikoSchool_upset

        Next time, leave out the “rag pile” part.

    - else:

        Hah! You’ve got a sharp tongue and a sharp eye. I respect that. #sprite:HoshikoSchool_embarassed2

        Most people just call me a weirdo and walk away.

    }

* [Do you know anything about the old ruins nearby?]

    Ruins? Yeah. I’ve poked around the edges. Bad energy. Real cold, even in the sun. #sprite:HoshikoSchool_serious

    Some say it’s haunted. Me? I think it remembers. And it’s waiting.

* [You’re kinda charming, in a mysterious way. (Charisma)]

    ~ temp charismaReq2 = 18

    {PlayerCharisma < charismaReq2:

        Heh. Not buying it, but nice try. You’re smooth, I’ll give you that. #sprite:HoshikoSchool_smile

    - else:

        Oh? Now *you’re* just trying to make me blush. It’s working. #sprite:HoshikoSchool_embarassed2

        Careful, traveler. Flattery opens more than doors.

    }

-->END
