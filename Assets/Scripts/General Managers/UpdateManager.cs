using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour, ILifecycle<GameManager>
{
    private GameManager _gameManager;
    private List<IUpdatable> _updatables = new List<IUpdatable>();

    public void Initialize(GameManager gameManager)
    {
        _gameManager = gameManager;
    }
    public void Dispose()
    {
        _updatables.Clear();
        _gameManager = null;
    }




    public void AddUpdatable(IUpdatable updatable)
    {
        if (!_updatables.Contains(updatable))
        {
            _updatables.Add(updatable);
        }
    }

    public void RemoveUpdatable(IUpdatable updatable)
    {
        if (_updatables.Contains(updatable))
        {
            _updatables.Remove(updatable);
        }
    }

    private void Update()
    {
        for (int i = 0; i < _updatables.Count; i++)
        {
            _updatables[i].UpdateLogic();
        }
    }

    private void OnDestroy()
    {
        _updatables.Clear();
    }


}
