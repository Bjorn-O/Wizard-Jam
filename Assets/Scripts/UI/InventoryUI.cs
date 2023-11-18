using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventorySlotUI[] _inventorySlots;
    [SerializeField] private List<Modifier> _modifiersAdded = new List<Modifier>();
    [SerializeField] private Dictionary<Modifier, InventorySlotUI> _modSlotPair = new Dictionary<Modifier, InventorySlotUI>();

    // Start is called before the first frame update
    void Awake()
    {
        _inventorySlots = GetComponentsInChildren<InventorySlotUI>();
    }

    public void UpdateModifier(Modifier modifier, int count)
    {
        if (_modifiersAdded.Contains(modifier))
        {
            InventorySlotUI slot = _modSlotPair[modifier];
            slot.UpdateSlot(modifier, count);
        }
        else
        {
            InventorySlotUI slot = _inventorySlots[_modSlotPair.Count];
            _modSlotPair.Add(modifier, slot);
            slot.UpdateSlot(modifier, count);
            _modifiersAdded.Add(modifier);
        }
    }
}
