using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlotUI : MonoBehaviour, IDropHandler
{
    //private ModDragUI _modDragUI;
    private string _modName;
    private string _modDescription;
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private Image _icon;
    [SerializeField] private Transform _modParent;
    [SerializeField] private float _size = 30;
    private Modifier _modifier;
    public Modifier SlottedModifier { get { return _modifier; } }
    private int _count = 0;
    public int Count { get { return _count; } set { _count = value; } }

    // Start is called before the first frame update
    void Awake()
    {
        //_modDragUI = FindObjectOfType<ModDragUI>();
        _icon.enabled = false;
        _countText.enabled = false;
    }

    public void RemoveMod()
    {
        _modifier = null;
        _icon.enabled = false;
        _count = 0;
        _countText.enabled = false;
    }

    public void UpdateSlot(Modifier modifier, int count)
    {
        if (modifier != null)
        {
            _modifier = modifier;
            _icon.enabled = true;
            _modName = modifier.name;
            _icon.sprite = modifier.Icon;
        }

        _count = count;
        _countText.enabled = count > 0;
        _countText.text = "x" + count;
    }

    public void OnDrop(PointerEventData eventData)
    {
        DraggableMod dragMod = eventData.pointerDrag.GetComponent<DraggableMod>();

        if (_count > 0 && _modifier != dragMod.modifier)
        {
            if (_modifier == dragMod.modifier)
            {
                UpdateSlot(null, _count + 1);
                Destroy(dragMod.gameObject);
                return;
            }
            else
            {
                dragMod.transform.SetParent(dragMod.parentBeforeDrag, false);
                return;
            }
        }

        dragMod.inventorySlotUI.Drop(dragMod);
    }

    public void Drop(DraggableMod dragMod)
    {
        dragMod.transform.SetParent(_modParent, false);
        dragMod.parentBeforeDrag = _modParent;
        dragMod.GetComponent<RectTransform>().sizeDelta = Vector2.one * _size;
        dragMod.inInventory = true;
    }
}
