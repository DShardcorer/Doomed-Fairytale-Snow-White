using System;

namespace EventBus.Narrative
{
    public class NarrativeEventSystem
    {
        public class NarrativeEventArgs : EventArgs
        {
            public string EventType;
            public string EventData;
        }
        
        public event Action<NarrativeEventArgs> OnNarrativeEventTriggered;

        public void InvokeNarrativeEvent(NarrativeEventArgs narrativeEventArgs)
        {
            OnNarrativeEventTriggered?.Invoke(narrativeEventArgs);
        }
    }
}