using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpellCast : MonoBehaviour
{
    private HotbarUI hotbarUI;
    private SpellPanelUI spellPanelUI;
    private CharacterStats playerStats;

    [SerializeField] private Spell[] spells = new Spell[4];

    private float[] spellCooldowns = new float[4];

    // Start is called before the first frame update
    void Start()
    {
        playerStats = GetComponent<CharacterStats>();
        hotbarUI = FindObjectOfType<HotbarUI>();
        spellPanelUI = FindObjectOfType<SpellPanelUI>(true);

        for (int i = 0; i < spells.Length; i++)
        {
            Spell spell = spells[i];

            if (spell == null || spell.SpellIcon == null)
                continue;

            hotbarUI.UpdateIcon(i, spell.SpellIcon);
            spellPanelUI.UpdateSpellStats(spell);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Placeholder
        if (Input.GetKeyDown(KeyCode.M))
        {
            foreach (var spell in spells)
            {
                if (spell == null)
                    continue;

                foreach (var mod in spell.Modifiers)
                {
                    mod.SetSpell(spell);
                }
            }

            print("Apllied mods!");
        }

        UpdateCooldowns();
    }

    private void UpdateCooldowns()
    {
        for (int i = 0; i < spellCooldowns.Length; i++)
        {
            if (spellCooldowns[i] <= 0)
                continue;

            if (spells[i].cooldown == Mathf.Infinity)
            {
                hotbarUI.UpdateCooldown(i, 1, 1);
                continue;
            }

            spellCooldowns[i] -= Time.deltaTime;

            hotbarUI.UpdateCooldown(i, spellCooldowns[i], spells[i].cooldown);
        }
    }

    private void CastSpell(int index)
    {
        if (spells[index] == null)
            return;

        Spell spell = spells[index];

        if (spell.manaCost > playerStats.Mana || spellCooldowns[index] > 0)
            return;

        spellCooldowns[index] = spell.cooldown;
        StartCoroutine(spell.CastSpell());
    }

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
