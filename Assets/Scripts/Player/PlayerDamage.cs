using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    private PlayerStatsUI _playerStatsUI;
    private CharacterStats _characterStats;

    // Start is called before the first frame update
    void Start()
    {
        _playerStatsUI = FindObjectOfType<PlayerStatsUI>();
        _characterStats = GetComponent<CharacterStats>();
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
}
