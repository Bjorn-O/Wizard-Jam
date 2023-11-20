using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int loopCount = 0;
    public float completionTime = 0;
    public int killCount = 0;
    private bool _loading = false;

    private Modifier[] _savedInventory;
    private List<Modifier[]> _savedModifiersForSpells = new List<Modifier[]>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        for (int i = 0; i < 4; i++)
        {
            _savedModifiersForSpells.Add(new Modifier[3]);
        }
    }

    private void Start()
    {
        LevelLoader.instance.OnLoadingLevel += () => { _loading = true; };
        LevelLoader.instance.OnLevelLoaded += () => { _loading = false; };
    }

    private void Update()
    {
        if (_loading)
            return;

        completionTime += Time.deltaTime;
    }

    //public void SaveInventoryModifiers(Modifier[] modifiers)
    //{
    //    _savedInventory = modifiers;
    //}

    //public void SaveSpellIndexModifiers(int spellIndex, Modifier[] modifiers)
    //{
    //    _savedModifiersForSpells[spellIndex] = modifiers;

    //    foreach (var mod in _savedModifiersForSpells[spellIndex])
    //    {
    //        print(mod.name);
    //    }
    //}

    //public Modifier[] GetSavedInventoryModifiers()
    //{
    //    return _savedInventory;
    //}

    //public Modifier[] GetSavedModifiersForSpell(int spellIndex)
    //{
    //    return _savedModifiersForSpells[spellIndex];
    //}
}
