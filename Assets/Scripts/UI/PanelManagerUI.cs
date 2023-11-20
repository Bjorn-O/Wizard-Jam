using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManagerUI : MonoBehaviour
{
    [SerializeField] private GameObject spellCardPanel;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject toolTip;
    [SerializeField] private GameObject lootPanelUI;
    [SerializeField] private GameObject fadeOut;

    // Start is called before the first frame update
    void Start()
    {
        fadeOut.SetActive(true);
        spellCardPanel.SetActive(false);
        inventoryPanel.SetActive(false);
    }

    public bool ToggleInventoryAndCardPanel()
    {
        if (lootPanelUI.activeSelf)
            return true;

        bool show = !spellCardPanel.activeSelf;
        spellCardPanel.SetActive(show);
        inventoryPanel.SetActive(show);

        if (!show)
        {
            toolTip.SetActive(false);
        }

        return show;
    }

    public void Retry()
    {
        LevelLoader.instance.ReloadCurrentScene();
    }

    public void QuitGame()
    {
        Invoke(nameof(Quit), 0.2f);
    }

    private void Quit()
    {
        Application.Quit();
    }
}
