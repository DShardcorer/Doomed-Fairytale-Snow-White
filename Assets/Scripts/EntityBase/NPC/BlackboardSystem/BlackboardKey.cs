using System;

namespace EntityBase.NPC.BlackboardSystem
{
    public readonly struct BlackboardKey: IEquatable<BlackboardKey>
    {
        readonly string name;
        readonly int hashedKey;
        
        public BlackboardKey(string name)
        {
            this.name = name;
            hashedKey = name.ComputeFNV1aHash();
        }

        public bool Equals(BlackboardKey other)
        {
            return hashedKey == other.hashedKey;
        }

        public override bool Equals(object obj)
        {
            return obj is BlackboardKey other && Equals(other);
        }
        public override int GetHashCode()
        {
            return hashedKey;
        }
        public override string ToString()
        {
            return name;
        }
        public static bool operator ==(BlackboardKey left, BlackboardKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BlackboardKey left, BlackboardKey right)
        {
            return !(left == right);
        }
    }
}