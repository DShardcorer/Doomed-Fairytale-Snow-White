using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using System.Linq; // Don't forget this for LINQ methods!

public class FixTileSprites : EditorWindow
{
    private string tileFolder = "Assets/Tilesets";
    private string spriteSheetPath = "Assets/Resources/Tilesets/Exterior/door/Fantasy_door1.png";

    [MenuItem("Tools/Fix Broken Tile Sprites")]
    public static void ShowWindow()
    {
        GetWindow<FixTileSprites>();
    }

    void OnGUI()
    {
        GUILayout.Label("Auto-Reassign Sprites to Tiles", EditorStyles.boldLabel);
        tileFolder = EditorGUILayout.TextField("Tile Folder", tileFolder);
        spriteSheetPath = EditorGUILayout.TextField("Tileset Sprite", spriteSheetPath);

        if (GUILayout.Button("Fix Tiles"))
        {
            FixTiles();
        }
    }

    void FixTiles()
    {
        Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(spriteSheetPath)
            .OfType<Sprite>().ToArray();

        string[] tilePaths = Directory.GetFiles(tileFolder, "*.asset", SearchOption.AllDirectories);
        int fixedCount = 0;

        foreach (string path in tilePaths)
        {
            // Explicitly use UnityEngine.Tilemaps.Tile
            UnityEngine.Tilemaps.Tile tile = AssetDatabase.LoadAssetAtPath<UnityEngine.Tilemaps.Tile>(path);
            if (tile == null) continue;

            string tileName = Path.GetFileNameWithoutExtension(path);
            Sprite matchedSprite = sprites.FirstOrDefault(s => s.name == tileName);

            if (matchedSprite != null)
            {
                tile.sprite = matchedSprite;
                EditorUtility.SetDirty(tile);
                fixedCount++;
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"Reassigned {fixedCount} tiles.");
    }
}