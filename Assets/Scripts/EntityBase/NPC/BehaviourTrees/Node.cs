using System.Collections.Generic;

namespace EntityBase.NPC.BehaviourTrees
{
    public class Node
    {
        public enum Status
        {
            Success,
            Failure,
            Running
        }

        public readonly string name;
        public readonly int priority;
        public readonly List<Node> children = new();
        protected int currentChildIndex = 0;

        public Node(string name = "Node", int priority = 0)
        {
            this.name = name;
            this.priority = priority;
        }

        public void AddChild(Node child)
        {
            children.Add(child);
        }

        public virtual Status Process() => children[currentChildIndex].Process();

        public virtual void Reset()
        {
            currentChildIndex = 0;
            foreach (var child in children)
            {
                child.Reset();
            }
        }
    }
}