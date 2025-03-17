using UnityEngine;

public class NativeView : NPCView
{
    [SerializeField] private Transform _attackCheck;
    public Transform AttackCheck => _attackCheck;
    [SerializeField] private float _attackCheckRadius;
    public float AttackCheckRadius => _attackCheckRadius;
    private Native _native;

    public override void Initialize(Entity controller)
    {
        base.Initialize(controller);
        _native = (Native)controller;
        gameObject.SetActive(true);
    }


}
