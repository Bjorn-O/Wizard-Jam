using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    //DEBUG ONLY
    [SerializeField] private List<Modifier> _testMods = new List<Modifier>();
    private List<Modifier> _modifiers = new List<Modifier>();
    private Dictionary<Modifier, int> _modifierCount = new Dictionary<Modifier, int>();

    private PanelManagerUI panelManagerUI;
    private InventoryUI inventoryUI;
    private PlayerInput input;

    public UnityEvent OnInteraction;

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<PlayerInput>();
        panelManagerUI = FindObjectOfType<PanelManagerUI>(true);
        inventoryUI = FindObjectOfType<InventoryUI>(true);

        Modifier[] savedMods = GameManager.instance.GetSavedInventoryModifiers();

        if (savedMods != null)
        {
            foreach (var modifier in savedMods)
            {
                AddMod(modifier);
            }
        }
    }

    public void AddMod(Modifier modifier)
    {
        if (_modifiers.Contains(modifier))
        {
            _modifierCount[modifier]++;
            inventoryUI.UpdateModifier(modifier, _modifierCount[modifier]);
            return;
        }

        _modifiers.Add(modifier);
        _modifierCount.Add(modifier, 1);
        inventoryUI.UpdateModifier(modifier, 1);
    }

    public void RemoveMod(Modifier modifier)
    {
        if (!_modifiers.Contains(modifier))
            return;

        if (_modifierCount[modifier] > 1)
        {
            _modifierCount[modifier]--;
            inventoryUI.UpdateModifier(modifier, 0);
            return;
        }

        inventoryUI.UpdateModifier(modifier, 0);
        _modifiers.Remove(modifier);
        _modifierCount.Remove(modifier);
    }

    public void SaveInventory()
    {
        GameManager.instance.SaveInventoryModifiers(_modifiers.ToArray());
    }

    private void OnInventory()
    {
        bool showingPanels = panelManagerUI.ToggleInventoryAndCardPanel();
        SwitchControlMap(showingPanels ? "UI" : "Player");
    }

    public void SwitchControlMap(string mapString)
    {
        input.SwitchCurrentActionMap(mapString);

        Cursor.visible = mapString == "UI";
        Cursor.lockState = mapString == "UI" ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void OnInteract()
    {
        OnInteraction?.Invoke();
    }
}
