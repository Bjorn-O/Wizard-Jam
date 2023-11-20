using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellPanelUI : MonoBehaviour
{
    [SerializeField] private SpellCardUI[] _spellCardUIs;
    private Dictionary<Spell, SpellCardUI> _spellCardPair = new Dictionary<Spell, SpellCardUI>();
    private int _spellsAdded = 0;

    public void UpdateSpellStats(Spell spell)
    {
        if (_spellCardPair.ContainsKey(spell))
        {
            _spellCardPair[spell].UpdateStartingCardStats(spell);
            return;
        }

        SpellCardUI cardUI = _spellCardUIs[_spellsAdded];
        _spellCardPair.Add(spell, cardUI);
        cardUI.UpdateStartingCardStats(spell);

        _spellsAdded++;
    }
}
