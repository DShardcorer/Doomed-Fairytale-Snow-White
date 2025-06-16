//Player Stuff
EXTERNAL SetPlayerName(playerName)
EXTERNAL SetPlayerStat(statType, amount)
EXTERNAL AddPlayerStat(statType, amount)
EXTERNAL AddPlayerActiveSkill(skillId)
EXTERNAL AddPlayerPassiveSkill(skillId)
EXTERNAL AddPlayerItem(itemId, amount)

//Barter
EXTERNAL StartBarter()

//TextInputter
EXTERNAL OpenTextInputter(placeholderText, inputPurpose)

//Audio
EXTERNAL PlaySFX(sfxName)

//Fade
EXTERNAL Fade(fadeInDuration, fadeOutDuration)

//Switch Scene
EXTERNAL SwitchScene(sceneToLoad, portalToSpawnAt)

//Quest stuff
EXTERNAL StartQuest(questId)
EXTERNAL AdvanceQuest(questId)
EXTERNAL FinishQuest(questId)


