using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerWallRunning : MonoBehaviour
{
    [SerializeField]
    private LayerMask _groundLayer;

    [SerializeField]
    private float _wallRunSpeed = 8f;
    [SerializeField]
    private float _wallCheckDistance = 0.01f;

    [SerializeField]
    private float _lerpSmooth = 12f;


    private Rigidbody _rb;
    private CapsuleCollider _capsuleCollider;

    private PlayerJump _playerJump;

    private Transform _camTransform;
    private Vector3 _moveAmount;

    private readonly List<RaycastHit> _results = new(8);

    private Coroutine _wallJumpCoroutine;
    private bool _isWallRunning = false;
    
    private void Start()
    {
        _rb = transform.GetComponentCache<Rigidbody>();
        _capsuleCollider = transform.GetComponentCache<CapsuleCollider>();
        _playerJump = transform.GetComponentCache<PlayerJump>();

        _camTransform = Define.MainCam.transform;
    }

    // 벽을 체크해서 달라붙음
    private void CheckWallState()
    {
        _results.Clear();
        Vector3 center = transform.position + _capsuleCollider.center;

        const int rayCount = 8;
        const float oneStep = 360f / rayCount;

        // Color[] colorTable = new[] {
        //     Color.white, Color.red, Color.yellow, Color.green, Color.cyan,
        //     Color.blue, Color.magenta, Color.gray, Color.black
        // };

        float startAngle = -_camTransform.eulerAngles.y + 90f;

        for (int i = 0; i < 5; ++i)
        {
            {
                float angle = startAngle + oneStep * i;
                Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0f, Mathf.Sin(angle * Mathf.Deg2Rad));
                Ray ray = new(center, direction);

                if(Physics.Raycast(ray, out RaycastHit hit, _wallCheckDistance, _groundLayer))
                {
                    OnGUIManager.Instance.SetGUI("HitNormal", hit.normal);
                    _results.Add(hit);
                }

                // Debug.DrawLine(center, center + direction * _wallCheckDistance, colorTable[i], 1f);
            }

            {
                float angle = startAngle - oneStep * i;
                Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0f, Mathf.Sin(angle * Mathf.Deg2Rad));
                Ray ray = new(center, direction);

                if(Physics.Raycast(ray, out RaycastHit hit, _wallCheckDistance, _groundLayer))
                {
                    OnGUIManager.Instance.SetGUI("HitNormal", hit.normal);
                    _results.Add(hit);
                }
                
                // Debug.DrawLine(center, center + direction * _wallCheckDistance, colorTable[i], 1f);

            }
        }

    }

    private void Update()
    {
        if(_playerJump.IsGround()) return;

        CheckWallState();

        if(_results.Count == 0)
        {
            if(_isWallRunning)
                WallRunningEnd();
            return;
        }

        if(Input.GetKey(KeyCode.Space))
        {
            if(!_isWallRunning)
                WallRunningStart();
            
            WallRunning();
            StickWall();
        }
        else
        {
            if(_isWallRunning)
                WallRunningEnd();
        }
        
    }

    
    private void WallRunningStart()
    {
        Debug.Log("Start");
        // 카메라 각도 돌림
        _isWallRunning = true;
        
        _playerJump.AddJumpCount();
        
        _playerJump.enabled = false;
    }

    private void WallRunning()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        _rb.SetVelocityY(0f);

        Vector3 wallForwardCross = Vector3.Cross(-_camTransform.forward, _results[0].normal);
        Vector3 wallUpMoveDir = Vector3.Cross(wallForwardCross, _results[0].normal);

        Vector3 wallRightCross = Vector3.Cross(-_camTransform.right, _results[0].normal);
        Vector3 wallRightMoveDir = Vector3.Cross(wallRightCross, _results[0].normal);

        Vector3 dir = (wallUpMoveDir * vertical + wallRightMoveDir * horizontal).normalized;

        _moveAmount = Vector3.Lerp(_moveAmount, dir * _wallRunSpeed, Time.deltaTime * _lerpSmooth) * TimeManager.PlayerTimeScale;

        _rb.velocity = _moveAmount;
    }

    private void StickWall()
    {
        _rb.AddForce(-_results[0].normal, ForceMode.Impulse);
    }

    private void WallRunningEnd()
    {
        Debug.Log("End");

        _isWallRunning = false;

        _playerJump.enabled = true;
    }

}
