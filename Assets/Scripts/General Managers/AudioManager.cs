using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        AudioEventSystem.OnPlayAudioClip += OnPlayAudioClip;
    }

    private void OnPlayAudioClip(AudioEventSystem.PlayAudioClipEventArgs args)
    {
        if (args.AudioClip != null)
        {
            audioSource.PlayOneShot(args.AudioClip);

        }
    }
}
