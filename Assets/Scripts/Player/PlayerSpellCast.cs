using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpellCast : MonoBehaviour
{
    private HotbarUI _hotbarUI;
    private SpellPanelUI _spellPanelUI;
    private CharacterStats _characterStats;
    private PlayerStatsUI _playerStatsUI;

    [SerializeField] private Spell[] spells = new Spell[4];
    [SerializeField] private float _manaOverTime = 1;

    private float[] spellCooldowns = new float[4];

    // Start is called before the first frame update
    void Start()
    {
        _characterStats = GetComponent<CharacterStats>();
        _hotbarUI = FindObjectOfType<HotbarUI>();
        _spellPanelUI = FindObjectOfType<SpellPanelUI>(true);
        _playerStatsUI = FindObjectOfType<PlayerStatsUI>();

        for (int i = 0; i < spells.Length; i++)
        {
            Spell spell = spells[i];

            if (spell == null || spell.SpellIcon == null)
                continue;

            _hotbarUI.UpdateIcon(i, spell.SpellIcon);
            _spellPanelUI.UpdateSpellStats(spell);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCooldowns();
        AddManaOverTime();
    }

    private void AddManaOverTime()
    {
        _characterStats.Mana += _manaOverTime * Time.deltaTime;
        _playerStatsUI.UpdateMana(_characterStats.Mana, _characterStats.MaxMana);
    }

    private void UpdateCooldowns()
    {
        for (int i = 0; i < spellCooldowns.Length; i++)
        {
            if (spellCooldowns[i] <= 0)
                continue;

            if (spells[i].cooldown == Mathf.Infinity)
            {
                _hotbarUI.UpdateCooldown(i, 1, 1);
                continue;
            }

            spellCooldowns[i] -= Time.deltaTime;

            _hotbarUI.UpdateCooldown(i, spellCooldowns[i], spells[i].cooldown);
        }
    }

    private void CastSpell(int index)
    {
        if (spells[index] == null)
            return;

        Spell spell = spells[index];

        if (spell.manaCost > _characterStats.Mana || spellCooldowns[index] > 0)
            return;

        _characterStats.Mana -= spell.manaCost;
        spellCooldowns[index] = spell.cooldown;
        StartCoroutine(spell.CastSpell());
    }

    //public void SaveAllModifiers()
    //{
    //    for (int i = 0; i < spells.Length; i++)
    //    {
    //        GameManager.instance.SaveSpellIndexModifiers(i, spells[i].Modifiers.ToArray());
    //    }
    //}

    //public void GetAllModifiers()
    //{
    //    for (int i = 0; i < spells.Length; i++)
    //    {
    //        Modifier[] mods = GameManager.instance.GetSavedModifiersForSpell(i);
    //        spells[i].ClearModifiers();

    //        foreach (var mod in mods)
    //        {
    //            if (mod == null)
    //                continue;

    //            spells[i].AddModifier(mod);
    //        }
    //    }
    //}

    private void OnHotbar1()
    {
        CastSpell(0);
    }

    private void OnHotbar2()
    {
        CastSpell(1);
    }

    private void OnHotbar3()
    {
        CastSpell(2);
    }

    private void OnHotbar4()
    {
        CastSpell(3);
    }
}
