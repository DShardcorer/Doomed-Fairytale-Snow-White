using UnityEngine;
using UnityEngine.EventSystems;
using GeneralManagers;
using AudioSystem;

namespace UI.Components
{
    /// <summary>
    /// Attach this component to any UI element to play a sound when it's selected.
    /// This works with Unity's EventSystem and requires an AudioManager.
    /// </summary>
    public class UISelectSound : MonoBehaviour, ISelectHandler
    {
        [Header("Sound Settings")]
        [Tooltip("Sound effect to play when UI element is selected")]
        [SerializeField] private AudioClip selectSound;
        
        [Tooltip("Volume of the select sound (0-1)")]
        [Range(0f, 1f)]
        [SerializeField] private float volume = 1f;
        
        [Tooltip("Pitch of the select sound")]
        [Range(0.5f, 1.5f)]
        [SerializeField] private float pitch = 1f;
        
        [Header("Alternative Resource Path")]
        [Tooltip("Path to sound in Resources folder (leave empty if using AudioClip above)")]
        [SerializeField] private string resourcePath = "";
        
        /// <summary>
        /// Called when the UI element is selected. Plays the configured sound.
        /// </summary>
        public void OnSelect(BaseEventData eventData)
        {
            PlaySelectSound();
        }
        
        /// <summary>
        /// Plays the selection sound through the AudioManager
        /// </summary>
        public void PlaySelectSound()
        {
            // Get reference to the AudioManager
            AudioManager audioManager = GameManager.Instance.AudioManager;
            
            if (audioManager == null)
            {
                Debug.LogWarning("AudioManager not found! Cannot play UI select sound.");
                return;
            }
            
            // If we have a direct reference to the audio clip, use it
            if (selectSound != null)
            {
                audioManager.PlayUISound(selectSound, volume, pitch);
            }
            // Otherwise try to load from resources if path is provided
            else if (!string.IsNullOrEmpty(resourcePath))
            {
                audioManager.PlaySFXFromResources(resourcePath, volume, pitch);
            }
            else
            {
                Debug.LogWarning("No sound specified for UI element: " + gameObject.name);
            }
        }
    }
}