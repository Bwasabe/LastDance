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
    private float _airSmooth = 50f;

    [SerializeField]
    private float _towardSmooth = 11f;

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
    
    [ContextMenu("ResetSpeed")]
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
        Vector3 forward = _camTransform.forward;
        forward.y = 0f;

        Vector3 right = new Vector3(forward.z, 0f, -forward.x);
        
        MoveDir = (right * input.x + forward * input.z).normalized;
        Vector3 velocity = _rb.velocity;


        if(!_groundController.IsGround)
        {
            if(_rb.velocity.magnitude > Speed)
            {
                Vector3 velocityWithoutY = new Vector3(velocity.x, 0f, velocity.z).normalized;
                float dot = Vector3.Dot(MoveDir, velocityWithoutY);
                if(input == Vector3.zero)
                    dot = 1f;
                
                OnGUIManager.Instance.SetGUI("Dot", dot);
                // 외적에 따른 속도
                Vector3 dotSpeed = Vector3.Lerp(MoveDir * Speed , velocity, (dot + 1) * 0.5f);
                OnGUIManager.Instance.SetGUI("dotSpeed", dotSpeed); 

                _rb.velocity = Vector3.Lerp(_rb.velocity, dotSpeed, _towardSmooth * Time.deltaTime);
            }
            else
            {
                _rb.velocity = Vector3.Lerp(_rb.velocity, MoveDir * Speed, _towardSmooth * Time.deltaTime);
            }

            _rb.SetVelocityY(velocity.y);
        }
        else if(_groundController.IsOnSlope)
        {
            MoveDir = Vector3.ProjectOnPlane(MoveDir, _groundController.GroundInfo.normal);
            
            float multipleValue = Speed / new Vector3(MoveDir.x * Speed, 0f, MoveDir.z * Speed).magnitude + MoveDir.y;

            if(MoveDir.y >= 0 || input == Vector3.zero)
                multipleValue = 1f;
            
            _moveAmount = multipleValue * Speed * MoveDir;

            if(_playerStateController.HasState(Player_State.Jump))
            {
                _moveAmount.y = velocity.y;

                _rb.velocity = Vector3.Lerp(_rb.velocity, _moveAmount, _towardSmooth * Time.deltaTime);
                _rb.SetVelocityY(velocity.y);
            }
            else
            {
                _rb.velocity = _moveAmount;
            }
        }
        else
        {
            _moveAmount = MoveDir * Speed; //Vector3.Lerp(_moveAmount, MoveDir * Speed, Time.deltaTime * _groundSmooth) * TimeManager.PlayerTimeScale;
            _moveAmount.y = velocity.y;

            _rb.velocity = Vector3.Lerp(_rb.velocity, _moveAmount, _towardSmooth * Time.deltaTime);
            _rb.SetVelocityY(_moveAmount.y);
        }
        
    }
    
}
