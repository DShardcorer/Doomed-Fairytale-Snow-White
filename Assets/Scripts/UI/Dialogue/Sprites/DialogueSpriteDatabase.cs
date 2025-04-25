namespace UI.Dialogue.Sprites
{
    using UnityEngine;
    using Sirenix.OdinInspector;
    using System.Collections.Generic;

    [CreateAssetMenu(fileName = "DialogueSpriteDatabase", menuName = "Dialogue/SpriteDatabase")]
    public class DialogueSpriteDatabase : SerializedScriptableObject
    {
        [DictionaryDrawerSettings(KeyLabel = "Character ID", ValueLabel = "Emotions")]
        public Dictionary<string, EmotionSpriteMap> characterSprites = new();

        public Sprite GetSprite(string characterId, string emotion)
        {
            if (characterSprites.TryGetValue(characterId, out var emotionMap))
                return emotionMap.GetEmotionSprite(emotion);

            Debug.LogWarning($"No portrait found for character ID: {characterId}");
            return null;
        }
    }
}