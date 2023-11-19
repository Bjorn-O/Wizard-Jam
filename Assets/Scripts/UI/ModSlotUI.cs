using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ModSlotUI : MonoBehaviour, IDropHandler
{
    private SpellCardUI cardUI;
    public Modifier modifier;
    [SerializeField] private Transform _modParent;
    [SerializeField] private float _size = 25;

    public void OnDrop(PointerEventData eventData)
    {
        DraggableMod dragMod = eventData.pointerDrag.GetComponent<DraggableMod>();

        if (modifier)
        {
            dragMod.transform.SetParent(dragMod.parentBeforeDrag, false);
            return;
        }
       
        dragMod.inInventory = false;
        dragMod.GetComponent<RectTransform>().sizeDelta = Vector2.one * _size;
        dragMod.transform.SetParent(_modParent, false);
        dragMod.parentBeforeDrag = _modParent;
        modifier = dragMod.modifier;
        cardUI.UpdateStatsByMod(modifier, true);
        dragMod.cardUI = cardUI;
        dragMod.modSlotUI = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        cardUI = GetComponentInParent<SpellCardUI>();
    }
}
