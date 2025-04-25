using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UI.Dialogue.Sprites
{
    [System.Serializable]
    public class EmotionSpriteMap
    {
        [DictionaryDrawerSettings(KeyLabel = "Emotion", ValueLabel = "Sprite")]
        public Dictionary<string, Sprite> emotionToSprite = new();
    
        public Sprite GetEmotionSprite(string emotion)
        {
            if (emotionToSprite.TryGetValue(emotion, out var sprite))
                return sprite;

            Debug.LogWarning($"No sprite for emotion '{emotion}'");
            return null;
        }
    }

}