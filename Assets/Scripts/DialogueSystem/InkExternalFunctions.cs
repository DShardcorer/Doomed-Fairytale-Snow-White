using EventSystem.Quest;
using Ink.InkLibs.InkRuntime;

namespace DialogueSystem
{
    public class InkExternalFunctions
    {

        public void StartListening(Story story)
        {
            story.BindExternalFunction("StartQuest", (string questId) => StartQuest(questId));
            story.BindExternalFunction("AdvanceQuest", (string questId) => AdvanceQuest(questId));
            story.BindExternalFunction("FinishQuest", (string questId) => FinishQuest(questId));
        }

        public void StopListening(Story story)
        {
            story.UnbindExternalFunction("StartQuest");
            story.UnbindExternalFunction("AdvanceQuest");
            story.UnbindExternalFunction("FinishQuest");
        }
        private void StartQuest(string questId){
            // Start the quest with the given ID
            QuestEventSystem.InvokeQuestStarted(questId);
        }

        private void AdvanceQuest(string questId){
            // Advance the quest with the given ID
            QuestEventSystem.InvokeQuestAdvanced(questId);

        }
        private void FinishQuest(string questId){
            // Finish the quest with the given ID
            QuestEventSystem.InvokeQuestFinished(questId);
        }
    }
}
