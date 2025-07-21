namespace FlagSystem
{
    namespace FlagSystem
    {
        public enum FlagType { Boolean, Integer, Float, String }

        [System.Serializable]
        public abstract class GameFlag
        {
            public string id;
            public abstract FlagType GetFlagType();
            public abstract object GetValue();
        }

        [System.Serializable]
        public class BoolGameFlag : GameFlag
        {
            public bool value;
            public override FlagType GetFlagType() => FlagType.Boolean;
            public override object GetValue() => value;
        }

        [System.Serializable]
        public class IntGameFlag : GameFlag
        {
            public int value;
            public override FlagType GetFlagType() => FlagType.Integer;
            public override object GetValue() => value;
        }

        [System.Serializable]
        public class FloatGameFlag : GameFlag
        {
            public float value;
            public override FlagType GetFlagType() => FlagType.Float;
            public override object GetValue() => value;
        }

        [System.Serializable]
        public class StringGameFlag : GameFlag
        {
            public string value;
            public override FlagType GetFlagType() => FlagType.String;
            public override object GetValue() => value;
        }
    }
}