using System;
using UnityEngine;

namespace EventBus.Audio
{
    public static class AudioEventSystem
    {
        public class PlayAudioClipEventArgs :EventArgs
        {
            public AudioClip AudioClip;
            public PlayAudioClipEventArgs(AudioClip audioClip)
            {
                AudioClip = audioClip;
            }
        }

        public static Action<PlayAudioClipEventArgs> OnPlayAudioClip;

        public static void InvokePlayAudioClip(PlayAudioClipEventArgs args)
        {
            OnPlayAudioClip?.Invoke(args);
        }
    }
}
