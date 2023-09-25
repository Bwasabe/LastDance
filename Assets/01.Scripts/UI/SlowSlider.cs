using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SlowSlider : MonoBehaviour
{
    private PlayerSlow _playerSlow;

    private Material _material;
    private static readonly int Progress1 = Shader.PropertyToID("_Progress");
    
    private Image _image;

    private CanvasGroup _canvasGroup;
    
    private Tweener _fadeTweener;


    private void Awake()
    {
        _canvasGroup = GetComponentInParent<CanvasGroup>();
        
        _playerSlow = GameManager.Instance.Player.transform.GetComponentCache<PlayerSlow>();
        _playerSlow.OnGaugeChanged += OnGaugeChanged;
        _playerSlow.OnGaugeAchieveMax += OnGaugeAchieveMax;
        _playerSlow.OnStartSlow += OnStartSlow;
        
        Image image = GetComponent<Image>();
        
        _material = image.material;
        image.material = _material;
        
        _fadeTweener = _canvasGroup.DOFade(0f, 0.1f).SetAutoKill(false);
    }
    private void OnStartSlow()
    {
        _fadeTweener.ChangeEndValue(1f, true).Restart();
    }
    private void OnGaugeAchieveMax()
    {
        _fadeTweener.ChangeEndValue(0f, true).Restart();
    }
    
    private void OnGaugeChanged(float gauge)
    {
        _material.SetFloat(Progress1, gauge);
    }
}
