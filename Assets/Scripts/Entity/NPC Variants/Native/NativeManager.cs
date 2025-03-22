using System.Collections.Generic;
using UnityEngine;

public class NativeManager : NPCManager
{
    [SerializeField] private GameObject _nativePrefab;
    [SerializeField] private AbilityStatboardSO _abilityStatboardSO;
    private List<Native> _enemies = new List<Native>();


    public void SpawnMeleeNative(Vector3 position)
    {
        // // EnemyProperties enemyProperties = new EnemyProperties(_enemyPropertiesSO);
        NativeView nativeView = _poolManager.GetObject(_nativePrefab.name).GetComponent<NativeView>();

        SkillSystem skillSystem = new SkillSystem(new List<Skill> { });

        //State creation
        NativeIdlingProperties nativeIdlingProperties = new NativeIdlingProperties(2);
        NativeIdlingState nativeIdlingState = new NativeIdlingState(AnimationStateHelper.IS_IDLING, nativeIdlingProperties);

        NativeMovingProperties nativeMovingProperties = new NativeMovingProperties(2);
        NativeMovingState nativeMovingState = new NativeMovingState(nativeMovingProperties, AnimationStateHelper.IS_MOVING);

        NativeChasingProperties nativeChasingProperties = new NativeChasingProperties(2);
        NativeMeleeChasingState nativeChasingState = new NativeMeleeChasingState(nativeChasingProperties, AnimationStateHelper.IS_CHASING);

        NativeAttackingProperties nativeAttackingProperties = new NativeAttackingProperties();
        NativeMeleeAttackingState nativeAttackingState = new NativeMeleeAttackingState(AnimationStateHelper.IS_ATTACKING, nativeAttackingProperties);


        EntityStateMachine stateMachine = new EntityStateMachine();

        //Stat system creation
        AbilityStatBoard abilityStatBoard = new AbilityStatBoard(_abilityStatboardSO);
        StatSystem statSystem = new StatSystem(abilityStatBoard, AttackStatType.Strength);



        NativeProperties nativeProperties = new NativeProperties(EntityFaction.Native, statSystem.CombatStatBoard.Health.BaseValue, 10);


        //LevelSystem creation
        LevelSystem levelSystem = new LevelSystem();

        //HealthSystem creation (convert health to int)
        HealthSystem healthSystem = new HealthSystem((int)statSystem.CombatStatBoard.Health.ModifiedValue);

        //ManaSystem creation
        ManaSystem manaSystem = new ManaSystem((int)statSystem.CombatStatBoard.Mana.ModifiedValue);

        //StaminaSystem creation
        StaminaSystem staminaSystem = new StaminaSystem((int)statSystem.CombatStatBoard.Stamina.ModifiedValue);
        Inventory inventory = new Inventory();

        Native native = new Native(nativeView, nativeProperties, nativeIdlingState, nativeMovingState, nativeChasingState, nativeAttackingState, statSystem, skillSystem, levelSystem, healthSystem, manaSystem, staminaSystem, stateMachine, inventory);

        native.Initialize(this);

        native.NativeView.transform.position = position;
        _enemies.Add(native);
    }

    public void DespawnMeleeNative(Native native)
    {
        _poolManager.ReturnObject(_nativePrefab.name, native.NativeView.gameObject);
        native.Dispose();
        _enemies.Remove(native);
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
