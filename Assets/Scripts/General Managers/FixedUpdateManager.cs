using System.Collections.Generic;
using UnityEngine;

public class FixedUpdateManager : MonoBehaviour, ILifecycle<GameManager>
{
    private GameManager _gameManager;
    private List<IFixedUpdatable> _fixedUpdatables = new List<IFixedUpdatable>();

    public void Initialize(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void Dispose()
    {
        _fixedUpdatables.Clear();
        _gameManager = null;
    }



    public void AddFixedUpdatable(IFixedUpdatable fixedUpdatable)
    {
        if (!_fixedUpdatables.Contains(fixedUpdatable))
        {
            _fixedUpdatables.Add(fixedUpdatable);
        }
    }

    public void RemoveFixedUpdatable(IFixedUpdatable fixedUpdatable)
    {
        if (_fixedUpdatables.Contains(fixedUpdatable))
        {
            _fixedUpdatables.Remove(fixedUpdatable);
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < _fixedUpdatables.Count; i++)
        {
            _fixedUpdatables[i].FixedUpdateLogic();
        }
    }

    private void OnDestroy()
    {
        _fixedUpdatables.Clear();
    }


}
