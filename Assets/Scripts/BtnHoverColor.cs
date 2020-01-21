using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BtnHoverColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Color NormalColor = Color.white;
    public Color HoverColor;

    private TMP_Text text;

    private void Start()
    {
        text = GetComponentInChildren<TMP_Text>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = HoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = NormalColor;
    }
}
