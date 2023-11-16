using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarUI : MonoBehaviour
{
    [SerializeField] private SpellSlotUI[] spellSlotUIs;

    public void UpdateIcon(int index, Sprite sprite)
    {
        spellSlotUIs[index].icon.sprite = sprite;
    }

    public void UpdateCooldown(int index, float currentTime, float cooldownTime)
    {
        spellSlotUIs[index].cooldown.fillAmount = currentTime / cooldownTime;
    }
}

[System.Serializable]
public class SpellSlotUI
{
    public Image icon;
    public Image cooldown;
}
