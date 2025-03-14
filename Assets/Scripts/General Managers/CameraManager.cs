using UnityEngine;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{
    private GameManager _gameManager;
    public void Initialize(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    [SerializeField] private CinemachineCamera cinemachineCamera;


    /// <summary>
    /// Sets the camera to follow the given target.
    /// </summary>
    public void SetFollowTarget(Transform newTarget)
    {
        if (cinemachineCamera != null && newTarget != null)
        {
            cinemachineCamera.Follow = newTarget;
        }
    }
}
