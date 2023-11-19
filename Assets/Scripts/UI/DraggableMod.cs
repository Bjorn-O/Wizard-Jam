using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableMod : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform parentBeforeDrag;
    public Modifier modifier;
    public InventorySlotUI inventorySlotUI;
    public SpellCardUI cardUI;
    public ModSlotUI modSlotUI;
    private DraggableMod replacementMod;
    public bool inInventory = true;
    private Image _image;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (inInventory)
        {
            if (modifier == null)
            {
                modifier = inventorySlotUI.SlottedModifier;
            }

            if (inventorySlotUI.Count > 1 && replacementMod == null)
            {
                GameObject replacementObj = Instantiate(gameObject, transform.parent);
                replacementMod = replacementObj.GetComponent<DraggableMod>();
            }
            inventorySlotUI.UpdateSlot(null, inventorySlotUI.Count - 1);
        }
        else
        {
            cardUI.UpdateStatsByMod(modifier, false);
            modSlotUI.modifier = null;
        }
        
        parentBeforeDrag = transform.parent;
        transform.SetParent(transform.root);
        _image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _image.raycastTarget = true;

        if (inInventory)
        {
            inventorySlotUI.UpdateSlot(null, inventorySlotUI.Count + 1);

            if (cardUI == null)
            {
                transform.SetParent(parentBeforeDrag, false);
                if (replacementMod != null && replacementMod.inventorySlotUI == inventorySlotUI)
                {
                    Destroy(replacementMod.gameObject);
                }
            }            

            cardUI = null;
        }

        transform.localPosition = Vector2.zero;
    }

    private void Awake()
    {
        _image = GetComponent<Image>();
        inventorySlotUI = GetComponentInParent<InventorySlotUI>();
    }
}
