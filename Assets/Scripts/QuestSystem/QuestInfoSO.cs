using Item.Inventory;
using UnityEngine;

namespace QuestSystem
{
    [CreateAssetMenu(fileName = "QuestInfoSO", menuName = "Quest System/QuestInfoSO")]
    public class QuestInfoSO : ScriptableObject
    {
        [SerializeField]
        private string questName;
        public string QuestName => questName;

        [Header("General")]
        public string displayName;
        [Header("Requirements")]
        public int levelRequirement;
        public QuestInfoSO[] questPrerequisites;

        [Header("Quest Steps")]
        public GameObject[] questStepPrefabs;

        [Header("Rewards")]
        public InventoryItem[] itemRewards;
        public int experienceReward;



        private void OnValidate()
        {
#if UNITY_EDITOR
            questName = name;
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

    }
}
