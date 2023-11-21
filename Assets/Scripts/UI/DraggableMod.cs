using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableMod : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Transform parentBeforeDrag;
    public Modifier modifier;
    public InventorySlotUI inventorySlotUI;
    public SpellCardUI cardUI;
    public ModSlotUI modSlotUI;
    private DraggableMod replacementMod;
    public bool inInventory = true;
    public bool isReplacement = false;
    private Image _image;
    private TooltipUI _tooltipUI;

    public void OnBeginDrag(PointerEventData eventData)
    {
        _tooltipUI.dragging = true;

        if (inInventory)
        {
            if (modifier == null)
            {
                modifier = inventorySlotUI.SlottedModifier;
            }

            if (inventorySlotUI.Count > 1 && replacementMod == null && transform.parent.childCount < inventorySlotUI.Count)
            {
                GameObject replacementObj = Instantiate(gameObject, transform.parent);
                replacementMod = replacementObj.GetComponent<DraggableMod>();
            }
            inventorySlotUI.UpdateSlot(null, false);
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
        _tooltipUI.dragging = false;

        if (inInventory)
        {
            inventorySlotUI.UpdateSlot(null, true);

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

        transform.SetParent(parentBeforeDrag, false);
        transform.localPosition = Vector2.zero;
    }

    private void Awake()
    {
        _image = GetComponent<Image>();
        inventorySlotUI = GetComponentInParent<InventorySlotUI>();
        _tooltipUI = FindObjectOfType<TooltipUI>(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_tooltipUI.dragging)
            return;

        if (modifier == null)
            modifier = inventorySlotUI.SlottedModifier;

        _tooltipUI.Show(modifier);
        _tooltipUI.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _tooltipUI.gameObject.SetActive(false);
    }
}
