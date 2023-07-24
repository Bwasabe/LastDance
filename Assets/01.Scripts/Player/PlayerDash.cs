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
    private GameObject _dashParticle;

    [SerializeField]
    private AnimationCurve _dashCurve;

    
    
    [SerializeField, ColorUsage(false, false)]
    private Color _dashColor = Color.blue;

    private float _timer;
    private float _dashTimer;

    private float _camFOV;
        
    private int _currentDashCount = 0;

    private Vector3 _dashAmount;

    private Rigidbody _rb;

    private Tweener _fovTweener;

    private PlayerMove _playerMove;
    private PlayerJump _playerJump;
    

    private Vignette _vignette;
    private ChromaticAberration _chromaticAberration;
    private MotionBlur _motionBlur;

    private CinemachineVirtualCamera _vCam;

    private Vector3 _prevVelocity;
    protected override void Start()
    {
        base.Start();

        _rb = transform.GetComponentCache<Rigidbody>();

        _playerMove = transform.GetComponentCache<PlayerMove>();
        _playerJump = transform.GetComponentCache<PlayerJump>();

        _vCam = PlayerCamera.Instance.VirtualCamera;

        GlobalVolume.Instance.GetProfile(out _vignette);
        GlobalVolume.Instance.GetProfile(out _chromaticAberration);
        GlobalVolume.Instance.GetProfile(out _motionBlur);

        _camFOV = _vCam.m_Lens.FieldOfView;
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

        _playerMove.IsFreeze = true;
        _playerJump.RemoveGravity = true;

        _prevVelocity = _rb.velocity;
        _prevVelocity.y = 0f;
        
        _dashParticle.gameObject.SetActive(true);
        
        if(_fovTweener is not null)
        {
            _vCam.m_Lens.FieldOfView = _camFOV;
            _fovTweener.Kill();
        }

        _fovTweener = DOTween.To(
            () => _vCam.m_Lens.FieldOfView,
            value => _vCam.m_Lens.FieldOfView = value,
            _dashFOV, _dashDuration).SetLoops(2, LoopType.Yoyo);


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
        
        _dashParticle.gameObject.SetActive(false);
        
        _dashTimer = 0f;
    }

    private void ChangeVolume()
    {
        _vignette.DOIntensity(0f, _dashDuration).SetEase(Ease.InBack);

        _motionBlur.DOIntensity(0f, _dashDuration).SetEase(Ease.InBack);
        
        _chromaticAberration.DOIntensity(0f, _dashDuration).SetEase(Ease.InBack);
    }

    
}
