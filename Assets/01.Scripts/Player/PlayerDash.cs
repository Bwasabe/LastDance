using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private float _timer;
    private float _dashTimer;
        
    private int _currentDashCount = 0;

    private Vector3 _dashAmount;

    private Rigidbody _rb;

    protected override void Start()
    {
        base.Start();

        _rb = transform.GetComponentCache<Rigidbody>();

        _dashDuration = 1 / _dashDuration;
        
        _playerStateController.AddState(Player_State.Dash);
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
            _playerStateController.RemoveState(Player_State.Invincible);
            _playerStateController.RemoveState(Player_State.Dash);

            _dashTimer = 0f;
        }
        
        _rb.velocity = Vector3.Lerp(_dashAmount * _dashDuration, Vector3.zero, _dashCurve.Evaluate(_dashTimer));
    }

    
}
