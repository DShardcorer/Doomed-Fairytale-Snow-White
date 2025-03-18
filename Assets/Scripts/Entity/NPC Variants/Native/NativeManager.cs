using System.Collections.Generic;
using UnityEngine;

public class NativeManager : NPCManager
{
    [SerializeField] private GameObject _nativePrefab;
    [SerializeField] private NPCPropertiesSO _nativePropertiesSO;
    private List<Native> _enemies = new List<Native>();


    public void SpawnMeleeNative(Vector3 position)
    {
        // // EnemyProperties enemyProperties = new EnemyProperties(_enemyPropertiesSO);
        NativeView nativeView = _poolManager.GetObject(_nativePrefab.name).GetComponent<NativeView>();
        NativeProperties nativeProperties = new NativeProperties(_nativePropertiesSO);


        SkillSystem skillSystem = new SkillSystem(new List<Skill> {});

        //State creation
        NativeIdlingProperties nativeIdlingProperties = new NativeIdlingProperties(2);
        NativeIdlingState nativeIdlingState = new NativeIdlingState(nativeIdlingProperties, AnimationStateHelper.IS_IDLING);

        NativeMovingProperties nativeMovingProperties = new NativeMovingProperties(2);
        NativeMovingState nativeMovingState = new NativeMovingState(nativeMovingProperties, AnimationStateHelper.IS_MOVING);

        NativeChasingProperties nativeChasingProperties = new NativeChasingProperties(2);
        NativeMeleeChasingState nativeChasingState = new NativeMeleeChasingState(nativeChasingProperties, AnimationStateHelper.IS_CHASING);

        NativeAttackingProperties nativeAttackingProperties = new NativeAttackingProperties();
        NativeMeleeAttackingState nativeAttackingState = new NativeMeleeAttackingState(nativeAttackingProperties, AnimationStateHelper.IS_ATTACKING);


        EntityStateMachine stateMachine = new EntityStateMachine();

        Native native = new Native(nativeView, nativeProperties,
         nativeIdlingState, nativeMovingState, nativeChasingState, nativeAttackingState,
        skillSystem, stateMachine);

        native.Initialize(this);

        native.NativeView.transform.position = position;
        _enemies.Add(native);
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            SpawnMeleeNative(new Vector3(0, 0, 0));
        }
    }


    public override void Dispose()
    {
        base.Dispose();
        _enemies.Clear();
    }



}
