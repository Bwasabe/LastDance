using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SlidingGauge : MonoBehaviour
{
    [SerializeField]
    private Color _canSlidingColor1;
    
    [SerializeField]
    private Color _canSlidingColor2;
    
    // [SerializeField]
    // private Color _cantSlidingColor1;
    //
    // [SerializeField]
    // private Color _cantSlidingColor2;
    
    private PlayerSliding _playerSliding;

    private CanvasGroup _canvasGroup;

    private Tweener _fadeTweener;

    private Material _material;
    private static readonly int ProgressHash = Shader.PropertyToID("_Progress");
    private static readonly int Color1 = Shader.PropertyToID("_Color1");
    private static readonly int Color2 = Shader.PropertyToID("_Color2");


    private void Awake()
    {
        _canvasGroup = GetComponentInParent<CanvasGroup>();
        _playerSliding = GameManager.Instance.Player.transform.GetComponentCache<PlayerSliding>();

        Image image = GetComponent<Image>();
        
        _material = image.material;
        image.material = _material;

        _playerSliding.OnTimerChanged += OnTimerChanged;

        _playerSliding.OnSlidingTimerMax += OnSlidingTimerMax;
        _playerSliding.OnSlidingStart += OnSlidingStart;

        _fadeTweener = _canvasGroup.DOFade(1f, 0.1f).SetAutoKill(false);
    }
    private void OnSlidingStart()
    {
        _fadeTweener.ChangeEndValue(1f, true).Restart();
    }
    private void OnSlidingTimerMax()
    {
        _fadeTweener.ChangeEndValue(0f, true).Restart();
    }
    private void OnTimerChanged(float value)
    {
        _material.SetFloat(ProgressHash, value);

    }
}
