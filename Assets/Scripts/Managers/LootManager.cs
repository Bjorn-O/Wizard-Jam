using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    [SerializeField] private Modifier[] _availableMods;
    private PlayerInventory _playerInventory;
    private LootPanelUI _lootPanelUI;
    public PlayerInventory GetPlayerInventory { get { return _playerInventory; } }

    private void Awake()
    {
        _playerInventory = FindObjectOfType<PlayerInventory>();
        _lootPanelUI = FindObjectOfType<LootPanelUI>(true);
        _lootPanelUI.gameObject.SetActive(false);
    }

    public Modifier GetRandomModifier()
    {
        return _availableMods[Random.Range(0, _availableMods.Length)];
    }

    public void GiveModsToPlayer(List<Modifier> mods)
    {
        _lootPanelUI.gameObject.SetActive(false);
        _lootPanelUI.gameObject.SetActive(true);
        _lootPanelUI.playerInventory = _playerInventory;
        _lootPanelUI.UpdateModifierIcons(mods.ToArray());
        _playerInventory.SwitchControlMap("UI");

        foreach (var mod in mods)
        {
            _playerInventory.AddMod(mod);
        }
    }
}
