using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class PlayerJump : PlayerComponentBase
{
    private enum CheckState
    {
        None,
        UpGround,
        DownGround,
    }
    public bool RemoveGravity{ get; set; } = false;

    [SerializeField]
    private float _jumpForce = 8f;
    [SerializeField]
    private float _gravityScale = 3f;

    [SerializeField]
    private int _jumpMaxCount = 2;


    private Rigidbody _rb;

    private PlayerGroundController _groundController;

    private int _currentJumpCount;

    private float _prevVelocityY;

    private CheckState _checkState = CheckState.None;

    protected override void Start()
    {
        base.Start();

        _rb = transform.GetComponentCache<Rigidbody>();
        _groundController = transform.GetComponentCache<PlayerGroundController>();
    }

    private void Update()
    {
        OnGUIManager.Instance.SetGUI("CheckState", _checkState);
        if(_playerStateController.HasState(Player_State.Jump))
            GroundCheck();

        // 점프키를 눌렀을 경우
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(_currentJumpCount < _jumpMaxCount)
            {
                bool isFirstJump = _currentJumpCount == 0;

                // 처음 하는 점프에서 땅이 아닐경우 한번만 점프하도록
                if(isFirstJump)
                {
                    if(_groundController.IsGround)
                        _currentJumpCount++;
                    else
                        _currentJumpCount += 2;
                }
                else
                    _currentJumpCount++;

                Jump();
            }
        }
    }
    private void GroundCheck()
    {
        // UpGround에서 IsGround가 False가 되었다
        if(_checkState == CheckState.UpGround && !_groundController.IsGround)
        {
            _checkState = CheckState.DownGround;
        }
        // DownGround상태에서 땅에 닿았으면
        else if(_checkState == CheckState.DownGround && _groundController.IsGround)
        {
            OnGround();
        }
    }

    private void FixedUpdate()
    {
        if(RemoveGravity) return;

        // 중력 적용
        _rb.AddForce(Physics.gravity.y * _gravityScale * TimeManager.PlayerTimeScale * Vector3.up, ForceMode.Force);
    }

    
    private void OnGround()
    {
        _currentJumpCount = 0;

        _rb.SetVelocityY(0f);

        _checkState = CheckState.None;

        _playerStateController.RemoveState(Player_State.Jump);
    }

    public void Jump()
    {
        _playerStateController.AddState(Player_State.Jump);
        _checkState = CheckState.UpGround;

        _rb.SetVelocityY(Mathf.Sqrt(_jumpForce * -2.0f * Physics.gravity.y) * TimeManager.PlayerTimeScale);
    }

}
