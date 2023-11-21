using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManagerUI : MonoBehaviour
{
    [SerializeField] private GameObject spellCardPanel;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject toolTip;
    [SerializeField] private GameObject lootPanelUI;
    [SerializeField] private GameObject deathPanelUI;
    [SerializeField] private GameObject fadeOut;
    [SerializeField] private GameObject fadeIn;

    // Start is called before the first frame update
    void Start()
    {
        fadeOut.SetActive(true);
        spellCardPanel.SetActive(false);
        inventoryPanel.SetActive(false);
    }

    public bool ToggleInventoryAndCardPanel()
    {
        if (lootPanelUI.activeSelf || deathPanelUI.activeSelf)
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

    public void ShowFade()
    {
        fadeIn.SetActive(true);
        Invoke(nameof(HideFade), 0.2f);
    }

    public void HideFade()
    {
        fadeIn.SetActive(false);
    }

    public void ShowFadeOut()
    {
        fadeOut.SetActive(false);
        fadeOut.SetActive(true);
    }

    public void Retry()
    {
        Invoke(nameof(ReloadScene), 0.2f);
    }

    private void ReloadScene()
    {
        GameManager.instance.completionTime = 0;
        GameManager.instance.loopCount = 0;
        GameManager.instance.killCount = 0;
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
