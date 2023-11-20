using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TooltipUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI modName;
    [SerializeField] private TextMeshProUGUI modDescription;

    public bool dragging = false;

    private RectTransform canvas;

    // Start is called before the first frame update
    void Start()
    {
        canvas = transform.parent.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, Input.mousePosition, null, out localPoint);
        transform.localPosition = localPoint;
    }

    public void Show(Modifier modifier)
    {
        modName.text = modifier.name;
        modDescription.text = modifier.Description;
    }
}
