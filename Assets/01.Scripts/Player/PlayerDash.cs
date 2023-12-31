using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
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
    private float _dashFOV = 80f;

    [SerializeField]
    private ParticleSystem _dashParticle;

    [SerializeField]
    private AnimationCurve _dashCurve;

    
    
    [SerializeField, ColorUsage(false, false)]
    private Color _dashColor = Color.blue;

    private float _timer;
    private float _dashTimer;

    private float _camFOV;

    private int _currentDashCount{
        get {
            return _currentDashCount1;
        }
        set {
            _currentDashCount1 = value;
            OnDash?.Invoke(_currentDashCount1);
        }
    }

    private Vector3 _dashAmount;

    private Rigidbody _rb;

    private Tweener _fovTweener;
    private Coroutine _dashParticleCoroutine;

    private PlayerMove _playerMove;
    private PlayerJump _playerJump;
    private PlayerGroundController _groundController;

    public event Action<int> OnDash;

    private Vignette _vignette;
    private ChromaticAberration _chromaticAberration;
    private MotionBlur _motionBlur;

    private CinemachineVirtualCamera _vCam;

    private Vector3 _prevVelocity;
    private Color _originColor;
    private float _originVignetteIntensity;
    private float _originBlurIntensity;
    private float _originChromaticIntensity;
    private int _currentDashCount1;
    
    protected override void Start()
    {
        base.Start();

        _rb = transform.GetComponentCache<Rigidbody>();

        _groundController = transform.GetComponentCache<PlayerGroundController>();
        _playerMove = transform.GetComponentCache<PlayerMove>();
        _playerJump = transform.GetComponentCache<PlayerJump>();

        _vCam = PlayerCamera.Instance.VirtualCamera;

        GlobalVolume.Instance.GetProfile(out _vignette);
        GlobalVolume.Instance.GetProfile(out _chromaticAberration);
        GlobalVolume.Instance.GetProfile(out _motionBlur);

        _camFOV = _vCam.m_Lens.FieldOfView;
        // _camFOV = 70;
    }
    

    private void Update()
    {
        Cooldown();

        if (Input.GetKeyDown(KeyCode.LeftShift) && !_groundController.IsGround)
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

    private IEnumerator DisableParticle()
    {
        yield return Yields.WaitForSeconds(_dashDuration);
        _dashParticle.Stop();
    }

    private void Dash()
    {
        if(_currentDashCount <= 0) return;
        if(_playerStateController.HasState(Player_State.Dash))return;

        _playerMove.IsFreeze = true;
        _playerJump.RemoveGravity = true;

        _prevVelocity = _rb.velocity;
        _prevVelocity.y = 0f;

        _dashParticle.Play(true);
        
        if(_fovTweener is not null)
        {
            _vCam.m_Lens.FieldOfView = _camFOV;
            
            _fovTweener.Kill();
            if(_dashParticleCoroutine != null)
             StopCoroutine(_dashParticleCoroutine);
        }

        _dashParticleCoroutine = StartCoroutine(DisableParticle());

        _fovTweener = DOTween.To(
            () => _vCam.m_Lens.FieldOfView,
            value => _vCam.m_Lens.FieldOfView = value,
            _dashFOV, _dashDuration).SetLoops(2, LoopType.Yoyo);

        _originColor = _vignette.color.value;
        _originVignetteIntensity = _vignette.intensity.value;
        _originBlurIntensity = _motionBlur.intensity.value;
        _originChromaticIntensity = _chromaticAberration.intensity.value;

        _vignette.color.Override(_dashColor);
        _vignette.intensity.Override(0.4f);
        
        _motionBlur.intensity.Override(1f);
        
        _chromaticAberration.intensity.Override(1f);
        
        ChangeVolume();

        
        _dashTimer = 0f;
        _currentDashCount--;
        
        _rb.SetVelocityY(0f);

        _dashAmount = Define.MainCam.transform.forward * _dashDistance;

        _playerStateController.AddState(Player_State.Invincible);
        _playerStateController.AddState(Player_State.Dash);
    }

    private void DashAcceleration()
    {
        if (!_playerStateController.HasState(Player_State.Dash)) return;

        _dashTimer += Time.deltaTime / _dashDuration;

        if(_dashTimer >= 1)
        {
            DashEnd();
        }
        
        _rb.velocity = Vector3.Lerp(_dashAmount / _dashDuration, _prevVelocity, _dashCurve.Evaluate(_dashTimer));
    }

    private void DashEnd()
    {
        _playerStateController.RemoveState(Player_State.Invincible);
        _playerStateController.RemoveState(Player_State.Dash);

        _playerMove.IsFreeze = false;
        _playerJump.RemoveGravity = false;
        
        _dashTimer = 0f;
    }

    private void ChangeVolume()
    {
        _vignette.intensity.DOFloat(0f, _dashDuration).SetEase(Ease.InBack).OnComplete(ReturnToOrigin);

        _motionBlur.intensity.DOFloat(0f, _dashDuration).SetEase(Ease.InBack);
        
        _chromaticAberration.intensity.DOFloat(0f, _dashDuration).SetEase(Ease.InBack);
    }
    private void ReturnToOrigin()
    {
        _vignette.color.Override(_originColor);
        _vignette.intensity.Override(_originVignetteIntensity);
        
        _motionBlur.intensity.Override(_originBlurIntensity);
        
        _chromaticAberration.intensity.Override(_originChromaticIntensity);
    }


}
