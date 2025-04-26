using EntitySystems.Level;
using GeneralManagers;
using Item.Inventory;
using UnityEngine;

namespace QuestSystem
{
    public class Quest : ILifecycle<QuestManager>
    {
        private QuestManager _questManager;
        public QuestManager QuestManager => _questManager;

        public QuestInfoSO questInfo;

        public QuestState questState;

        private int currentQuestStepIndex;
        public int CurrentQuestStepIndex => currentQuestStepIndex;
        private QuestStepState[] questStepStates;
        public QuestStepState[] QuestStepStates => questStepStates;

        public Quest(QuestInfoSO questInfo)
        {
            this.questInfo = questInfo;
            this.currentQuestStepIndex = 0;
            this.questState = QuestState.REQUIREMENTS_NOT_MET;
            questStepStates = new QuestStepState[questInfo.questStepPrefabs.Length];
            for (int i = 0; i < questStepStates.Length; i++)
            {
                questStepStates[i] = new QuestStepState();
            }
        }

        public void Initialize(QuestManager parent)
        {
            _questManager = parent;
            questState = QuestState.REQUIREMENTS_NOT_MET;
            currentQuestStepIndex = 0;
        }

        public void Dispose()
        {
            _questManager = null;
            questInfo = null;
            currentQuestStepIndex = 0;
        }

        public void MoveToNextStep()
        {
            currentQuestStepIndex++;
        }
        public bool CurrentQuestStepExists()
        {
            return currentQuestStepIndex < questInfo.questStepPrefabs.Length;
        }
        public void InstantiateCurrentQuestStep(Transform parent)
        {
            GameObject questStepPrefab = GetCurrentQuestStepPrefab();
            if (questStepPrefab != null)
            {
                GameObject questStepInstance = GameObject.Instantiate(questStepPrefab, parent);
                Debug.Log($"Instantiated quest step: {questStepInstance.name} for quest: {questInfo.QuestName}");
                QuestStep questStep = questStepInstance.GetComponent<QuestStep>();
                if (questStep != null)
                {
                    questStep.Initialize(this);
                }
                else
                {
                    Debug.LogWarning($"Quest step prefab {questStepPrefab.name} does not have a QuestStep component.");
                }

            }
        }

        private GameObject GetCurrentQuestStepPrefab()
        {
            GameObject questStepPrefab = null;
            if (CurrentQuestStepExists())
            {
                questStepPrefab = questInfo.questStepPrefabs[currentQuestStepIndex];
            }
            else
            {
                Debug.LogWarning("No more quest steps available");
            }
            return questStepPrefab;
        }

        public void GiveRewards()
        {
            //Put reward into inventory
            Debug.Log($"Quest {questInfo.QuestName} completed! Rewards given.");
            InventorySystem playerInventorySystem = GameManager.Instance.PlayerManager.Player.InventorySystem;
            foreach (InventoryItem inventoryItem in questInfo.itemRewards)
            {
                playerInventorySystem.AddItem(inventoryItem.ItemData, inventoryItem.stackSize);
            }

            LevelSystem playerLevelSystem = GameManager.Instance.PlayerManager.Player.LevelSystem;
            playerLevelSystem.AddExperience(questInfo.experienceReward);
            Debug.Log($"Quest {questInfo.QuestName} completed! Experience given: {questInfo.experienceReward}");
        }

        public void StoreQuestStepState(QuestStepState questStepState, int index)
        {
            if (index < questStepStates.Length)
            {
                questStepStates[index] = questStepState;
            }
            else
            {
                Debug.LogWarning($"Index {index} is out of bounds for quest step states of quest {questInfo.QuestName}.");
            }
        }

        public QuestData GetQuestData()
        {
            return new QuestData(questState, currentQuestStepIndex, questStepStates);
        }
        public void SetQuestData(QuestData questData)
        {
            questState = questData.questState;
            currentQuestStepIndex = questData.currentQuestStepIndex;
            questStepStates = questData.questStepStates;
        }
    }
}
