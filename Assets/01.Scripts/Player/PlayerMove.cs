using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : PlayerComponentBase
{
    [SerializeField]
    private float _moveSpeed = 8;

    [SerializeField]
    private float _groundSmooth = 13f;

    [SerializeField]
    private float _airSmooth = 1f;

    [SerializeField]
    private float _towardSmooth = 1f;

    public bool IsFreeze{ get; set; }
    
    private Rigidbody _rb;
    private Transform _camTransform;
    private PlayerGroundController _groundController;

    private Vector3 _moveAmount;

    public Vector3 MoveDir { get; private set; }
    
    public float Speed{ get; set; }

    protected override void Start()
    {
        base.Start();
        
        _camTransform = Define.MainCam.transform;
        
        _rb = transform.GetComponentCache<Rigidbody>();
        _groundController = transform.GetComponentCache<PlayerGroundController>();
        
        ResetSpeed();
    }

    private void Update()
    {
        if(IsFreeze) return;

         Vector3 moveInput = Define.GetInput();
         SetState(moveInput);

        Move(moveInput);
    }

    public void ResetSpeed()
    {
        Speed = _moveSpeed;
    }


    /// <summary>
    /// dir에 따라 플레이어의 State를 조절하는 함수
    /// </summary>
    /// <param name="dir">
    /// 움직이는 방향
    /// </param>
    private void SetState(in Vector3 dir)
    {
        if (dir != Vector3.zero)
        {
            _playerStateController.AddState(Player_State.Move);
            _playerStateController.RemoveState(Player_State.Idle);
        }
        else
        {
            _playerStateController.AddState(Player_State.Idle);
            _playerStateController.RemoveState(Player_State.Move);
        }
    }

    /// <summary>
    /// input 방향으로 움직이는 함수
    /// </summary>
    /// <param name="input">
    /// 움직일 방향
    /// </param>
    private void Move(in Vector3 input)
    {
        OnGUIManager.Instance.SetGUI("Speed", Speed);
        Vector3 forward = _camTransform.forward;
        forward.y = 0f;

        Vector3 right = new Vector3(forward.z, 0f, -forward.x);
        
        MoveDir = (right * input.x + forward * input.z).normalized;
        
        if(!_groundController.IsGround)
        {
            if(input == Vector3.zero) return;

            _moveAmount = MoveDir;
            _moveAmount.y = 0f;
            _moveAmount.Normalize();
            
            Vector3 velocityWithoutY = new(_rb.velocity.x, 0f, _rb.velocity.z);

            float magnitude = velocityWithoutY.magnitude;
            
            velocityWithoutY.Normalize();

            float dot = Vector3.Dot(_moveAmount, velocityWithoutY);

            Debug.Log(dot);

            if(Math.Abs(dot - 1f) < 0.005f && magnitude >= _moveSpeed)
                _moveAmount = Vector3.Lerp(new(MoveDir.x * magnitude, 0f, MoveDir.z * magnitude), MoveDir * Speed, Time.deltaTime * _airSmooth) * TimeManager.PlayerTimeScale;
            else
                _moveAmount = Vector3.Lerp( _rb.velocity,MoveDir * Speed, (dot + 1) / 2) * TimeManager.PlayerTimeScale;

            _moveAmount.y = _rb.velocity.y;
            
            if(magnitude >= _moveSpeed)
                _rb.VelocityToward(_moveAmount, 10f);
            else
                _rb.VelocityToward(_moveAmount, _towardSmooth);
            
            _rb.SetVelocityY(_moveAmount.y);
        }  
        else if(_groundController.IsOnSlope)
        {
            MoveDir = Vector3.ProjectOnPlane(MoveDir, _groundController.GroundInfo.normal);

            _moveAmount = MoveDir * Speed;
            
            if(_playerStateController.HasState(Player_State.Jump))
            {
                _moveAmount.y = _rb.velocity.y;
                
                _rb.VelocityToward(_moveAmount, _towardSmooth);
                _rb.SetVelocityY(_moveAmount.y);
            }
            else
                _rb.velocity = _moveAmount;
        }
        else
        {
            _moveAmount = Vector3.Lerp(_moveAmount, MoveDir * Speed, Time.deltaTime * _groundSmooth) * TimeManager.PlayerTimeScale;
            _moveAmount.y = _rb.velocity.y;
            
            _rb.VelocityToward(_moveAmount, _towardSmooth);
            _rb.SetVelocityY(_moveAmount.y);
        }
        
    }
    
}
