using System;
using System.Collections.Generic;
using EntityBase.Player;
using GeneralManagers;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tile
{
    public class TileManager : MonoBehaviour
    {
        [SerializeField] private Tilemap groundTilemap;
        [SerializeField] private Tilemap walkInfrontTilemap;

        [SerializeField] private List<TileData> groundTileDataList;
        [SerializeField] private List<TileData> walkInfrontTileDataList;

        private Dictionary<TileBase, TileData> _groundTileDataDictionary;
        private Dictionary<TileBase, TileData> _walkInfrontTileDataDictionary;

        private PlayerView _playerView;

        private void Awake()
        {
            _groundTileDataDictionary = new Dictionary<TileBase, TileData>();
            _walkInfrontTileDataDictionary = new Dictionary<TileBase, TileData>();

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
                if (tileGround != null && _groundTileDataDictionary.TryGetValue(tileGround, out TileData groundTileData))
                {
                    Debug.Log($"Ground Tile Data: {groundTileData.name}");
                }
                else
                {
                    Debug.Log("No ground tile data found.");
                }

                if (tileWalkInFront != null && _walkInfrontTileDataDictionary.TryGetValue(tileWalkInFront, out TileData walkInfrontTileData))
                {
                    Debug.Log($"Walk In Front Tile Data: {walkInfrontTileData.name}");
                }
                else
                {
                    Debug.Log("No walk in front tile data found.");
                }
            }
        }
    }
}