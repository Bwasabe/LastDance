using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerSlow : PlayerComponentBase
{
    [SerializeField]
    private float _timeScale = 0.2f;
    [SerializeField]
    private float _slowDuration = 5f;
    [SerializeField]
    private float _slowChargeDuration = 10f;

    [SerializeField]
    private float _animDuration = 0.25f;
    
    [SerializeField]
    private GameObject _slowParticle;
    
    [SerializeField, ColorUsage(false, false)]
    private Color _slowColor = Color.blue;
    
    [SerializeField, ColorUsage(false, false)]
    private Color _slowVignetteColor = Color.green;

    private float _gauge;
    private float _originScale;

    public event Action<float> OnGaugeChanged;

    public event Action OnGaugeAchieveMax;
    public event Action OnStartSlow;

    private Vignette _vignette;
    private MotionBlur _motionBlur;
    private ColorAdjustments _colorAdjustments;
    private ChromaticAberration _chromaticAberration;

    private Color _originColor;
    private Color _originVignetteColor;
    private float _originVignetteIntensity;
    private float _originBlurIntensity;
    private float _originChromaticIntensity;
    private float _originContrast;

    protected override void Start()
    {
        base.Start();

        _gauge = 1f;

        GlobalVolume.Instance.GetProfile(out _vignette);
        GlobalVolume.Instance.GetProfile(out _motionBlur);
        GlobalVolume.Instance.GetProfile(out _colorAdjustments);
        GlobalVolume.Instance.GetProfile(out _chromaticAberration);
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(_playerStateController.HasState(Player_State.Slow))
            {
                EndSlow();                
            }
            else
            {
                StartSlow();
            }
        }
        
        // Slow를 쓰는 동안 깎인다
        if(_playerStateController.HasState(Player_State.Slow))
        {
            if(_gauge < 0)
            {
                _gauge = 0f;
                EndSlow();
                return;
            }

            _gauge -= Time.unscaledDeltaTime * 1 / _slowDuration;
        }
        else
        {
            if(_gauge >= 1f)
            {
                _gauge = 1f;
                return;
            }

            _gauge += Time.unscaledDeltaTime * 1/_slowChargeDuration;
            
            if(_gauge >= 1f)
                OnGaugeAchieveMax?.Invoke();
                
        }
        
        OnGaugeChanged?.Invoke(_gauge);
    }

    private void StartSlow()
    {
        _playerStateController.AddState(Player_State.Slow);
        
        // TimeScale을 Smooth하게 만들자
        _originScale = Time.timeScale;
        
        OnStartSlow?.Invoke();
        
        ChangeTimeScale(_timeScale);
        
        OnParticle();

        StartChangeVolume();
    }

    private void StartChangeVolume()
    {
        _originVignetteColor = _vignette.color.value;

        _originVignetteIntensity = _vignette.intensity.value;
        _originBlurIntensity = _motionBlur.intensity.value;
        _originChromaticIntensity = _chromaticAberration.intensity.value;
        _originColor = _colorAdjustments.colorFilter.value;
        _originContrast = _colorAdjustments.contrast.value;

        _vignette.color.Override(_slowVignetteColor);
        
        _colorAdjustments.contrast.DOFloat(50f, _animDuration);
        _vignette.intensity.DOFloat(0.5f, _animDuration);//.OnComplete(OnParticle);
        _motionBlur.intensity.DOFloat(1f, _animDuration);
        _chromaticAberration.intensity.DOFloat(1f, _animDuration);
        _colorAdjustments.colorFilter.DOColor(_slowColor, _animDuration);
    }
    
    private void EndChangeVolume()
    {
        _colorAdjustments.contrast.DOFloat(_originContrast, _animDuration);
        _motionBlur.intensity.DOFloat(_originBlurIntensity, _animDuration);
        _vignette.intensity.DOFloat(_originVignetteIntensity, _animDuration).OnComplete(ReturnVolume);
        _chromaticAberration.intensity.DOFloat(_originChromaticIntensity, _animDuration);
        _colorAdjustments.colorFilter.DOColor(_originColor, _animDuration);
    }
    private void ReturnVolume()
    {
        _vignette.color.Override(_originVignetteColor);
        
    }

    private void OnParticle()
    {
        _slowParticle.SetActive(true);
    }

    private void EndSlow()
    {
        _playerStateController.RemoveState(Player_State.Slow);
        
        ChangeTimeScale(_originScale);

        _slowParticle.SetActive(false);
        EndChangeVolume();
    }

    private void ChangeTimeScale(float endValue)
    {
        DOTween.To(
            () => Time.timeScale,
            value => Time.timeScale = value,
            endValue, _animDuration
        );
    }
}
