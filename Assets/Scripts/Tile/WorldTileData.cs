using SceneSwitch;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tile
{
    [CreateAssetMenu(fileName = "WorldTileData", menuName = "Tile/WorldTileData")]
    public class WorldTileData: ScriptableObject
    {
        public TileBase[] tiles;
        public WorldTileType tileType;
        public SceneField sceneToLoad;
        
    }
}