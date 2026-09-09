using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommnoButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image image;
    private TextMeshProUGUI text;

    private void Awake()
    {
        image = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        image.color = Color.black;
        text.color = new Color(255, 255, 255);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = new Color(255, 255, 255);
        text.color = Color.black;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = Color.black;
        text.color = new Color(255, 255, 255);
    }           

}
