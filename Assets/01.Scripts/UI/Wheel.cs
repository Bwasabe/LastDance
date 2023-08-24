using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Wheel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Color pointerColor = new Color32(255, 255, 255, 163);

    private Color originColor;

    private Image wheelImage;

    private void Awake()
    {
        wheelImage = this.GetComponent<Image>();
        originColor = wheelImage.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        wheelImage.DOColor(pointerColor, 0.4f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        wheelImage.DOColor(originColor, 0.2f);
    }
}
