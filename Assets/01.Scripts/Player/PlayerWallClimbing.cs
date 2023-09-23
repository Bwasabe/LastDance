using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerWallClimbing : PlayerComponentBase
{
    private readonly int WallClimbHash = Animator.StringToHash("Climb"); 
    // Climb중엔 Jump, Move, WallRunning, Crouch, Dash, Sliding Enable 꺼버리기
    [SerializeField]
    private Vector3 _offset = new Vector3(0f, 1f, 1f);
    [SerializeField]
    private float _checkDistance = 1f;
    [SerializeField]
    private float _climbUpDuration = .1f;
    [SerializeField]
    private float _climbEndDuration = 0.1f;
    [SerializeField]
    private float _climbPosYOffset = 0.25f;
    
    private PlayerMove _playerMove;
    private PlayerJump _playerJump;
    private PlayerDash _playerDash;
    private PlayerCrouch _playerCrouch;
    private PlayerSliding _playerSliding;
    private PlayerWallRunning _PlayerWallRunning;
    private PlayerGroundController _playerGroundController;

    private PlayerItemController _playerItemController;
    
    private Rigidbody _rb;
    private CapsuleCollider _capsuleCollider;
    private Animator _animator;

    protected override void Start()
    {
        base.Start();

        _animator = transform.GetComponentCache<Animator>();
        
        _playerMove = transform.GetComponentCache<PlayerMove>();
        _playerJump = transform.GetComponentCache<PlayerJump>();
        _playerDash = transform.GetComponentCache<PlayerDash>();
        _playerCrouch = transform.GetComponentCache<PlayerCrouch>();
        _playerSliding = transform.GetComponentCache<PlayerSliding>();
        _playerItemController = transform.GetComponentCache<PlayerItemController>();
        
        _PlayerWallRunning = transform.GetComponentCache<PlayerWallRunning>();
        _playerGroundController = transform.GetComponentCache<PlayerGroundController>();

        _rb = transform.GetComponentCache<Rigidbody>();
        _capsuleCollider = transform.GetComponentCache<CapsuleCollider>();
    }

    private void Update()
    {
        Vector3 rayPos = transform.position + _capsuleCollider.center + Quaternion.Euler(0f, Define.MainCam.transform.eulerAngles.y, 0f) * _offset;
        Ray ray = new(rayPos, Vector3.down);

        if(_PlayerWallRunning.IsLookWall && Input.GetKey(KeyCode.Space) && !_playerStateController.HasState(Player_State.Climbing))
        {
            if(Physics.Raycast(ray, out RaycastHit hitInfo, _checkDistance, _playerGroundController.GroundLayer.value))
            {
                Vector3 climbPos = hitInfo.point + Vector3.up * _climbPosYOffset;
                StartCoroutine(Climbing(climbPos));
            }
        }
        Debug.DrawRay(rayPos, ray.direction * _checkDistance, Color.cyan, 1f);
    }

    private IEnumerator Climbing(Vector3 climbPos)
    {
        _playerStateController.AddState(Player_State.Climbing);
        
        _rb.SetVelocityY(0f);
        
        _animator.SetTrigger(WallClimbHash);

        _playerItemController.HandTransform.gameObject.SetActive(false);
        
        _playerMove.enabled = false;
        _playerJump.enabled = false;
        _playerDash.enabled = false;
        _playerCrouch.enabled = false;
        _playerSliding.enabled = false;
        _PlayerWallRunning.enabled = false;
        
        {
            float timer = 0f;
            
            Vector3 startPos = transform.position;
            Vector3 endPos = new Vector3(startPos.x, climbPos.y, startPos.z);

            while (timer < _climbUpDuration)
            {
                timer += Time.deltaTime;
                
                Vector3 movePos = Vector3.Lerp(startPos, endPos, timer / _climbUpDuration);
                _rb.MovePosition(movePos);

                yield return Yields.WaitForFixedUpdate;
            }
        }

        {
            float timer = 0f;
            
            Vector3 startPos = transform.position;
            Vector3 endPos = new Vector3(climbPos.x, climbPos.y - _climbPosYOffset, climbPos.z);

            while (timer < _climbEndDuration)
            {
                timer += Time.deltaTime;
                
                Vector3 movePos = Vector3.Lerp(startPos, endPos, timer / _climbEndDuration);
                _rb.MovePosition(movePos);

                yield return Yields.WaitForFixedUpdate;
            }
        }
        
        _playerMove.enabled = true;
        _playerJump.enabled = true;
        _playerDash.enabled = true;
        _playerCrouch.enabled = true;
        _playerSliding.enabled = true;
        _PlayerWallRunning.enabled = true;
        
        _playerItemController.HandTransform.gameObject.SetActive(true);
        
        _playerStateController.RemoveState(Player_State.Climbing);

    }

    private void OnDrawGizmos()
    {
        try
        {
            Gizmos.DrawSphere(transform.position + _capsuleCollider.center + Quaternion.Euler(0f, Define.MainCam.transform.eulerAngles.y, 0f) * _offset, 0.3f);
        }
        catch
        {
            // ignored
        }
    }
}
