using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerDash : PlayerComponentBase
{
    [SerializeField]
    private int _maxDashCount = 2;

    [SerializeField]
    private float _dashCooldown = 2f;
    [SerializeField]
    private float _dashDistance = 8f;
    [SerializeField]
    private float _dashDuration = 0.2f;

    [SerializeField]
    private AnimationCurve _dashCurve;
    
    [SerializeField, ColorUsage(false, false)]
    private Color _dashColor = Color.blue;

    private float _timer;
    private float _dashTimer;
        
    private int _currentDashCount = 0;

    private Vector3 _dashAmount;

    private Rigidbody _rb;
    
    [SerializeField]
    private float VolumeDuration = 0.1f;

    private Vignette _vignette;
    private LiftGammaGain _liftGammaGain;
    private LensDistortion _lensDistortion;
    private ChromaticAberration _chromaticAberration;

    protected override void Start()
    {
        base.Start();

        _rb = transform.GetComponentCache<Rigidbody>();

        _dashDuration = 1 / _dashDuration;

        GlobalVolume.Instance.GetProfile(out _vignette);
        GlobalVolume.Instance.GetProfile(out _liftGammaGain);
        GlobalVolume.Instance.GetProfile(out _lensDistortion);
        GlobalVolume.Instance.GetProfile(out _chromaticAberration);

        Debug.Log(_liftGammaGain.gamma.value);
    }

    private void Update()
    {
        Cooldown();

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Dash();
        }

        DashAcceleration();
    }

    private void Cooldown()
    {
        if (_currentDashCount < _maxDashCount)
        {
            _timer += Time.deltaTime * TimeManager.PlayerTimeScale;
            if (_timer >= _dashCooldown)
            {
                _timer = 0f;
                _currentDashCount++;
            }
        }
    }

    private void Dash()
    {
        if(_currentDashCount <= 0) return;
        if(_playerStateController.HasState(Player_State.Dash))return;

        _vignette.color.Override(_dashColor);

        _vignette.DOIntensity(0.8f, VolumeDuration);
        
        
        _lensDistortion.DOIntensity(-0.5f, VolumeDuration);
        
        _chromaticAberration.DOIntensity(1f, VolumeDuration);

        
        _liftGammaGain.DOLift(new(1f, 1f, 1f, 0.2f), VolumeDuration);
        
        _liftGammaGain.DOGamma(new(0.83f, 0.91f, 1f, -0.73f), VolumeDuration);

        _dashTimer = 0f;
        _currentDashCount--;
        
        Vector3 velocity = _rb.velocity;
        velocity.y = 0f;
        _rb.velocity = velocity;

        _dashAmount = Define.MainCam.transform.forward * _dashDistance;

        _playerStateController.AddState(Player_State.Invincible);
        _playerStateController.AddState(Player_State.Dash);
    }

    private void DashAcceleration()
    {
        if (!_playerStateController.HasState(Player_State.Dash)) return;

        _dashTimer += Time.deltaTime * _dashDuration;

        if(_dashTimer >= 1)
        {
            DashEnd();
        }
        
        _rb.velocity = Vector3.Lerp(_dashAmount * _dashDuration, Vector3.zero, _dashCurve.Evaluate(_dashTimer));
    }

    private void DashEnd()
    {
        _playerStateController.RemoveState(Player_State.Invincible);
        _playerStateController.RemoveState(Player_State.Dash);

        _dashTimer = 0f;
        
        _vignette.DOIntensity(0f, VolumeDuration);
        
        _lensDistortion.DOIntensity(0f, VolumeDuration);
        
        _chromaticAberration.DOIntensity(0f, VolumeDuration);
        
        _liftGammaGain.DOLift(Vector3.one, VolumeDuration);
        
        _liftGammaGain.DOGamma(Vector3.one, VolumeDuration);
    }

    
}
