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

    public bool RemoveGravity{ get; set; } = false;

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

    public int CurrentJumpCount{ get; set; }

    private const float TOLERANCE = 0.01f;
    private const float RADIUS_TOLERANCE = 0.02f;

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
        // 점프키를 눌렀을 경우
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(CurrentJumpCount < _jumpMaxCount)
            {
                bool isFirstJump = CurrentJumpCount == 0;

                // 처음 하는 점프에서 땅이 아닐경우 한번만 점프하도록
                if(isFirstJump)
                {
                    if(IsGround())
                        CurrentJumpCount++;
                    else
                        CurrentJumpCount += 2;
                }
                else
                    CurrentJumpCount++;

                Jump();
            }
        }

    }

    private void FixedUpdate()
    {
        if(RemoveGravity) return;

        // 중력 적용
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
                CurrentJumpCount = 0;

                _rb.SetVelocityY(0f);

                _jumpState = Jump_State.None;
                _playerStateController.RemoveState(Player_State.Jump);
            }
        }
    }

    public void Jump()
    {
        _playerStateController.AddState(Player_State.Jump);

        _rb.SetVelocityY(Mathf.Sqrt(_jumpForce * -2.0f * Physics.gravity.y) * TimeManager.PlayerTimeScale);
    }

    private Vector3 GetGroundPos()
    {
        Vector3 checkPos = _capsuleCollider.transform.position + _capsuleCollider.center;
        float halfHeight = _capsuleCollider.height * 0.5f - _capsuleCollider.radius;

        checkPos.y -= halfHeight + TOLERANCE + RADIUS_TOLERANCE;

        return checkPos;
    }

    public bool IsGround()
    {
        Vector3 checkPos = GetGroundPos();

        return Physics.CheckSphere(checkPos, _capsuleCollider.radius - RADIUS_TOLERANCE, _groundLayer);
    }

    private void OnDrawGizmos()
    {
        try
        {
            Gizmos.color = Color.red;

            Vector3 checkPos = GetGroundPos();

            Gizmos.DrawSphere(checkPos, _capsuleCollider.radius - RADIUS_TOLERANCE);
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
