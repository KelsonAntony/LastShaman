using UnityEngine;
using TMPro;

public class Tooltip : MonoBehaviour
{
    //Static so there can only ever be one tooltip
    public static Tooltip tooltipInstance { get; private set; }

    [SerializeField] private RectTransform backgroundRect;
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private RectTransform canvas;
    private RectTransform rectTransform;

    private void Awake()
    {
        //Set the instance of the tooltip (not singleton)
        tooltipInstance = this;

        rectTransform = GetComponent<RectTransform>();
        //So it doesn't appear when loading
        HideTooltip();
    }

    private void Update()
    {
        //Using the mouse position
        Vector2 anchorPosition = Input.mousePosition;

        //Fixing anchor point to keep tooltip on screen
        if(anchorPosition.x + backgroundRect.rect.width > canvas.rect.width)
        {
            //Tooltip is offscreen (right side)
            anchorPosition.x = canvas.rect.width - backgroundRect.rect.width;
        }
        if (anchorPosition.y + backgroundRect.rect.height > canvas.rect.height)
        {
            //Tooltip is offscreen (topside)
            anchorPosition.y = canvas.rect.height - backgroundRect.rect.height;
        }

        //Apply validated position to anchor position
        rectTransform.anchoredPosition = anchorPosition;

    }

    //private functions for the tooltip
    private void SetText(string a_text)
    {
        textMesh.text = a_text;
        //To make sure the mesh has updated
        textMesh.ForceMeshUpdate();

        Vector2 textSize = textMesh.GetRenderedValues(false);
        //Increasing size by paddding * 2 for both sides
        Vector2 paddingSize = new Vector2(8, 8);
        backgroundRect.sizeDelta = textSize + paddingSize;
    }

    private void ShowText(string a_tooltip)
    {
        gameObject.SetActive(true);
        SetText(a_tooltip);
    }

    private void HideText()
    {
        gameObject.SetActive(false);
    }

    //Static functions as public interface for the tooltip
    public static void ShowTooltip(string a_tooltip)
    {
        tooltipInstance.ShowText(a_tooltip);
    }

    public static void HideTooltip()
    {
        tooltipInstance.HideText();
    }
}
