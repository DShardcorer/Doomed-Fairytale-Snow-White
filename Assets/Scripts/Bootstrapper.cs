using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    private void Start()
    {
        // Ensure GameManager and its components are initialized first.
        GameManager.Instance.Initialize();
        
        // Then initialize UIManager or notify UI components.
        UIManager.Instance.Initialize();
    }
}
