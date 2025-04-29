using System;
using EntitySystems.Skill;
using EntitySystems.Skill.SkillFactory;
using EntitySystems.Stats;
using EventSystem.Quest;
using GeneralManagers;
using Ink.InkLibs.InkRuntime;
using Item;

namespace DialogueSystem
{
    public class InkExternalFunctions
    {
        public void StartListening(Story story)
        {
            BindQuestFunctions(story);
            BindPlayerFunctions(story);
        }

        public void StopListening(Story story)
        {
            UnbindQuestFunctions(story);
            UnbindPlayerFunctions(story);
        }

        #region PLAYER

        private void BindPlayerFunctions(Story story)
        {
            story.BindExternalFunction("SetPlayerName", (string playerName) => SetPlayerName(playerName));
            story.BindExternalFunction("SetPlayerStat",
                (string statType, int amount) => SetPlayerStat(statType, amount));
            story.BindExternalFunction("AddPlayerStat",
                (string statType, int amount) => AddPlayerStat(statType, amount));
            story.BindExternalFunction("AddPlayerActiveSkill", (string skillId) => AddPlayerActiveSkill(skillId));
            story.BindExternalFunction("AddPlayerPassiveSkill", (string skillId) => AddPlayerPassiveSkill(skillId));
            story.BindExternalFunction("AddPlayerItem", (string itemId, int amount) => AddPlayerItem(itemId, amount));
        }

        private void AddPlayerItem(string itemId, int amount)
        {
            ItemData itemData = ItemDataRegistry.GetItemDataByName(itemId);
            GameManager.Instance.PlayerManager.AddItemToInventory(itemData, amount);
        }

        private void AddPlayerPassiveSkill(string skillId)
        {
            PassiveSkill passiveSkill = SkillRegistry.CreatePassiveSkill(skillId);
            GameManager.Instance.PlayerManager.AddPassiveSkill(passiveSkill);
        }

        private void AddPlayerActiveSkill(string skillId)
        {
            ActiveSkill activeSkill = SkillRegistry.CreateActiveSkill(skillId);
            GameManager.Instance.PlayerManager.AddActiveSkill(activeSkill);
        }

        private void AddPlayerStat(string statType, int amount)
        {
            StatType statTypeEnum = (StatType)Enum.Parse(typeof(StatType), statType);
            GameManager.Instance.PlayerManager.SetPlayerStat(statTypeEnum, amount);
        }

        private void SetPlayerStat(string statType, int amount)
        {
            StatType statTypeEnum = (StatType)Enum.Parse(typeof(StatType), statType);
            GameManager.Instance.PlayerManager.SetPlayerStat(statTypeEnum, amount);
        }

        private void SetPlayerName(string playerName)
        {
            GameManager.Instance.PlayerManager.SetPlayerName(playerName);
        }


        private void UnbindPlayerFunctions(Story story)
        {
            story.UnbindExternalFunction("SetPlayerName");
            story.UnbindExternalFunction("SetPlayerStat");
            story.UnbindExternalFunction("AddPlayerStat");
            story.UnbindExternalFunction("AddPlayerActiveSkill");
            story.UnbindExternalFunction("AddPlayerPassiveSkill");
            story.UnbindExternalFunction("AddPlayerItem");
        }

        #endregion

        #region QUEST

        private void BindQuestFunctions(Story story)
        {
            story.BindExternalFunction("StartQuest", (string questId) => StartQuest(questId));
            story.BindExternalFunction("AdvanceQuest", (string questId) => AdvanceQuest(questId));
            story.BindExternalFunction("FinishQuest", (string questId) => FinishQuest(questId));
        }


        private static void UnbindQuestFunctions(Story story)
        {
            story.UnbindExternalFunction("StartQuest");
            story.UnbindExternalFunction("AdvanceQuest");
            story.UnbindExternalFunction("FinishQuest");
        }

        private void StartQuest(string questId)
        {
            // Start the quest with the given ID
            QuestEventSystem.InvokeQuestStarted(questId);
        }

        private void AdvanceQuest(string questId)
        {
            // Advance the quest with the given ID
            QuestEventSystem.InvokeQuestAdvanced(questId);
        }

        private void FinishQuest(string questId)
        {
            // Finish the quest with the given ID
            QuestEventSystem.InvokeQuestFinished(questId);
        }

        #endregion
    }
}