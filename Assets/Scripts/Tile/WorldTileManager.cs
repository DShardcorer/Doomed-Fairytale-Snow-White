using System;
using System.Collections.Generic;
using DefaultNamespace.Utility;
using EntityBase.Player;
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
        [SerializeField] private Tilemap landMarksTilemap;
        
        public Tilemap GroundTilemap => groundTilemap;
        public Tilemap WalkInfrontTilemap => walkInfrontTilemap;
        public Tilemap CollisionTilemap => collisionTilemap;
        public Tilemap LandMarksTilemap => landMarksTilemap;

        [SerializeField] private List<WorldTileData> groundTileDataList;
        [SerializeField] private List<WorldTileData> walkInfrontTileDataList;
        [SerializeField] private List<WorldTileData> landMarksTileDataList;

        private Dictionary<TileBase, WorldTileData> _groundTileDataDictionary;
        private Dictionary<TileBase, WorldTileData> _walkInfrontTileDataDictionary;
        private Dictionary<TileBase, WorldTileData> _landMarksTileDataDictionary;

        private PlayerView _playerView;

        public (Tilemap ground, Tilemap collision) GetTilemaps()
        {
            return (groundTilemap, collisionTilemap);
        }

        private void Awake()
        {
            _groundTileDataDictionary = new Dictionary<TileBase, WorldTileData>();
            _walkInfrontTileDataDictionary = new Dictionary<TileBase, WorldTileData>();
            _landMarksTileDataDictionary = new Dictionary<TileBase, WorldTileData>();

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
            
            foreach (var tileData in landMarksTileDataList)
            {
                foreach (var tile in tileData.tiles)
                {
                    if (!_landMarksTileDataDictionary.ContainsKey(tile))
                    {
                        _landMarksTileDataDictionary.Add(tile, tileData);
                    }
                }
            }
            
            ServiceLocator.RegisterService(this);
        }
        private void OnDestroy()
        {
            ServiceLocator.UnregisterService(this);
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
            
            Vector3Int cellPositionLandmarks = landMarksTilemap.WorldToCell(playerPosition);
            Vector3Int cellPositionGround = groundTilemap.WorldToCell(playerPosition);
            Vector3Int cellPositionWalkInFront = walkInfrontTilemap.WorldToCell(playerPosition);

            TileBase tileLandmarks = landMarksTilemap.GetTile(cellPositionLandmarks);
            TileBase tileGround = groundTilemap.GetTile(cellPositionGround);
            TileBase tileWalkInFront = walkInfrontTilemap.GetTile(cellPositionWalkInFront);
            
            if (UnityEngine.Input.GetKeyDown(KeyCode.E))
            {
                // Check landmarks first (prioritized)
                if (tileLandmarks != null &&
                    _landMarksTileDataDictionary.TryGetValue(tileLandmarks, out WorldTileData landmarkTileData))
                {
                    SceneSwitchManager.Instance.SwitchSceneFromOverworldToPortal(landmarkTileData.sceneToLoad,
                        playerPosition);
                }
                // Then check walk in front tiles
                else if (tileWalkInFront != null &&
                    _walkInfrontTileDataDictionary.TryGetValue(tileWalkInFront, out WorldTileData walkInfrontTileData))
                {
                    SceneSwitchManager.Instance.SwitchSceneFromOverworldToPortal(walkInfrontTileData.sceneToLoad,
                        playerPosition);
                }
                else
                {
                    Debug.Log("No interactable tile data found.");
                }
            }
        }
    }
}