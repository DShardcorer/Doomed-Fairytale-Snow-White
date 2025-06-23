using GeneralManagers;
using Pool;
using UnityEngine;

namespace EntityBase.NPC
{
    public class NPCManager : MonoBehaviour, ILifecycle<GameManager>
    {

        protected GameManager _gameManager;

        public GameManager GameManager => _gameManager;
        protected PoolManager _poolManager;

        public void Initialize(GameManager gameManager)
        {
            _gameManager = gameManager;
            _poolManager = _gameManager.PoolManager;
        }
    



        public virtual void Dispose()
        {
            _gameManager = null;
            _poolManager = null;
        }
    }
}
