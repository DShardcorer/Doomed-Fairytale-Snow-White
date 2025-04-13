using System.Collections.Generic;
using UnityEngine;

namespace NarrativeSystem
{
    [CreateAssetMenu(menuName = "Narrative/Narrative Node")]
    public class NarrativeNode : ScriptableObject
    {
        public string nodeID;
        [TextArea] public string narrativeText;
        public bool isTerminal;

        // public List<BranchChoice> manualChoices; // Optional for player dialogue
        public List<NarrativeTrigger> automaticTriggers; // Listens to world

        // Whether this node is actively evaluating its conditions
        [HideInInspector] public bool isActive;
    }
}