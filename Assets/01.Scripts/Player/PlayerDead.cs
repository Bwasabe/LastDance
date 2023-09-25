using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerDead : PlayerComponentBase
{
    [SerializeField, ColorUsage(false, false)]
    private Color _vignetteColor;
    [SerializeField]
    private float _vignetteDuration = 0.2f;
    
    
    [SerializeField, ColorUsage(false, false)]
    private Color _colorFilterColor;
    [SerializeField]
    private float _colorFilterDuration = 1f;
    
    
    private Animator _animator;
    private static readonly int DeadHash = Animator.StringToHash("Dead");

    private Vignette _vignette;
    private ColorAdjustments _colorAdjustments;

    private void Awake()
    {
        _animator = transform.GetComponentCache<Animator>();
    }

    protected override void Start()
    {
        base.Start();
        
        GlobalVolume.Instance.GetProfile(out _vignette);
        GlobalVolume.Instance.GetProfile(out _colorAdjustments);
    }

    public void Hit()
    {
        if(_playerStateController.HasState(Player_State.Dead)) return;
        
        Time.timeScale = 0.2f;

        _vignette.intensity.DOFloat(0.5f, _vignetteDuration);
        _vignette.color.Override(_vignetteColor);


        _colorAdjustments.colorFilter.DOColor(_colorFilterColor, _colorFilterDuration);
        
        _animator.SetTrigger(DeadHash);
        
        _playerStateController.AddState(Player_State.Dead);
        
        PlayerComponentBase[] componentBases = transform.root.GetComponentsInChildren<PlayerComponentBase>();
        
        foreach (PlayerComponentBase playerComponentBase in componentBases)
        {
            playerComponentBase.enabled = false;
        }
    }
}
