
using Events.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUI : IngameMenuPageUI
{
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI playerLevelText;
    [SerializeField] private TextMeshProUGUI playerExperienceText;

    [SerializeField] private TextMeshProUGUI StrengthAbilityStatText;
    [SerializeField] private TextMeshProUGUI DexterityAbilityStatText;
    [SerializeField] private TextMeshProUGUI ConstitutionAbilityStatText;
    [SerializeField] private TextMeshProUGUI IntelligenceAbilityStatText;
    [SerializeField] private TextMeshProUGUI WisdomAbilityStatText;
    [SerializeField] private TextMeshProUGUI CharismaAbilityStatText;

    [SerializeField] private Button increaseStrengthButton;
    [SerializeField] private Button increaseDexterityButton;
    [SerializeField] private Button increaseConstitutionButton;
    [SerializeField] private Button increaseIntelligenceButton;
    [SerializeField] private Button increaseWisdomButton;
    [SerializeField] private Button increaseCharismaButton;

    [SerializeField] private TextMeshProUGUI unallocatedStatPointsText;

    [SerializeField] private TextMeshProUGUI combatStatsText;

    private int _unallocatedAbilityStatPoints = 0;

    public override void Initialize(IngameMenuUI parent)
    {
        base.Initialize(parent);
        PlayerStatsEventSystem.OnInitialAbilityStatsSet += PlayerStatusEventSystem_OnAbilityStatsChanged;
        PlayerStatsEventSystem.OnInitialCombatStatsSet += PlayerStatusEventSystem_OnCombatStatsChanged;
        PlayerStatsEventSystem.OnAbilityStatsChanged += PlayerStatusEventSystem_OnAbilityStatsChanged;
        PlayerStatsEventSystem.OnCombatStatsChanged += PlayerStatusEventSystem_OnCombatStatsChanged;
        PlayerLevelEventSystem.OnLevelChanged += PlayerLevelEventSystem_OnLevelChanged;
        PlayerLevelEventSystem.OnExperienceChanged += PlayerLevelEventSystem_OnExperienceChanged;
        PlayerLevelEventSystem.OnInitialLevelSet += PlayerLevelEventSystem_OnInitialLevelSet;
        PlayerLevelEventSystem.OnInitialExperienceSet += PlayerLevelEventSystem_OnInitialExperienceSet;

        increaseStrengthButton.onClick.AddListener(() => { 
            PlayerStatsEventSystem.InvokeStatPointAllocated(StatType.Strength); 
            DecrementStatPoint();
        });
        increaseDexterityButton.onClick.AddListener(() => { 
            PlayerStatsEventSystem.InvokeStatPointAllocated(StatType.Dexterity); 
            DecrementStatPoint();
        });
        increaseConstitutionButton.onClick.AddListener(() => { 
            PlayerStatsEventSystem.InvokeStatPointAllocated(StatType.Constitution); 
            DecrementStatPoint();
        });
        increaseIntelligenceButton.onClick.AddListener(() => { 
            PlayerStatsEventSystem.InvokeStatPointAllocated(StatType.Intelligence); 
            DecrementStatPoint();
        });
        increaseWisdomButton.onClick.AddListener(() => { 
            PlayerStatsEventSystem.InvokeStatPointAllocated(StatType.Wisdom); 
            DecrementStatPoint();
        });
        increaseCharismaButton.onClick.AddListener(() => { 
            PlayerStatsEventSystem.InvokeStatPointAllocated(StatType.Charisma); 
            DecrementStatPoint();
        });

        DisableAllocationButtons();
        UpdateUnallocatedStatPointsText();
    }

    private void EnableAllocationButtons()
    {
        increaseStrengthButton.gameObject.SetActive(true);
        increaseDexterityButton.gameObject.SetActive(true);
        increaseConstitutionButton.gameObject.SetActive(true);
        increaseIntelligenceButton.gameObject.SetActive(true);
        increaseWisdomButton.gameObject.SetActive(true);
        increaseCharismaButton.gameObject.SetActive(true);
    }

    private void DisableAllocationButtons()
    {
        increaseStrengthButton.gameObject.SetActive(false);
        increaseDexterityButton.gameObject.SetActive(false);
        increaseConstitutionButton.gameObject.SetActive(false);
        increaseIntelligenceButton.gameObject.SetActive(false);
        increaseWisdomButton.gameObject.SetActive(false);
        increaseCharismaButton.gameObject.SetActive(false);
    }

    /// <summary>
    /// Decrements the unallocated stat points by one, updates the UI, and disables buttons when points are spent.
    /// </summary>
    private void DecrementStatPoint()
    {
        _unallocatedAbilityStatPoints--;
        if (_unallocatedAbilityStatPoints <= 0)
        {
            _unallocatedAbilityStatPoints = 0;
            DisableAllocationButtons();
        }
        UpdateUnallocatedStatPointsText();
    }

    /// <summary>
    /// Updates the unallocated stat points UI text.
    /// </summary>
    private void UpdateUnallocatedStatPointsText()
    {
        if (_unallocatedAbilityStatPoints == 0){
            unallocatedStatPointsText.text = "";
            return;
        }
        unallocatedStatPointsText.text = $"Unallocated Points: {_unallocatedAbilityStatPoints}";
    }

    public override void Dispose()
    {
        base.Dispose();
        PlayerStatsEventSystem.OnInitialAbilityStatsSet -= PlayerStatusEventSystem_OnAbilityStatsChanged;
        PlayerStatsEventSystem.OnInitialCombatStatsSet -= PlayerStatusEventSystem_OnCombatStatsChanged;
        PlayerStatsEventSystem.OnAbilityStatsChanged -= PlayerStatusEventSystem_OnAbilityStatsChanged;
        PlayerStatsEventSystem.OnCombatStatsChanged -= PlayerStatusEventSystem_OnCombatStatsChanged;
        PlayerLevelEventSystem.OnLevelChanged -= PlayerLevelEventSystem_OnLevelChanged;
        PlayerLevelEventSystem.OnExperienceChanged -= PlayerLevelEventSystem_OnExperienceChanged;
        PlayerLevelEventSystem.OnInitialLevelSet -= PlayerLevelEventSystem_OnInitialLevelSet;
        PlayerLevelEventSystem.OnInitialExperienceSet -= PlayerLevelEventSystem_OnInitialExperienceSet;
    }

    private void PlayerLevelEventSystem_OnInitialExperienceSet(object sender, OnExperienceChangedEventArgs e)
    {
        playerExperienceText.text = $"{e.Experience}/{e.ExperienceToNextLevel} XP";
    }

    private void PlayerLevelEventSystem_OnInitialLevelSet(object sender, OnLevelChangedEventArgs e)
    {
        playerLevelText.text = "Level " + e.Level.ToString();
    }

    private void PlayerLevelEventSystem_OnExperienceChanged(object sender, OnExperienceChangedEventArgs e)
    {
        playerExperienceText.text = $"{e.Experience}/{e.ExperienceToNextLevel} XP";
    }

    private void PlayerLevelEventSystem_OnLevelChanged(object sender, OnLevelChangedEventArgs e)
    {
        playerLevelText.text = "Level " + e.Level.ToString();
        _unallocatedAbilityStatPoints += 6;
        UpdateUnallocatedStatPointsText();
        EnableAllocationButtons();
    }

    private void PlayerStatusEventSystem_OnAbilityStatsChanged(object sender, AbilityStatBoard e)
    {
        StrengthAbilityStatText.text = "Strength: " + e.Strength.ToString();
        DexterityAbilityStatText.text = "Dexterity: " + e.Dexterity.ToString();
        ConstitutionAbilityStatText.text = "Constitution: " + e.Constitution.ToString();
        IntelligenceAbilityStatText.text = "Intelligence: " + e.Intelligence.ToString();
        WisdomAbilityStatText.text = "Wisdom: " + e.Wisdom.ToString();
        CharismaAbilityStatText.text = "Charisma: " + e.Charisma.ToString();
    }

    private void PlayerStatusEventSystem_OnCombatStatsChanged(object sender, CombatStatBoard e)
    {
        combatStatsText.text = e.ToString();
    }
}
