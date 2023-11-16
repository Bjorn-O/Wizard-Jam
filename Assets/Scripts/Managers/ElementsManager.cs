using System.Collections.Generic;
using UnityEngine;

public enum Elements
{
    Unaspected, Fire, Water, Earth, Wind
}

public class ElementsManager : MonoBehaviour
{
    public static ElementsManager instance;

    private Dictionary<Elements, Color> elementColors = new Dictionary<Elements, Color>();
    public Dictionary<Elements, Color> ElementColors { get { return elementColors; } }

    [SerializeField] private ElementalColorsInspector[] colorsInspector;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach (var item in colorsInspector)
        {
            elementColors.Add(item.element, item.color);
        }
    }
}

[System.Serializable]
public class ElementalColorsInspector
{
    public Elements element;
    public Color color;
}