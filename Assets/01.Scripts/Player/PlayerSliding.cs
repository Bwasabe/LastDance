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
    private float _slidingForce = 70f;
    [SerializeField]
    private float _slopeForce = 1000f;
    [SerializeField]
    private float _slidingDuration = 1f;

    private Rigidbody _rb;
    private PlayerGroundController _groundController;
    private PlayerMove _playerMove;
    
    
    private float _timer;
    
    private float _gravity;
    

    protected override void Start()
    {
        base.Start();
        _rb = transform.GetComponentCache<Rigidbody>();
        _playerMove = transform.GetComponentCache<PlayerMove>();
        _groundController = transform.GetComponentCache<PlayerGroundController>();
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
            if(Input.GetKeyUp(KeyCode.LeftShift) || _timer >= _slidingDuration && !_groundController.IsOnSlope || !_groundController.IsGround)
            {
                EndSliding();
            }

            _timer += Time.deltaTime;
        }
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
                _rb.AddForce(_slidingCurve.Evaluate(_timer / _slidingDuration) * _slidingForce * _playerMove.MoveDir, ForceMode.Force);
            }
        }
    }


    private void StartSliding()
    {
        _gravity = _rb.velocity.y;

        _rb.AddForce(_playerMove.MoveDir * _impulseForce, ForceMode.Impulse);
        _playerMove.IsFreeze = true;
        _timer = 0f;
        
        _playerStateController.AddState(Player_State.Sliding);
    }

    private void EndSliding()
    {
        _playerMove.IsFreeze = false;
        
        if(!_playerStateController.HasState(Player_State.Jump))
            _rb.SetVelocityY(_gravity);

        _playerStateController.RemoveState(Player_State.Sliding);
    }


}
