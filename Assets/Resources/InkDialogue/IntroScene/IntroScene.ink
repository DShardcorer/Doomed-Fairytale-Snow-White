INCLUDE ../global_variables.ink
INCLUDE ../global_methods.ink
===IntroScene===
I open my eyes. The first thing I see is darkness. Then I recognize the light from the moon, which then reveals a woman. #cgpath:IntroScene #cg:Main #delay:5

of them all...Oh? Finally, you're awake. I almost feel bad interrupting your sound sleep. #speaker:Mysterious Woman 

* [Who are you?]
* [Where am I?]

-Shhh. Leave the questions for later. All I can say is that you’re in a place where you should be.

It doesn't matter, not as much as who you are...

On that note, do you remember who you are? #speaker:Mysterious Woman

*[...No.]
-Don't worry. It's supposed to be that way.

* [What do you mean by that?]
* [Then who am I?]

-Tch, questions again. Impatient, aren't we? But I suppose it's understandable being in your shoes. #speaker:Mysterious Woman

It takes a while to explain, but for now, just know that you're now the protagonist of this story. #speaker:Mysterious Woman

* [Sorry, I may have misheard that?]
* [Excuse me... what?]

-A hero. A leading character in a tale. My lord, you don't have stories where you come from? #speaker:Mysterious Woman

*[...I guess you too are a character of some kind?]
* [Can you tell me who you are at least?]

-For now, I can only tell you that I'm a kind of divinity. #speaker:Mysterious Woman

* [Yeah, cannot argue with that...]
    -> MWdialogue1
* [Do some magic trick]
    -> MWdialogue2

= MWdialogue1

You can, but I can see you're the type that won't waste your time fighting a losing battle. We're fine, then. #speaker:Mysterious Woman

-> yesno_choice

= MWdialogue2

The moonlight in the room changes into blue and then purple.

I hope it's convincing enough. #speaker:Mysterious Woman

-> yesno_choice

= yesno_choice

* [Yes]
    Then let’s continue. Shall we go along and humor each other a bit? #speaker:Mysterious Woman
    -> quiz1

* [No]
    I've done my part, if it can't convince you, let the rest of the world do it. Shall we play along and humor each other a bit? #speaker:Mysterious Woman
    -> quiz1

= quiz1

What alias do you want to be called by? We're starting fresh, after all.

- Protag enters name

A lovely choice. #speaker:Mysterious Woman

Then... let me tell you a story. One that I'd like to call "Snow White and the Seven Dwarves".

There was this princess, a girl so beautiful with hair as black as ebony, lips as red as the rose, skin as white as snow. The fairest of them all.

Almost everyone treasured her. All but her stepmother, who wanted to be the "fairest of them all".

And so, she wanted her dead. For such a reason. Quite petty, right?

* [Yes]
    ...Indeed.

* [There must be something behind it.]
    You're quite perceptive, aren't you?
    But that's just a minor detail, let's continue. //+Perception skill

-In response to that, what do you think the girl should do?

* [Fight back]
    A very straightforward approach. I like your spirits.
    But what could a small girl do against the all-powerful queen, doused to the teeth with magical prowess?

* [Expose the queen's intention to her father.]
    A wise choice, but no one would believe her. Nor could they do a thing. The whole queendom was under the queen's palm.

* [Mutilate her own face]
    ...Would you believe that the girl would stay the fairest even with many scars on her face?

* [Escape to the woods.]
    A realistic approach. Saving your skin at the cost of living your whole future in fear and seclusion.

-But, what do you think the girl chose?

* [Fight back]
* [Expose the queen]
* [Mutilate her own face]
* [Escape]
* [Another choice?]

-...She chose all of them. But to no avail.

This is where the dwarves come into play. They are hunted by the queendom for certain reasons. The girl ran into them during her escape. And they somehow managed to live together.

What did the girl do to fit in? Your take?

* [Learn how to fight.]
    Yes. How else would she survive? With now the whole queendom on her tail. But that's not all.

* [Do housework]
    The obvious choice. If only it was that simple. This isn't a fairytale.

* [...The things]
    Dirty-minded, aren't you?... But you're not entirely wrong either. #cg:blush

-Well, you might have guessed the answer by now. Again, she did all of them. There wasn't any other way.

Let's move on, then.

//Play knock sound effect

...

There were more things like the poison apple or the mad prince that I'd love to continue on. But our time is up. So this will be our final question.

...If you had the power to change fates, would you help the girl?

    * [Yes] //+xtra Sanity
    * [I would hog the power first] //+xtra stat

-I see...

I feel my eyelids heavy and the sleep's about to take over me.

I may have known what I need to know about you, and I can only wish you the best. It’s about time for your journey to start.

Be rested and aware, for the paths ahead will be darker and rougher than you may ever know.

One final thing.

...No matter how desperate it gets, please don't lose hope. #speaker:Mysterious Woman #cg: sad

-> END
