using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerWallRunning : PlayerComponentBase
{
    private enum WallJumpState
    {
        None,
        ReadyToJump,
        Jumping,
    }

    [SerializeField]
    private LayerMask _groundLayer;

    [SerializeField]
    private float _wallRunSpeed = 8f;
    [SerializeField]
    private float _wallCheckDistance = 0.5f;

    [SerializeField]
    private float _lerpSmooth = 12f;
    [SerializeField]
    private float _wallRotationDuration = 0.2f;
    [SerializeField]
    private float _wallJumpDuration = 0.2f;

    [SerializeField]
    private float _camRotationMultiplier = 30f;

    private readonly List<RaycastHit> _results = new(8);

    private WallJumpState _wallJumpState;

    private Rigidbody _rb;
    private CapsuleCollider _capsuleCollider;
    private Tweener _camRotateTweener;

    private PlayerJump _playerJump;
    private PlayerGroundController _groundController;
    private CameraMovement _cameraMovement;

    private Vector3 _moveAmount;

    private float _dampVelocity;

    protected override void Start()
    {
        base.Start();

        _rb = transform.GetComponentCache<Rigidbody>();
        _capsuleCollider = transform.GetComponentCache<CapsuleCollider>();
        
        _playerJump = transform.GetComponentCache<PlayerJump>();
        _groundController = transform.GetComponentCache<PlayerGroundController>();
        
        _cameraMovement = transform.GetComponentCache<CameraMovement>();
    }

    // 벽을 체크해서 달라붙음
    private void CheckWallState()
    {
        _results.Clear();
        Vector3 center = transform.position + _capsuleCollider.center;

        const int rayCount = 8;
        const float oneStep = 360f / rayCount;

        float startAngle = -_cameraMovement.transform.eulerAngles.y + 90f;

        for (int i = 0; i < rayCount / 2 + 1; ++i)
        {
            {
                float angle = startAngle + oneStep * i;
                Vector3 direction = Define.AngleToVector3(angle);
                Ray ray = new(center, direction);

                if(Physics.Raycast(ray, out RaycastHit hit, _wallCheckDistance, _groundLayer))
                {
                    _results.Add(hit);
                }

                Debug.DrawRay(ray.origin, ray.direction * _wallCheckDistance, Color.green);
            }

            {
                float angle = startAngle - oneStep * i;
                Vector3 direction = Define.AngleToVector3(angle);
                Ray ray = new(center, direction);

                if(Physics.Raycast(ray, out RaycastHit hit, _wallCheckDistance, _groundLayer))
                {
                    _results.Add(hit);
                }

                Debug.DrawRay(ray.origin, ray.direction * _wallCheckDistance, Color.green);
            }
        }

    }

    private void Update()
    {
        OnGUIManager.Instance.SetGUI("Velocity", _rb.velocity);

        // 땅에 닿았을 때 처리
        if(_groundController.GroundValue)
        {
            if(_playerStateController.HasState(Player_State.WallRunning))
                WallRunningEnd();

            _wallJumpState = WallJumpState.None;
            return;
        }
        
        
        if(_wallJumpState == WallJumpState.None)
        {
            OnStateNone();
        }
        else if(_wallJumpState == WallJumpState.ReadyToJump)
        {
            OnStateReadyToJump();
        }
        else if(_wallJumpState == WallJumpState.Jumping)
        {
            OnStateJumping();
        }
    }
    private void OnStateJumping()
    {
        if(_rb.velocity.y < 0)
            _wallJumpState = WallJumpState.None;
    }
    private void OnStateReadyToJump()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            WallJump();
        }
    }
    private void OnStateNone()
    {

        CheckWallState();

        if(_results.Count == 0)
        {
            if(_playerStateController.HasState(Player_State.WallRunning))
            {
                WallRunningEnd();
                StartCoroutine(ReadyToJump());
            }

            return;
        }

        if(Input.GetKey(KeyCode.Space))
        {
            if(!_playerStateController.HasState(Player_State.WallRunning))
                WallRunningStart();

            WallRunning();
        }
        else if(_playerStateController.HasState(Player_State.WallRunning))
        {
            WallRunningEnd();
            StartCoroutine(ReadyToJump());
        }
    }

    private IEnumerator ReadyToJump()
    {
        _wallJumpState = WallJumpState.ReadyToJump;

        yield return Yields.WaitForSeconds(_wallJumpDuration);

        if(_wallJumpState == WallJumpState.ReadyToJump)
            _wallJumpState = WallJumpState.None;
    }


    private void WallJump()
    {
        // Debug.Log("WallJump");
        // IEnumerator Task()
        // {
        //     yield return Yields.WaitForEndOfFrame;
        //     _playerJump.CurrentJumpCount = 0;
        // }

        _playerJump.Jump();

        // StartCoroutine(Task());
        _wallJumpState = WallJumpState.Jumping;
        WallRunningEnd();
    }


    private void WallRunningStart()
    {
        if(_camRotateTweener != null)
        {
            _camRotateTweener.Kill();
        }

        // _playerJump.CurrentJumpCount = 0;
        
        _rb.SetVelocityY(0f);

        _playerJump.RemoveGravity = true;
        _playerStateController.AddState(Player_State.WallRunning);

    }
    
    private void WallRunning()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 wallForwardCross = Vector3.Cross(-_cameraMovement.transform.forward, _results[0].normal);
        Vector3 wallUpMoveDir = Vector3.Cross(wallForwardCross, _results[0].normal);

        Vector3 wallRightCross = Vector3.Cross(-_cameraMovement.transform.right, _results[0].normal);
        Vector3 wallRightMoveDir = Vector3.Cross(wallRightCross, _results[0].normal);

        Vector3 dir = (wallUpMoveDir * vertical + wallRightMoveDir * horizontal).normalized;

        // forward와 닿은 벽의 normal사이의 각도를 구한 후, 현재 y 와 구한 각도 사이 값을 구하고, 그 값을 90으로 나눈 것에 Multiplier를 곱함
        float signedAngle = Vector3.SignedAngle(Vector3.forward, _results[0].normal, Vector3.up);
        float deltaAngle = Mathf.DeltaAngle(_cameraMovement.transform.eulerAngles.y, signedAngle);
        float rotationAngle = -deltaAngle / 90f * _camRotationMultiplier;
        
        OnGUIManager.Instance.SetGUI("deltaAngle", deltaAngle);
        
        // 고개가 돌아가도 괜찮은 각도
        if(deltaAngle >= -150 && deltaAngle <= 150)
        {
            _cameraMovement.RotationZ = Mathf.SmoothDamp(_cameraMovement.RotationZ, rotationAngle, ref _dampVelocity, _wallRotationDuration);
        }
        else
        {
            _cameraMovement.RotationZ = Mathf.SmoothDamp(_cameraMovement.RotationZ, 0f, ref _dampVelocity, _wallRotationDuration);
        }


        _moveAmount = Vector3.Lerp(_moveAmount, dir * _wallRunSpeed, Time.deltaTime * _lerpSmooth) * TimeManager.PlayerTimeScale;

        _rb.velocity = _moveAmount;

        // 벽에 붙게 만들기
        _rb.AddForce(-_results[0].normal, ForceMode.Impulse);
    }


    private void WallRunningEnd()
    {
        _playerStateController.RemoveState(Player_State.WallRunning);
        _playerJump.RemoveGravity = false;

        _camRotateTweener = DOTween.To(
            () => _cameraMovement.RotationZ,
            value => _cameraMovement.RotationZ = value,
            0f, _wallRotationDuration
        ).SetEase(Ease.InOutCubic);
    }

}
