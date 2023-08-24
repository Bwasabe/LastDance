using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public enum Dir
{
    None = -1,
    Top = 0,
    Bottom,
    Left,
    Right
}

public class PlayerItem : PlayerComponentBase
{
    [SerializeField]
    private float radius = 190f;

    [Header("Top")]
    [SerializeField]
    private float minTopRadius = 45.9559f;
    [SerializeField]
    private float maxTopRadius = 134.0497f;
    [Header("Bottom")]
    [SerializeField]
    private float minBottomRadius = 225.7034f;
    [SerializeField]
    private float maxBottomRadius = 313.9654f;
    [Header("Left")]
    [SerializeField]
    private float minLeftRadius = 135.903f;
    [SerializeField]
    private float maxLeftRadius = 224.1244f;
    [Header("Right")]
    [SerializeField]
    private float minRightRadius = 44.2663f;
    [SerializeField]
    private float maxRightRadius = 315.8902f;

    [Header("Wheel UI(Top - Bottom - Left - Right)")]
    [SerializeField]
    private Image[] wheels;

    private Dir selectIdx = Dir.None;
    private Dir prevIdx = Dir.None;

    private void Update()
    {
        CheckRadius();
    }

    private void CheckRadius()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0f);
        Vector3 mousePosition = Input.mousePosition;
        float distance = Vector3.Distance(mousePosition, screenCenter);

        if (distance > radius)
        {
            Vector3 toMouse = mousePosition - screenCenter;

            float angle = Mathf.Atan2(toMouse.y, toMouse.x) * Mathf.Rad2Deg;

            if (angle < 0)
            {
                angle += 360;
            }

            CheckItem(angle);
        }
        else
        {
            Debug.Log("Select Sword");
        }
    }

    private void CheckItem(float angle)
    {
        if(angle >= minTopRadius && angle <= maxTopRadius)
        {
            Debug.Log("Top");
            if(selectIdx != Dir.Top)
            {
                prevIdx = selectIdx;
                selectIdx = Dir.Top;

                ChangeScaleWheel();
            }
        }
        else if(angle >= minBottomRadius && angle <= maxBottomRadius)
        {
            Debug.Log("Bottom");

            if (selectIdx != Dir.Bottom)
            {
                prevIdx = selectIdx;
                selectIdx = Dir.Bottom;

                ChangeScaleWheel();
            }
        }
        else if (angle >= minLeftRadius && angle <= maxLeftRadius)
        {
            Debug.Log("Left");

            if (selectIdx != Dir.Left)
            {
                prevIdx = selectIdx;
                selectIdx = Dir.Left;

                ChangeScaleWheel();
            }
        }
        else if((angle >= 0 && angle <= minRightRadius) || (angle >= maxRightRadius && angle <= 360))
        {
            Debug.Log("Right");

            if (selectIdx != Dir.Right)
            {
                prevIdx = selectIdx;
                selectIdx = Dir.Right;

                ChangeScaleWheel();
            }
        }
    }

    private void ChangeScaleWheel()
    {
        if(prevIdx != Dir.None)
            wheels[(int)prevIdx].transform.DOScale(1f, 0.5f);

        wheels[(int)selectIdx].transform.DOScale(1.2f, 0.5f);
    }
}
