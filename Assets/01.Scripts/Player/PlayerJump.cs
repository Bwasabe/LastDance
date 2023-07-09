using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class PlayerJump : PlayerComponentBase
{
    private enum Jump_State
    {
        None = 0,
        JumpUp = 1,
        JumpDown = 2,
    }
    private Jump_State _jumpState = Jump_State.None;


    [SerializeField]
    private float _jumpForce = 8f;
    [SerializeField]
    private float _gravityScale = 3f;

    [SerializeField]
    private int _jumpMaxCount = 2;

    [SerializeField]
    private LayerMask _groundLayer;

    private Rigidbody _rb;

    private CapsuleCollider _capsuleCollider;

    private int _currentJumpCount;

    private const float _tolerance = 0.01f;
    private const float _radiusTolerance = 0.02f;

    protected override void Start()
    {
        base.Start();

        _rb = transform.GetComponentCache<Rigidbody>();
        _capsuleCollider = transform.GetComponentCache<CapsuleCollider>();
    }

    private void Update()
    {
        CheckJumpState();
        
        GroundAction();
        
        Jump();
    }

    private void FixedUpdate()
    {
        // 중력 적용
        if(!_playerStateController.HasState(Player_State.Dash))
            _rb.AddForce(Physics.gravity.y * _gravityScale * TimeManager.PlayerTimeScale * Vector3.up, ForceMode.Force);
    }

    private void LateUpdate()
    {
        AdjoinGround();
    }

    private void CheckJumpState()
    {
        if(_rb.velocity.y > 0)
        {
            _jumpState = Jump_State.JumpUp;
        }
        else
        {
            _jumpState = Jump_State.JumpDown;
        }
    }

    private void GroundAction()
    {
        // 땅에 닿아있는 상태에서
        if(IsGround())
        {
            // 내려오는 중이면
            if(_jumpState == Jump_State.JumpDown)
            {
                _currentJumpCount = 0;

                Vector3 velocity = _rb.velocity;
                velocity.y = 0f;
                _rb.velocity = velocity;
                
                _jumpState = Jump_State.None;
                _playerStateController.RemoveState(Player_State.Jump);
            }
        }
    }

    private void Jump()
    {
        // 점프키를 눌렀을 경우
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(_currentJumpCount < _jumpMaxCount)
            {
                bool isFirstJump = _currentJumpCount == 0;
                
                // 처음 하는 점프에서 땅이 아닐경우 한번만 점프하도록
                if(isFirstJump)
                {
                    if(IsGround())
                        _currentJumpCount++;
                    else
                        _currentJumpCount += 2;
                }
                else
                    _currentJumpCount++;
                
                _playerStateController.AddState(Player_State.Jump);
                
                Vector3 velocity = _rb.velocity;
                velocity.y = Mathf.Sqrt(_jumpForce * -2.0f * Physics.gravity.y) * TimeManager.PlayerTimeScale;
                _rb.velocity = velocity;
            }
        }

        
    }

    private Vector3 GetGroundPos()
    {
        Vector3 checkPos = _capsuleCollider.transform.position + _capsuleCollider.center;
        float halfHeight = _capsuleCollider.height * 0.5f - _capsuleCollider.radius;

        checkPos.y -= halfHeight + _tolerance + _radiusTolerance;

        return checkPos;
    }

    private bool IsGround()
    {
        Vector3 checkPos = GetGroundPos(); 

        return Physics.CheckSphere(checkPos, _capsuleCollider.radius - _radiusTolerance, _groundLayer);
    }

    private void OnDrawGizmos()
    {
        try
        {
            Gizmos.color = Color.red;

            Vector3 checkPos = GetGroundPos();

            Gizmos.DrawSphere(checkPos, _capsuleCollider.radius - _radiusTolerance);
        }
        catch {}

    }

    /// <summary>
    /// 플레이어가 내리막길 같은 곳을 갈 때 땅에 붙어서 갈 수 있도록 해주는 함수
    /// </summary>
    private void AdjoinGround()
    {
        if(_playerStateController.HasState(Player_State.Jump)) return;

        if(_jumpState is not Jump_State.None) return;

        if(Physics.Raycast(_capsuleCollider.transform.position, Vector3.down, out RaycastHit hit, 0.5f, _groundLayer))
        {
            Vector3 checkPos = GetGroundPos();

            float distance = checkPos.y - hit.point.y;

            _rb.MovePosition(Vector3.down * distance);
        }
    }
}
