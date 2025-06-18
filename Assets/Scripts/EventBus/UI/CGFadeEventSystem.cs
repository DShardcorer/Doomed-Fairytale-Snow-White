using System;
using System.Collections.Generic;

namespace EventBus.UI
{
    public static class CGFadeEventSystem
    {
        public class FadeEventArgs : EventArgs
        {
            public float FadeOutDuration;
            public float FadeInDuration;
            public Action OnComplete;
            public Action OnTextStartDissapearing;
            public List<string> StringsToWrite;
            public List<float> TextSpeed;
            public bool SkipFadeOut;

            public FadeEventArgs(float fadeOutDuration, float fadeInDuration, Action onComplete = null,
                Action onTextStartDissapearing = null,
                List<string> stringsToWrite = null, List<float> textSpeed = null, bool skipFadeOut = false)
            {
                FadeOutDuration = fadeOutDuration;
                FadeInDuration = fadeInDuration;
                OnComplete = onComplete;
                OnTextStartDissapearing = onTextStartDissapearing;
                StringsToWrite = stringsToWrite;
                TextSpeed = textSpeed;
                SkipFadeOut = skipFadeOut;
            }
        }

        public static Action<FadeEventArgs> OnFade;

        public static void InvokeFade(float fadeOutDuration, float fadeInDuration, Action onComplete = null,
            Action onTextStartDissapearing = null,
            List<string> stringsToWrite = null, List<float> textSpeed = null, bool skipFadeOut = false)
        {
            FadeEventArgs args = new FadeEventArgs(fadeOutDuration, fadeInDuration, onComplete, onTextStartDissapearing,
                stringsToWrite,
                textSpeed, skipFadeOut);
            OnFade?.Invoke(args);
        }
    }
}