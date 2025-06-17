using System;
using DefaultNamespace.EventSystem.Barter;
using DefaultNamespace.EventSystem.Input;
using Entity.NPC;
using EntitySystems.Skill;
using EntitySystems.Skill.SkillRegistry;
using EntitySystems.Stats;
using EventSystem.Quest;
using EventSystem.UI;
using GeneralManagers;
using Helpers;
using Ink.InkLibs.InkRuntime;
using Item;
using Item.Inventory;
using SceneSwitch;
using UnityEngine;

namespace DialogueSystem
{
    public class InkExternalFunctions
    {
        public void StartListening(Story story)
        {
            BindSceneSwitchingFunctions(story);
            BindBarterFunctions(story);
            BindQuestFunctions(story);
            BindPlayerFunctions(story);
            BindAudioFunctions(story);
            BindTextInputFunctions(story);
            BindMiscFunctions(story);
        }


        public void StopListening(Story story)
        {
            UnbindSceneSwitchingFunctions(story);
            UnbindBarterFunctions(story);
            UnbindQuestFunctions(story);
            UnbindPlayerFunctions(story);
            UnbindAudioFunctions(story);
            UnbindTextInputFunctions(story);
            UnbindMiscFunctions(story);
        }

        private NPC GetNPCCurrentlyInteractingWith()
        {
            return GameManager.Instance.PlayerManager.Player.PlayerProperties.NPCInteractingWith;
        }

        #region BARTER

        private InventorySystem GetPlayerInventorySystem()
        {
            return GameManager.Instance.PlayerManager.Player.InventorySystem;
        }

        private void BindBarterFunctions(Story story)
        {
            story.BindExternalFunction("StartBarter", () => BarterEventSystem.InvokeBarterStart(
                new BarterEventSystem.BarterStartEventArgs(
                    GetPlayerInventorySystem(),
                    GetNPCCurrentlyInteractingWith().InventorySystem
                )
            ));
        }

        private void UnbindBarterFunctions(Story story)
        {
            story.UnbindExternalFunction("StartBarter");
        }

        #endregion

        #region SCENESWITCHING

        private void BindSceneSwitchingFunctions(Story story)
        {
            story.BindExternalFunction("SwitchScene", (string sceneToLoad, string portalToSpawnAt) =>
                SwitchScene(sceneToLoad, portalToSpawnAt)
            );
        }
        private void UnbindSceneSwitchingFunctions(Story story)
        {
            story.UnbindExternalFunction("SwitchScene");
        }

        private void SwitchScene(string sceneToLoad, string portalToSpawnAt = "")
        {
            Enum.TryParse<SceneSwitchPortal.PortalToSpawnAt>(portalToSpawnAt, out var portalToSpawnAtEnum);
            SceneSwitchManager.Instance.SwitchSceneToPortal(sceneToLoad, portalToSpawnAtEnum);
        }

        #endregion

        #region TEXTINPUT

        private void BindTextInputFunctions(Story story)
        {
            story.BindExternalFunction("OpenTextInputter",
                (string placeholderText, string inputPurpose) => OpenTextInputter(placeholderText, inputPurpose));
        }

        private void UnbindTextInputFunctions(Story story)
        {
            story.UnbindExternalFunction("OpenTextInputter");
        }

        private void OpenTextInputter(string placeholderText, string inputPurpose)
        {
            TextInputEventSystem.InvokeOpenTextInputter(placeholderText, inputPurpose);
        }

        #endregion

        #region AUDIO

        private void BindAudioFunctions(Story story)
        {
            story.BindExternalFunction("PlaySFX", (string sfxName) => PlaySFX(sfxName));
        }

        private void UnbindAudioFunctions(Story story)
        {
            story.UnbindExternalFunction("PlaySFX");
        }

        private void PlaySFX(string sfxName)
        {
            GameManager.Instance.AudioManager.PlaySFXFromResources(HelperResourcePath.SFXPath + sfxName);
        }

        #endregion

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
            ItemDataSO itemDataSo = ItemRegistry.GetItemDataByName(itemId);
            GameManager.Instance.PlayerManager.AddItemToInventory(itemDataSo, amount);
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
            GameManager.Instance.PlayerManager.AddPlayerStat(statTypeEnum, amount);
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

        #region MISC

        private void BindMiscFunctions(Story story)
        {
            story.BindExternalFunction("Fade", (float fadeInDuration, float fadeOutDuration) =>
                CGFadeEventSystem.InvokeFade(fadeInDuration, fadeOutDuration));
        }

        private void UnbindMiscFunctions(Story story)
        {
            story.UnbindExternalFunction("Fade");
        }

        #endregion
    }
}