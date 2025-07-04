using System.Collections.Generic;

namespace EntityBase.NPC.BehaviourTree
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
        public readonly List<Node> children = new();
        protected int currentChildIndex = 0;

        public Node(string name)
        {
            this.name = name;
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