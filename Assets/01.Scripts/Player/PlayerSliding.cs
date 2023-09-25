using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerSliding : PlayerComponentBase
{
    [SerializeField]
    private AnimationCurve _slidingCurve;

    [SerializeField]
    private float _impulseForce = 5f;

    [SerializeField]
    private float _fillMultiplier = 0.5f;

    [SerializeField]
    private float _minimumTimer = 0.2f;

    [SerializeField]
    private float _slidingForce = 70f;
    [SerializeField]
    private float _slopeForce = 1000f;
    [SerializeField]
    private float _slidingDuration = 1f;

    private Rigidbody _rb;
    private PlayerGroundController _groundController;
    private PlayerMove _playerMove;

    public event Action<float> OnTimerChanged;
    public event Action OnSlidingTimerMax;
    
    public event Action OnSlidingStart;
    
    private float _timer;
    
    private float _gravity;
    

    protected override void Start()
    {
        base.Start();
        _rb = transform.GetComponentCache<Rigidbody>();
        _playerMove = transform.GetComponentCache<PlayerMove>();
        _groundController = transform.GetComponentCache<PlayerGroundController>();

        _timer = _slidingDuration;
        OnTimerChanged?.Invoke(_timer / _slidingDuration);
        OnSlidingTimerMax?.Invoke();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift) && _groundController.IsGround && Define.GetInput() != Vector3.zero)
        {
            StartSliding();
        }

        if(_playerStateController.HasState(Player_State.Sliding))
        {
            // Shift를 때거나, timer가 다 되면서 Slope가 아니거나, 땅에서 떨어진 경우
            if(Input.GetKeyUp(KeyCode.LeftShift) || _timer <= 0f && !_groundController.IsOnSlope || !_groundController.IsGround)
            {
                EndSliding();
            }

            _timer -= Time.deltaTime;
            
        }
        else
        {
            if(_timer >= _slidingDuration) return;

            _timer += Time.deltaTime * _fillMultiplier;

            if(_timer >= _slidingDuration)
            {
                _timer = _slidingDuration;
                
                OnSlidingTimerMax?.Invoke();
            }
        }
        OnTimerChanged?.Invoke(_timer / _slidingDuration);
    }

    private void FixedUpdate()
    {
        if(_playerStateController.HasState(Player_State.Jump)) return;
        
        if(_playerStateController.HasState(Player_State.Sliding))
        {
            if(_groundController.IsOnSlope)
            {
                _rb.AddForce(_slopeForce * Vector3.down, ForceMode.Force);
            }
            else
            {
                _rb.AddForce(_slidingCurve.Evaluate(1 - _timer / _slidingDuration) * _slidingForce * _playerMove.MoveDir, ForceMode.Force);
            }
        }
    }


    private void StartSliding()
    {
        OnSlidingStart?.Invoke();
        
        _gravity = _rb.velocity.y;

        _rb.AddForce(_playerMove.MoveDir * _impulseForce, ForceMode.Impulse);
        _playerMove.IsFreeze = true;
        
        _playerStateController.AddState(Player_State.Sliding);
    }

    private void EndSliding()
    {
        // OnCantSliding?.Invoke();
        
        
        _playerMove.IsFreeze = false;
        
        if(!_playerStateController.HasState(Player_State.Jump))
            _rb.SetVelocityY(_gravity);

        _playerStateController.RemoveState(Player_State.Sliding);
    }


}
