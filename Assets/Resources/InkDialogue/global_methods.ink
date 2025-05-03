//Player Stuff
EXTERNAL SetPlayerName(playerName)
EXTERNAL SetPlayerStat(statType, amount)
EXTERNAL AddPlayerStat(statType, amount)
EXTERNAL AddPlayerActiveSkill(skillId)
EXTERNAL AddPlayerPassiveSkill(skillId)
EXTERNAL AddPlayerItem(itemId, amount)


//TextInputter
EXTERNAL OpenTextInputter(placeholderText, inputPurpose)

//Audio
EXTERNAL PlaySFX(sfxName)


//Quest stuff
EXTERNAL StartQuest(questId)
EXTERNAL AdvanceQuest(questId)
EXTERNAL FinishQuest(questId)
