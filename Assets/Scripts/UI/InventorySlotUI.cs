using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    private string _modName;
    private string _modDescription;
    [SerializeField] private TextMeshProUGUI _count;
    [SerializeField] private Image _icon;

    // Start is called before the first frame update
    void Awake()
    {
        _icon.enabled = false;
        _count.enabled = false;
    }

    public void UpdateSlot(Modifier modifier, int count)
    {
        _icon.enabled = true;
        _count.enabled = true;
        _modName = modifier.name;
        _count.text = "x" + count;
        _icon.sprite = modifier.Icon;
    }
}
