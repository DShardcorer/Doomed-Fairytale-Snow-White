using System.Collections.Generic;

namespace Entity.NPC.AStar
{
    public class Node
    {
        public Node CameFrom;
        public List<Node> Neighbors;
        
        public float GScore;
        public float HScore;
        
        public float FScore => GScore + HScore;
    }
}