using System;
using TMPro;
using UnityEngine;

public class PlayerStatusUI : IngameMenuPageUI
{
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI playerLevelText;
    [SerializeField] private TextMeshProUGUI abilityStatsText;
    [SerializeField] private TextMeshProUGUI combatStatsText;

    public override void Initialize(IngameMenuUI parent)
    {
        base.Initialize(parent);
        PlayerStatusEventSystem.OnAbilityStatsChanged += PlayerStatusEventSystem_OnAbilityStatsChanged;
        PlayerStatusEventSystem.OnCombatStatsChanged += PlayerStatusEventSystem_OnCombatStatsChanged;

    }
    private void PlayerStatusEventSystem_OnAbilityStatsChanged(object sender, AbilityStatBoard e)
    {
        abilityStatsText.text = e.ToString();
    }
    private void PlayerStatusEventSystem_OnCombatStatsChanged(object sender, CombatStatBoard e)
    {
        combatStatsText.text = e.ToString();
    }


}
