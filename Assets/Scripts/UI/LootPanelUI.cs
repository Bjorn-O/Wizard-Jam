using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootPanelUI : MonoBehaviour
{
    [SerializeField] private Image[] icons;
    public PlayerInventory playerInventory;

    public void UpdateModifierIcons(Modifier[] modifiers)
    {
        foreach (var icon in icons)
        {
            icon.gameObject.SetActive(false);
        }

        for (int i = 0; i < modifiers.Length; i++)
        {
            icons[i].sprite = modifiers[i].Icon;
            icons[i].gameObject.SetActive(true);
        }
    }

    public void HidePanel()
    {
        playerInventory.SwitchControlMap("Player");
        Invoke(nameof(DisablePanel), 0.05f);
    }

    private void DisablePanel()
    {
        gameObject.SetActive(false);
    }
}
