using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    private PlayerStatsUI _playerStatsUI;
    private PlayerInventory _playerInventory;
    private DeathFinishPanelUI _deathFinishPanelUI;
    private CharacterStats _characterStats;

    // Start is called before the first frame update
    void Start()
    {
        _playerStatsUI = FindObjectOfType<PlayerStatsUI>();
        _deathFinishPanelUI = FindObjectOfType<DeathFinishPanelUI>(true);
        _characterStats = GetComponent<CharacterStats>();
        _playerInventory = GetComponent<PlayerInventory>();
    }

    public void OnDamage()
    {
        _playerStatsUI.UpdateHealth(_characterStats.Health, _characterStats.MaxHealth);
        _playerStatsUI.ShowHitFlash();
    }

    public void OnHeal()
    {
        _playerStatsUI.UpdateHealth(_characterStats.Health, _characterStats.MaxHealth);
    }

    public void OnDeath()
    {
        GameManager.instance.ClearSavedInventory();
        _playerInventory.SwitchControlMap("UI");
        _deathFinishPanelUI.ShowPanel(true);
    }
}
