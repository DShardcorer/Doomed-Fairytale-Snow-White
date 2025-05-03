INCLUDE ../global_variables.ink
INCLUDE ../global_methods.ink
===IntroScene===
=begin
of them all~~ #cgpath:IntroScene #cg:Default
Oh? Finally, you're awake. I almost feel bad interrupting your sound sleep. #speaker:Mysterious Woman
* [Who are you?]
* [Where am I?]

-Shhh. Questions are for later. All I can say is that you’re in a place where you should be.
~OpenTextInputter("Your name ?", "PlayerName")
It doesn't matter, not as much as who you are...

Do you remember who you are?

*[...No.]

-Don't worry. It's supposed to be that way, {PlayerName}

*[What do you mean by that?]
*[Then who am I?]

-Tch, questions again. Impatient, aren't we? Being you, that's how it should be.  #cg:SmileShyly

It takes a while to explain, but for now, just know that you're now a protagonist.  #cg:Default

* Sorry, I may have misheard that?
* Excuse me... what?

-A hero. A leading character in a tale. Lord, you don't have stories where you come from? #speaker:Mysterious Woman

* Stories...then I guess you too are a character of some kind?
* Can you tell me who you are at least?

-For now, I can only tell you that I'm a kind of divinity. #speaker:Mysterious Woman

* Yeah, cannot argue with that...
    You can, but I can see you're the type that won't waste your time fighting a losing battle. We're fine, then.
* Do some magic tricks then
    I hope the little trick is enough to convince you. #speaker:Mysterious Woman #cg:Laugh #cgfront:FrontBlue
-
* Yes
    Then let’s continue. Shall we go along and humor each other a bit? 
    -> quiz1

* No
    I've done my part, if it can't convince you, let the rest of the world do it. Shall we play along and humor each other a bit? #speaker:Mysterious Woman
    -> quiz1

= quiz1

What alias do you want to be called by? We're starting fresh, after all.

~OpenTextInputter("Your name ?", "PlayerName")
->IntroScene2

===IntroScene2===
{PlayerName} ?A lovely choice.

Then... let me tell you a story. One that I think you may find familiar.

There was this princess, hair of the ebony, skin of snow, and the smiles on her lips bloom as red as the loveliest of roses. She's known as, you may have guessed, the fairest of them all.

Almost everyone treasured her. Almost, aside from the Queen, her stepmother. Who was also a great mage. Who wanted to be the "fairest of them all" above everything.

And so, the Queen wanted the princess dead. For such a reason. Petty, I daresay?

* Yes
    Someone that petty coming to such a great power. My, my, you're as naive as I thought. 
    But it's just a minor detail. Let's go on.

* Royals are never so simple. There must be something else. 
    You're quite perceptive, aren't you?
    But that's just a minor detail, let's continue. //+Perception skill

-In response to that, what do you think the girl should do?

* Fight back
    A very straightforward approach. I like your spirits.
    But what could a small girl do against the all-powerful queen, doused to the teeth with magical prowess?

* Expose the Queen intention to her father.
    Really, depending on the King? Why do you think the queen get so powerful? Her Father was a good parent, wasn't that much of a ruler. 

* Multilate her own face
    ...Seems like a wise choice. I wish life would be so easy. 

* Escape to the woods.
    A realistic approach. A little girl raised in wealth and admiration, now alone in the place where life's boiled down to mere survival.

-But, what do you think the girl chose?

* Fight back
* Expose the queen
* Multilate her own face
* Escape
* Another choice?

-...She chose all of them. She ran off to the wood with her face changed. Not with the naive mind of merely survive, but rather the strong will to stay safe until she gathered enough power for a back strike.
But to no avail. #cg:Sad
This is where the dwarves come into play. They are hunted by the queendom for certain reasons. The girl ran into them during her escape. And they somehow managed to live together. What a fate, I daresay? #cg:Default

What did the princess do to fit in? Your take?

* Learn how to fight.
    Yes. How else would she survive? With now the whole queendom on her tail. But you shouldn't just fight the ones who are now your housemates.

* Do chores for them
    The obvious choice. If only it were that simple. This isn't a tale where a song can save you and mere chores can keep your head on your neck.

* What she needed to do in that situation. You know?
    Dirty-minded, aren't you?... But you're not entirely wrong either. #cg:Blush

-Well, you might have guessed the answer by now. Again, she did all of them. There wasn't any other way.

Let's move on, then.

//Play knock sound effect

...

There were more things like the poison apple or the mad prince that I'd love to continue on. But our time is up. So this will be our final question.

...If you had the power to change fates, would you help the girl?

    * Yes //+xtra Sanity
    * I would hog the power first. //+xtra stat

-I see... #cg:Sad

I feel my eyelids heavy and the sleep's about to take over me.

I may have known what I need to know about you, and I can only wish you the best. It’s about time for your journey to start.

Be rested and aware, for the paths ahead will be darker and rougher than you may ever know.

One final thing.

...No matter how desperate it gets, please don't lose hope. #speaker:Mysterious Woman #cg: sad

-> END
