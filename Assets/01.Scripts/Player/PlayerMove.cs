using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : PlayerComponentBase
{
    [SerializeField]
    private float _moveSpeed = 8;

    [SerializeField]
    private float _groundSmooth = 13f;

    [SerializeField]
    private float _airSmooth = 6f;

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

         float smooth = GetSmooth();
         
        Move(moveInput, smooth);
    }

    public void ResetSpeed()
    {
        Speed = _moveSpeed;
    }

    private float GetSmooth()
    {   
        if(_groundController.IsGround)
            return _groundSmooth;
        else
            return _airSmooth;
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
    private void Move(in Vector3 input, in float lerpSmooth)
    {
        OnGUIManager.Instance.SetGUI("Speed", Speed);
        Vector3 forward = _camTransform.forward;
        forward.y = 0f;

        Vector3 right = new Vector3(forward.z, 0f, -forward.x);

        MoveDir = (right * input.x + forward * input.z).normalized;
        
        if(_groundController.IsOnSlope)
        {
            MoveDir = Vector3.ProjectOnPlane(MoveDir, _groundController.GroundInfo.normal);
            
            _moveAmount = Vector3.Lerp(_rb.velocity, MoveDir * Speed, Time.deltaTime * lerpSmooth) * TimeManager.PlayerTimeScale;

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
            _moveAmount = Vector3.Lerp(_moveAmount, MoveDir * Speed, Time.deltaTime * lerpSmooth) * TimeManager.PlayerTimeScale;
            _moveAmount.y = _rb.velocity.y;
            
            _rb.VelocityToward(_moveAmount, _towardSmooth);
            _rb.SetVelocityY(_moveAmount.y);
        }
        
    }



}
