using System;
using System.Collections;
using System.Collections.Generic;
using GeneralManagers;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioSystem
{
    public class AudioManager : MonoBehaviour, ILifecycle<GameManager>
    {
        private GameManager _parent;


        [Header("Audio Mixer")] [SerializeField]
        private AudioMixer audioMixer;

        [Header("Audio Sources")] [SerializeField]
        private AudioSource musicSource;

        [SerializeField] private AudioSource ambienceSource;

        [Header("Audio Settings")]
        private float defaultFadeDuration = 3f;

        [SerializeField] private int sfxPoolSize = 10;
        [SerializeField] private int voicePoolSize = 5;
        [SerializeField] private int uiPoolSize = 3;

        [Header("Volume Settings")] [Range(0, 1)] [SerializeField]
        private float masterVolume = 1.0f;

        [Range(0, 1)] [SerializeField] private float musicVolume = 1.0f;
        [Range(0, 1)] [SerializeField] private float ambienceVolume = 1.0f;
        [Range(0, 1)] [SerializeField] private float sfxVolume = 1.0f;
        [Range(0, 1)] [SerializeField] private float uiVolume = 1.0f;
        [Range(0, 1)] [SerializeField] private float voiceVolume = 1.0f;

        // Add this method to automatically update volumes when changed in inspector
        private void OnValidate()
        {
            // Only update if the audioMixer is assigned
            if (audioMixer != null)
            {
                UpdateMixerVolumes();
            }
        }

        // Audio Source Pools
        private List<PooledAudioSource> sfxPool;
        private List<PooledAudioSource> voicePool;
        private List<PooledAudioSource> uiPool;

        // Currently playing sounds
        private Dictionary<string, PooledAudioSource> activeAudioSources = new Dictionary<string, PooledAudioSource>();

        // Audio clip cache
        private Dictionary<string, AudioClip> audioClipCache = new Dictionary<string, AudioClip>();

        // Coroutine references for fading
        private Coroutine musicFadeCoroutine;
        private Coroutine ambienceFadeCoroutine;

        // Audio mixer parameter names
        private const string MasterVolumeParam = "MasterVolume";
        private const string MusicVolumeParam = "MusicVolume";
        private const string AmbienceVolumeParam = "AmbienceVolume";
        private const string SFXVolumeParam = "SFXVolume";
        private const string UIVolumeParam = "UIVolume";
        private const string VoiceVolumeParam = "VoiceVolume";


        public void Dispose()
        {
            _parent = null;
        }

        public void Initialize(GameManager parent)
        {
            _parent = parent;
            // Create parent objects for organization
            Transform sfxParent = CreateChildTransform("SFX_Pool");
            Transform voiceParent = CreateChildTransform("Voice_Pool");
            Transform uiParent = CreateChildTransform("UI_Pool");

            // Create the music source if it doesn't exist
            if (musicSource == null)
            {
                var musicObj = new GameObject("MusicSource");
                musicObj.transform.SetParent(transform);
                musicSource = musicObj.AddComponent<AudioSource>();
                musicSource.loop = true;
                musicSource.playOnAwake = false;
                musicSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Music")[0];
            }

            // Create the ambience source if it doesn't exist
            if (ambienceSource == null)
            {
                var ambienceObj = new GameObject("AmbienceSource");
                ambienceObj.transform.SetParent(transform);
                ambienceSource = ambienceObj.AddComponent<AudioSource>();
                ambienceSource.loop = true;
                ambienceSource.playOnAwake = false;
                ambienceSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Ambience")[0];
            }

            // Initialize audio source pools
            sfxPool = CreateAudioPool(sfxPoolSize, sfxParent, "SFX_", audioMixer.FindMatchingGroups("SFX")[0]);
            voicePool = CreateAudioPool(voicePoolSize, voiceParent, "Voice_",
                audioMixer.FindMatchingGroups("Voice")[0]);
            uiPool = CreateAudioPool(uiPoolSize, uiParent, "UI_", audioMixer.FindMatchingGroups("UI")[0]);

            // Apply initial volume settings
            UpdateMixerVolumes();
        }

        private Transform CreateChildTransform(string name)
        {
            var go = new GameObject(name);
            go.transform.SetParent(transform);
            return go.transform;
        }

        private List<PooledAudioSource> CreateAudioPool(int size, Transform parent, string prefix,
            AudioMixerGroup mixerGroup)
        {
            List<PooledAudioSource> pool = new List<PooledAudioSource>(size);

            for (int i = 0; i < size; i++)
            {
                GameObject sourceObj = new GameObject($"{prefix}{i}");
                sourceObj.transform.SetParent(parent);

                AudioSource audioSource = sourceObj.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
                audioSource.outputAudioMixerGroup = mixerGroup;

                pool.Add(new PooledAudioSource
                {
                    Source = audioSource,
                    IsPlaying = false,
                    Id = string.Empty
                });
            }

            return pool;
        }

        private void UpdateMixerVolumes()
        {
            // Convert linear volume to logarithmic (for mixer)
            float ConvertToDecibel(float linearVolume)
            {
                return linearVolume > 0.001f ? 20f * Mathf.Log10(linearVolume) : -80f;
            }

            audioMixer.SetFloat(MasterVolumeParam, ConvertToDecibel(masterVolume));
            audioMixer.SetFloat(MusicVolumeParam, ConvertToDecibel(musicVolume));
            audioMixer.SetFloat(AmbienceVolumeParam, ConvertToDecibel(ambienceVolume));
            audioMixer.SetFloat(SFXVolumeParam, ConvertToDecibel(sfxVolume));
            audioMixer.SetFloat(UIVolumeParam, ConvertToDecibel(uiVolume));
            audioMixer.SetFloat(VoiceVolumeParam, ConvertToDecibel(voiceVolume));
        }

        #region Public API

        /// <summary>
        /// Play background music with optional crossfade
        /// </summary>
        public void PlayMusic(AudioClip musicClip, bool fade = true, float fadeDuration = -1)
        {
            if (musicClip == null) return;

            if (musicFadeCoroutine != null)
            {
                StopCoroutine(musicFadeCoroutine);
            }

            float actualFadeDuration = fadeDuration > 0 ? fadeDuration : defaultFadeDuration;

            if (fade && musicSource.isPlaying)
            {
                // Crossfade to new track
                musicFadeCoroutine = StartCoroutine(CrossfadeMusic(musicClip, actualFadeDuration));
            }
            else
            {
                // Play immediately
                musicSource.clip = musicClip;
                musicSource.Play();

                if (fade)
                {
                    // Fade in from silence
                    musicSource.volume = 0;
                    musicFadeCoroutine = StartCoroutine(FadeMusicVolume(0, 1, actualFadeDuration));
                }
                else
                {
                    musicSource.volume = 1;
                }
            }
        }

        /// <summary>
        /// Stop playing background music
        /// </summary>
        public void StopMusic(bool fade = true, float fadeDuration = -1)
        {
            if (!musicSource.isPlaying) return;

            if (musicFadeCoroutine != null)
            {
                StopCoroutine(musicFadeCoroutine);
            }

            if (fade)
            {
                float actualFadeDuration = fadeDuration > 0 ? fadeDuration : defaultFadeDuration;
                musicFadeCoroutine =
                    StartCoroutine(FadeMusicVolume(musicSource.volume, 0, actualFadeDuration, stop: true));
            }
            else
            {
                musicSource.Stop();
            }
        }

        /// <summary>
        /// Play ambient sound
        /// </summary>
        public void PlayAmbience(AudioClip ambienceClip, bool fade = true, float fadeDuration = -1)
        {
            if (ambienceClip == null) return;

            if (ambienceFadeCoroutine != null)
            {
                StopCoroutine(ambienceFadeCoroutine);
            }

            float actualFadeDuration = fadeDuration > 0 ? fadeDuration : defaultFadeDuration;

            if (fade && ambienceSource.isPlaying)
            {
                // Crossfade to new ambience
                ambienceFadeCoroutine = StartCoroutine(CrossfadeAmbience(ambienceClip, actualFadeDuration));
            }
            else
            {
                // Play immediately
                ambienceSource.clip = ambienceClip;
                ambienceSource.Play();

                if (fade)
                {
                    // Fade in from silence
                    ambienceSource.volume = 0;
                    ambienceFadeCoroutine = StartCoroutine(FadeAmbienceVolume(0, 1, actualFadeDuration));
                }
                else
                {
                    ambienceSource.volume = 1;
                }
            }
        }

        /// <summary>
        /// Stop ambient sound
        /// </summary>
        public void StopAmbience(bool fade = true, float fadeDuration = -1)
        {
            if (!ambienceSource.isPlaying) return;

            if (ambienceFadeCoroutine != null)
            {
                StopCoroutine(ambienceFadeCoroutine);
            }

            if (fade)
            {
                float actualFadeDuration = fadeDuration > 0 ? fadeDuration : defaultFadeDuration;
                ambienceFadeCoroutine =
                    StartCoroutine(FadeAmbienceVolume(ambienceSource.volume, 0, actualFadeDuration, stop: true));
            }
            else
            {
                ambienceSource.Stop();
            }
        }

        /// <summary>
        /// Play a sound effect
        /// </summary>
        /// <returns>A unique ID for the sound that can be used to stop it</returns>
        public string PlaySFX(AudioClip clip, float volume = 1f, float pitch = 1f, bool loop = false,
            Vector3? position = null)
        {
            if (clip == null) return string.Empty;

            PooledAudioSource pooledSource = GetAvailableSource(sfxPool);
            if (pooledSource == null) return string.Empty;

            // Generate a unique ID for this sound
            string soundId = $"SFX_{clip.name}_{DateTime.Now.Ticks}";

            ConfigureAudioSource(pooledSource.Source, clip, volume, pitch, loop, position);
            pooledSource.Source.Play();

            pooledSource.IsPlaying = true;
            pooledSource.Id = soundId;

            // Track the playing sound
            activeAudioSources[soundId] = pooledSource;

            // If not looping, auto-return to pool
            if (!loop)
            {
                StartCoroutine(ReturnToPoolWhenFinished(pooledSource, clip.length / pitch));
            }

            return soundId;
        }

        /// <summary>
        /// Play a UI sound
        /// </summary>
        public void PlayUISound(AudioClip clip, float volume = 1f, float pitch = 1f)
        {
            if (clip == null) return;

            PooledAudioSource pooledSource = GetAvailableSource(uiPool);
            if (pooledSource == null) return;

            // UI sounds are never looped, spatialize, or positioned
            string soundId = $"UI_{clip.name}_{DateTime.Now.Ticks}";

            pooledSource.Source.clip = clip;
            pooledSource.Source.volume = volume;
            pooledSource.Source.pitch = pitch;
            pooledSource.Source.loop = false;
            pooledSource.Source.spatialBlend = 0f; // 2D sound
            pooledSource.Source.Play();

            pooledSource.IsPlaying = true;
            pooledSource.Id = soundId;

            // Track the playing sound
            activeAudioSources[soundId] = pooledSource;

            // Auto-return to pool
            StartCoroutine(ReturnToPoolWhenFinished(pooledSource, clip.length / pitch));
        }

        /// <summary>
        /// Play voice audio (dialogue, etc.)
        /// </summary>
        /// <returns>A unique ID for the voice line that can be used to stop it</returns>
        public string PlayVoice(AudioClip clip, float volume = 1f, float pitch = 1f, Vector3? position = null)
        {
            if (clip == null) return string.Empty;

            PooledAudioSource pooledSource = GetAvailableSource(voicePool);
            if (pooledSource == null) return string.Empty;

            string soundId = $"Voice_{clip.name}_{DateTime.Now.Ticks}";

            ConfigureAudioSource(pooledSource.Source, clip, volume, pitch, false, position);
            pooledSource.Source.Play();

            pooledSource.IsPlaying = true;
            pooledSource.Id = soundId;

            // Track the playing sound
            activeAudioSources[soundId] = pooledSource;

            // Voice lines never loop
            StartCoroutine(ReturnToPoolWhenFinished(pooledSource, clip.length / pitch));

            return soundId;
        }

        /// <summary>
        /// Stop a sound by its ID
        /// </summary>
        public bool StopSound(string soundId)
        {
            if (string.IsNullOrEmpty(soundId) ||
                !activeAudioSources.TryGetValue(soundId, out PooledAudioSource pooledSource))
                return false;

            pooledSource.Source.Stop();
            pooledSource.IsPlaying = false;
            pooledSource.Id = string.Empty;

            activeAudioSources.Remove(soundId);

            return true;
        }

        /// <summary>
        /// Play a random sound from a collection
        /// </summary>
        public string PlayRandomSFX(AudioClip[] clips, float volume = 1f, float pitchMin = 0.9f, float pitchMax = 1.1f,
            bool loop = false, Vector3? position = null)
        {
            if (clips == null || clips.Length == 0)
                return string.Empty;

            AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
            float randomPitch = UnityEngine.Random.Range(pitchMin, pitchMax);

            return PlaySFX(randomClip, volume, randomPitch, loop, position);
        }

        /// <summary>
        /// Preload an audio clip into memory
        /// </summary>
        public void PreloadAudioClip(string resourcePath)
        {
            if (string.IsNullOrEmpty(resourcePath) || audioClipCache.ContainsKey(resourcePath))
                return;

            AudioClip clip = UnityEngine.Resources.Load<AudioClip>(resourcePath);
            if (clip != null)
            {
                audioClipCache[resourcePath] = clip;
            }
        }

        /// <summary>
        /// Play audio clip from resource path (with optional preloading)
        /// </summary>
        public string PlaySFXFromResources(string resourcePath, float volume = 1f, float pitch = 1f, bool loop = false,
            Vector3? position = null)
        {
            AudioClip clip;

            if (audioClipCache.TryGetValue(resourcePath, out clip))
            {
                // Use cached clip
                return PlaySFX(clip, volume, pitch, loop, position);
            }
            else
            {
                // Load and cache
                clip = UnityEngine.Resources.Load<AudioClip>(resourcePath);
                if (clip != null)
                {
                    audioClipCache[resourcePath] = clip;
                    return PlaySFX(clip, volume, pitch, loop, position);
                }
            }

            return string.Empty;
        }

        public string PlayVoiceFromResources(string resourcePath, float volume = 1f, float pitch = 1f,
            Vector3? position = null)
        {
            AudioClip clip;
            if (audioClipCache.TryGetValue(resourcePath, out clip))
            {
                // Use cached clip
                return PlayVoice(clip, volume, pitch, position);
            }
            else
            {
                // Load and cache
                clip = UnityEngine.Resources.Load<AudioClip>(resourcePath);
                if (clip != null)
                {
                    audioClipCache[resourcePath] = clip;
                    return PlayVoice(clip, volume, pitch, position);
                }
            }
            return string.Empty;
        }
        
        public void PlayMusicFromResources(string resourcePath, bool fade = true, float fadeDuration = -1f)
        {
            AudioClip clip;
            if (audioClipCache.TryGetValue(resourcePath, out clip))
            {
                // Use cached clip
                PlayMusic(clip, fade, fadeDuration);
            }
            else
            {
                // Load and cache
                clip = UnityEngine.Resources.Load<AudioClip>(resourcePath);
                if (clip != null)
                {
                    audioClipCache[resourcePath] = clip;
                    PlayMusic(clip, fade, fadeDuration);
                }
            }
        }

        /// <summary>
        /// Set master volume level
        /// </summary>
        public void SetMasterVolume(float volume)
        {
            masterVolume = Mathf.Clamp01(volume);
            UpdateMixerVolumes();
        }

        /// <summary>
        /// Set music volume level
        /// </summary>
        public void SetMusicVolume(float volume)
        {
            musicVolume = Mathf.Clamp01(volume);
            UpdateMixerVolumes();
        }

        /// <summary>
        /// Set SFX volume level
        /// </summary>
        public void SetSFXVolume(float volume)
        {
            sfxVolume = Mathf.Clamp01(volume);
            UpdateMixerVolumes();
        }

        /// <summary>
        /// Set ambience volume level
        /// </summary>
        public void SetAmbienceVolume(float volume)
        {
            ambienceVolume = Mathf.Clamp01(volume);
            UpdateMixerVolumes();
        }

        /// <summary>
        /// Set UI sounds volume level
        /// </summary>
        public void SetUIVolume(float volume)
        {
            uiVolume = Mathf.Clamp01(volume);
            UpdateMixerVolumes();
        }

        /// <summary>
        /// Set voice volume level
        /// </summary>
        public void SetVoiceVolume(float volume)
        {
            voiceVolume = Mathf.Clamp01(volume);
            UpdateMixerVolumes();
        }

        /// <summary>
        /// Mute/unmute all audio
        /// </summary>
        public void SetMute(bool isMuted)
        {
            audioMixer.SetFloat(MasterVolumeParam, isMuted ? -80f : ConvertToDecibel(masterVolume));
        }

        /// <summary>
        /// Pause all sounds
        /// </summary>
        public void PauseAll()
        {
            if (musicSource.isPlaying) musicSource.Pause();
            if (ambienceSource.isPlaying) ambienceSource.Pause();

            foreach (var source in activeAudioSources.Values)
            {
                if (source.Source.isPlaying)
                {
                    source.Source.Pause();
                    source.WasPaused = true;
                }
            }
        }

        /// <summary>
        /// Resume all paused sounds
        /// </summary>
        public void ResumeAll()
        {
            if (!musicSource.isPlaying && musicSource.time > 0) musicSource.UnPause();
            if (!ambienceSource.isPlaying && ambienceSource.time > 0) ambienceSource.UnPause();

            foreach (var source in activeAudioSources.Values)
            {
                if (source.WasPaused)
                {
                    source.Source.UnPause();
                    source.WasPaused = false;
                }
            }
        }

        /// <summary>
        /// Stop all sounds
        /// </summary>
        public void StopAll(bool includingMusic = true, bool includingAmbience = true)
        {
            if (includingMusic) StopMusic(false);
            if (includingAmbience) StopAmbience(false);

            List<string> soundsToStop = new List<string>(activeAudioSources.Keys);
            foreach (var soundId in soundsToStop)
            {
                StopSound(soundId);
            }
        }

        #endregion

        #region Helper Methods

        private void ConfigureAudioSource(AudioSource source, AudioClip clip, float volume, float pitch, bool loop,
            Vector3? position)
        {
            source.clip = clip;
            source.volume = volume;
            source.pitch = pitch;
            source.loop = loop;

            // Configure spatial settings
            if (position.HasValue)
            {
                source.transform.position = position.Value;
                source.spatialBlend = 1f; // 3D sound
            }
            else
            {
                source.spatialBlend = 0f; // 2D sound
            }
        }

        private PooledAudioSource GetAvailableSource(List<PooledAudioSource> pool)
        {
            // First try to find a non-playing source
            foreach (var source in pool)
            {
                if (!source.IsPlaying)
                {
                    return source;
                }
            }

            // If all are playing, find the oldest/least important one to reuse
            PooledAudioSource oldestSource = null;
            float oldestStartTime = float.MaxValue;

            foreach (var source in pool)
            {
                if (source.StartTime < oldestStartTime)
                {
                    oldestStartTime = source.StartTime;
                    oldestSource = source;
                }
            }

            // If we're about to reuse a playing source, clean up its tracking
            if (oldestSource != null && oldestSource.IsPlaying)
            {
                oldestSource.Source.Stop();
                if (!string.IsNullOrEmpty(oldestSource.Id))
                {
                    activeAudioSources.Remove(oldestSource.Id);
                }
            }

            return oldestSource;
        }

        private IEnumerator ReturnToPoolWhenFinished(PooledAudioSource pooledSource, float delay)
        {
            pooledSource.StartTime = Time.time;

            yield return new WaitForSeconds(delay);

            if (pooledSource.IsPlaying && !pooledSource.Source.isPlaying)
            {
                pooledSource.IsPlaying = false;

                if (!string.IsNullOrEmpty(pooledSource.Id))
                {
                    activeAudioSources.Remove(pooledSource.Id);
                    pooledSource.Id = string.Empty;
                }
            }
        }

        private IEnumerator FadeMusicVolume(float startVolume, float targetVolume, float duration, bool stop = false)
        {
            float elapsed = 0;

            while (elapsed < duration)
            {
                musicSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            musicSource.volume = targetVolume;

            if (stop && Mathf.Approximately(targetVolume, 0f))
            {
                musicSource.Stop();
            }

            musicFadeCoroutine = null;
        }

        private IEnumerator FadeAmbienceVolume(float startVolume, float targetVolume, float duration, bool stop = false)
        {
            float elapsed = 0;

            while (elapsed < duration)
            {
                ambienceSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            ambienceSource.volume = targetVolume;

            if (stop && Mathf.Approximately(targetVolume, 0f))
            {
                ambienceSource.Stop();
            }

            ambienceFadeCoroutine = null;
        }

        private IEnumerator CrossfadeMusic(AudioClip newClip, float duration)
        {
            // Create a temporary source for crossfading
            GameObject tempGO = new GameObject("TempMusicSource");
            tempGO.transform.SetParent(transform);
            AudioSource tempSource = tempGO.AddComponent<AudioSource>();

            // Configure temp source to match music source
            tempSource.outputAudioMixerGroup = musicSource.outputAudioMixerGroup;
            tempSource.clip = newClip;
            tempSource.loop = true;
            tempSource.volume = 0;
            tempSource.Play();

            float elapsed = 0;

            // Fade out original while fading in new
            while (elapsed < duration)
            {
                float t = elapsed / duration;
                musicSource.volume = Mathf.Lerp(1, 0, t);
                tempSource.volume = Mathf.Lerp(0, 1, t);
                elapsed += Time.deltaTime;
                yield return null;
            }

            // Ensure volumes are set correctly
            musicSource.volume = 0;
            tempSource.volume = 1;

            // Stop the original source
            musicSource.Stop();

            // Transfer clip to main music source
            musicSource.clip = newClip;
            musicSource.volume = 1;
            musicSource.Play();

            // Clean up temporary source
            Destroy(tempGO);

            musicFadeCoroutine = null;
        }

        private IEnumerator CrossfadeAmbience(AudioClip newClip, float duration)
        {
            // Create a temporary source for crossfading
            GameObject tempGO = new GameObject("TempAmbienceSource");
            tempGO.transform.SetParent(transform);
            AudioSource tempSource = tempGO.AddComponent<AudioSource>();

            // Configure temp source to match ambience source
            tempSource.outputAudioMixerGroup = ambienceSource.outputAudioMixerGroup;
            tempSource.clip = newClip;
            tempSource.loop = true;
            tempSource.volume = 0;
            tempSource.Play();

            float elapsed = 0;

            // Fade out original while fading in new
            while (elapsed < duration)
            {
                float t = elapsed / duration;
                ambienceSource.volume = Mathf.Lerp(1, 0, t);
                tempSource.volume = Mathf.Lerp(0, 1, t);
                elapsed += Time.deltaTime;
                yield return null;
            }

            // Ensure volumes are set correctly
            ambienceSource.volume = 0;
            tempSource.volume = 1;

            // Stop the original source
            ambienceSource.Stop();

            // Transfer clip to main ambience source
            ambienceSource.clip = newClip;
            ambienceSource.volume = 1;
            ambienceSource.Play();

            // Clean up temporary source
            Destroy(tempGO);

            ambienceFadeCoroutine = null;
        }

        // Helper to convert linear volume to decibels
        private float ConvertToDecibel(float linearVolume)
        {
            return linearVolume > 0.001f ? 20f * Mathf.Log10(linearVolume) : -80f;
        }

        #endregion
    }

    /// <summary>
    /// Represents a pooled audio source with metadata
    /// </summary>
    public class PooledAudioSource
    {
        public AudioSource Source;
        public bool IsPlaying;
        public bool WasPaused;
        public string Id;
        public float StartTime;
    }
}