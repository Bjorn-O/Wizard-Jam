using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManagerUI : MonoBehaviour
{
    [SerializeField] private GameObject spellCardPanel;
    [SerializeField] private GameObject inventoryPanel;

    // Start is called before the first frame update
    void Start()
    {
        spellCardPanel.SetActive(false);
        inventoryPanel.SetActive(false);
    }

    public bool ToggleInventoryAndCardPanel()
    {
        bool show = !spellCardPanel.activeSelf;
        spellCardPanel.SetActive(show);
        inventoryPanel.SetActive(show);

        return show;
    }
}
