using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ScaleUI : MonoBehaviour
{
    private void Start()
    {
        transform.DOScale(Vector3.one * 1.2f, 0.3f).SetLoops(-1, LoopType.Yoyo);
    }
}
