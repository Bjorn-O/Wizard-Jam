using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellCardUI : MonoBehaviour
{
    private Spell _spell;

    [Header("Card")]
    [SerializeField] private TextMeshProUGUI _spellName;
    [SerializeField] private TextMeshProUGUI _spellDescription;
    [SerializeField] private Image _spellIcon;

    [Header("Stats")]
    [SerializeField] private GameObject statsPanel;
    //[SerializeField] private TextMeshProUGUI[] statNames;
    [SerializeField] private TextMeshProUGUI[] statValues;
    [SerializeField] private List<float> startingValues = new List<float>();

    [Header("Mods")]
    [SerializeField] private Image[] modifierIcons = new Image[3];

    // Start is called before the first frame update
    void Start()
    {
        //modDragUI = FindObjectOfType<ModDragUI>();
    }

    public void ToggleStatsPanel(bool show)
    {
        statsPanel.SetActive(show);
    }

    public void UpdateStartingCardStats(Spell spell)
    {
        _spell = spell;
        _spellName.text = spell.SpellName;
        _spellIcon.sprite = spell.SpellIcon;

        startingValues.Clear();

        startingValues.Add(spell.damage);
        startingValues.Add(spell.manaCost);
        startingValues.Add(spell.cooldown);
        startingValues.Add(spell.effectScale);
        startingValues.Add(spell.castAmount);
        startingValues.Add(spell.effectAmount);

        for (int i = 0; i < statValues.Length; i++)
        {
            TextMeshProUGUI val = statValues[i];
            val.text = startingValues[i].ToString();
        }
    }

    public void UpdateStatsByMod(Modifier modifier, bool add)
    {
        if (add)
        {
            _spell.AddModifier(modifier);
            modifier.SetSpell(_spell, true);
        }
        else
        {
            _spell.RemoveModifier(modifier);
            modifier.RemoveSpell();
        }

        UpdateCard();
    }

    //Tired so i'm hardcoding...
    public void UpdateCard()
    {
        statValues[0].text = "" + Mathf.Round(_spell.damage * 10) / 10;
        GiveTextColor(0, _spell.damage, false);

        statValues[1].text = "" + Mathf.Round(_spell.manaCost * 10) / 10;
        GiveTextColor(1, _spell.manaCost, true);

        statValues[2].text = "" + Mathf.Round(_spell.cooldown * 10) / 10;
        GiveTextColor(2, _spell.cooldown, true);

        statValues[3].text = "" + Mathf.Round(_spell.effectScale * 100) / 100;
        GiveTextColor(3, _spell.effectScale, false);

        statValues[4].text = _spell.castAmount.ToString();
        GiveTextColor(4, _spell.castAmount, false);

        statValues[5].text = _spell.effectAmount.ToString();
        GiveTextColor(5, _spell.effectAmount, false);
    }
    
    public void GiveTextColor(int index, float val, bool oppositeColor)
    {
        if (val == startingValues[index])
        {
            statValues[index].color = Color.white;
            return;
        }

        bool valCheck = oppositeColor ? val < startingValues[index] : val > startingValues[index];
        statValues[index].color = valCheck ? Color.green : Color.red;
    }
}
