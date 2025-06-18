using System.Collections;
using DateDayNightSystem;
using Entity.Player;
using GeneralManagers;
using Input;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using DefaultNamespace.Utility;
using Tile;

namespace Entity.Player.Overworld
{
    public class OverworldPlayerView : MonoBehaviour
    {
        [Header("Sprites")] [SerializeField] private SpriteRenderer spriteRenderer;

        [Header("Movement")] [SerializeField] private float moveSpeed = 5f;
        private float moveDelay = 0.5f;

        private Player _player;
        private InputManager _inputManager;
        private Tilemap _groundTilemap;
        private Tilemap _collisionTilemap;
        private Vector3Int _currentGridPosition;
        private bool _isMoving = false;
        private float _lastMoveTime;

        private GameTimeManager _gameTimeManager;
        
        public void Initialize(Player player)
        {
            _player = player;
            _inputManager = player.InputManager;

            DontDestroyOnLoad(this);
            SceneManager.sceneLoaded += OnSceneLoaded;
            
            RefreshTilemaps();

            _gameTimeManager = GameManager.Instance.GameTimeManager;
        }

        private async void RefreshTilemaps()
        {
            var tileManager = await ServiceLocator.GetServiceAsync<WorldTileManager>(5f);

            if (tileManager != null)
            {
                _groundTilemap = tileManager.GroundTilemap;
                _collisionTilemap = tileManager.CollisionTilemap;

                // Update position based on new tilemaps
                if (_groundTilemap != null)
                {
                    _currentGridPosition = _groundTilemap.WorldToCell(transform.position);
                    transform.position = GetTileCenter(_currentGridPosition);
                    _lastMoveTime = Time.time;
                }
            }
            else
            {
                Debug.LogError("Failed to get WorldTileManager from ServiceLocator");
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "Scene_Overworld")
            {
                RefreshTilemaps();
                _currentGridPosition = _groundTilemap.WorldToCell(transform.position);
                transform.position = GetTileCenter(_currentGridPosition);
                _player.PlayerView.transform.position = transform.position;
                _lastMoveTime = Time.time;
            }
            else
            {
                // If not overworld, reset tilemaps
                _groundTilemap = null;
                _collisionTilemap = null;
            }
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void Update()
        {
            // Skip if tilemaps aren't available yet
            if (_groundTilemap == null) return;

            if (_isMoving || Time.time - _lastMoveTime < moveDelay)
                return;

            Vector2 input = _inputManager.GetMovementVector();

            if (input.magnitude > 0.1f)
            {
                Vector2 moveDirection;

                if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
                {
                    moveDirection = new Vector2(Mathf.Sign(input.x), 0);
                }
                else
                {
                    moveDirection = new Vector2(0, Mathf.Sign(input.y));
                }

                Vector3Int targetGridPosition =
                    _currentGridPosition + new Vector3Int((int)moveDirection.x, (int)moveDirection.y, 0);

                if (IsWalkable(targetGridPosition))
                {
                    StartCoroutine(MoveToGridPosition(targetGridPosition));
                }
            }
        }

        private bool IsWalkable(Vector3Int gridPosition)
        {
            return _groundTilemap.HasTile(gridPosition) &&
                   (_collisionTilemap == null || !_collisionTilemap.HasTile(gridPosition));
        }

        private IEnumerator MoveToGridPosition(Vector3Int gridPosition)
        {
            _isMoving = true;
            _lastMoveTime = Time.time;

            Vector3 startPosition = transform.position;
            Vector3 targetPosition = GetTileCenter(gridPosition);

            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime * moveSpeed;
                transform.position = Vector3.Lerp(startPosition, targetPosition, Mathf.Min(t, 1));
                yield return null;
            }

            _currentGridPosition = gridPosition;
            transform.position = targetPosition;
            _player.PlayerView.transform.position = targetPosition;
            _isMoving = false;
    
            // Advance game time by 30 minutes whenever player completes a move
            if (_gameTimeManager != null)
            {
                _gameTimeManager.AdvanceTimeByMinutes(30);
            }
        }

        private Vector3 GetTileCenter(Vector3Int gridPosition)
        {
            Vector3 worldPos = _groundTilemap.CellToWorld(gridPosition);
            Vector3 cellSize = _groundTilemap.cellSize;
            return worldPos + new Vector3(cellSize.x / 2, cellSize.y / 2, 0);
        }
    }
}