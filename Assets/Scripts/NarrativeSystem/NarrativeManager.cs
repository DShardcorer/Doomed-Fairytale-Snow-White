using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace NarrativeSystem
{

    public class NarrativeManager : MonoBehaviour
    {
        public WorldState worldState;
        private List<NarrativeNode> activeNodes = new List<NarrativeNode>();

        private void OnEnable()
        {
            worldState.OnFlagChanged += HandleFlagChanged;
        }

        private void OnDisable()
        {
            worldState.OnFlagChanged -= HandleFlagChanged;
        }

        public void ActivateNode(NarrativeNode node)
        {
            if (node == null)
            {
                Debug.LogError("Null node cannot be activated.");
                return;
            }

            if (!activeNodes.Contains(node))
            {
                node.isActive = true;
                activeNodes.Add(node);
            }

            Debug.Log($"Activated Node: {node.nodeID}");
            // Update UI with node narrative if desired.
        }

        private void HandleFlagChanged(string flagName, bool newValue)
        {
            foreach (var node in activeNodes.ToList())
            {
                foreach (var trigger in node.automaticTriggers)
                {
                    if (trigger.AreConditionsMet(worldState))
                    {
                        Debug.Log(
                            $"Transitioning from Node '{node.nodeID}' to Node '{trigger.nextNode.nodeID}' due to flag change '{flagName}'");
                        trigger.onTriggered?.Invoke();
                        TransitionToNode(node, trigger.nextNode);
                        break; // Exit after one trigger fires.
                    }
                }
            }
        }

        private void TransitionToNode(NarrativeNode fromNode, NarrativeNode toNode)
        {
            fromNode.isActive = false;
            activeNodes.Remove(fromNode);
            ActivateNode(toNode);
        }
    }
}