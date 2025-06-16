using System;
using System.Collections.Generic;
using DefaultNamespace.Utility;
using Entity.Player;
using GeneralManagers;
using SceneSwitch;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tile
{
    public class WorldTileManager : MonoBehaviour
    {
        [SerializeField] private Tilemap groundTilemap;
        [SerializeField] private Tilemap walkInfrontTilemap;
        [SerializeField] private Tilemap collisionTilemap;
        public Tilemap GroundTilemap => groundTilemap;
        public Tilemap WalkInfrontTilemap => walkInfrontTilemap;
        public Tilemap CollisionTilemap => collisionTilemap;

        [SerializeField] private List<WorldTileData> groundTileDataList;
        [SerializeField] private List<WorldTileData> walkInfrontTileDataList;

        private Dictionary<TileBase, WorldTileData> _groundTileDataDictionary;
        private Dictionary<TileBase, WorldTileData> _walkInfrontTileDataDictionary;

        private PlayerView _playerView;

        public (Tilemap ground, Tilemap collision) GetTilemaps()
        {
            return (groundTilemap, collisionTilemap);
        }

        private void Awake()
        {
            _groundTileDataDictionary = new Dictionary<TileBase, WorldTileData>();
            _walkInfrontTileDataDictionary = new Dictionary<TileBase, WorldTileData>();

            foreach (var tileData in groundTileDataList)
            {
                foreach (var tile in tileData.tiles)
                {
                    if (!_groundTileDataDictionary.ContainsKey(tile))
                    {
                        _groundTileDataDictionary.Add(tile, tileData);
                    }
                }
            }

            foreach (var tileData in walkInfrontTileDataList)
            {
                foreach (var tile in tileData.tiles)
                {
                    if (!_walkInfrontTileDataDictionary.ContainsKey(tile))
                    {
                        _walkInfrontTileDataDictionary.Add(tile, tileData);
                    }
                }
            }
            ServiceLocator.RegisterService(this);
        }


        private void Update()
        {
            if (_playerView == null)
            {
                if (GameManager.Instance.PlayerManager.Player == null)
                {
                    return;
                }
                else
                {
                    _playerView = GameManager.Instance.PlayerManager.Player.PlayerView;
                }
            }

            Vector2 playerPosition = _playerView.transform.position;
            Vector3Int cellPositionGround = groundTilemap.WorldToCell(playerPosition);

            Vector3Int cellPositionWalkInFront = walkInfrontTilemap.WorldToCell(playerPosition);

            TileBase tileGround = groundTilemap.GetTile(cellPositionGround);
            TileBase tileWalkInFront = walkInfrontTilemap.GetTile(cellPositionWalkInFront);
            if (UnityEngine.Input.GetKeyDown(KeyCode.E))
            {
                // if (tileGround != null && _groundTileDataDictionary.TryGetValue(tileGround, out WorldTileData groundTileData))
                // {
                //     // Debug.Log($"Ground Tile Data: {groundTileData.name}");
                // }
                // else
                // {
                //     Debug.Log("No ground tile data found.");
                // }

                if (tileWalkInFront != null &&
                    _walkInfrontTileDataDictionary.TryGetValue(tileWalkInFront, out WorldTileData walkInfrontTileData))
                {
                    SceneSwitchManager.Instance.SwitchSceneFromOverworldToPortal(walkInfrontTileData.sceneToLoad,
                        playerPosition, SceneSwitchPortal.PortalToSpawnAt.One);
                }
                else
                {
                    Debug.Log("No walk in front tile data found.");
                }
            }
        }
    }
}