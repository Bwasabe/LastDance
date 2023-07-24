using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : PlayerComponentBase
{
    [SerializeField]
    private float _speed = 8;

    [SerializeField]
    private float _lerpSmooth = 15f;
    
    public bool IsFreeze{ get; set; }
    
    private Rigidbody _rb;
    private Transform _camTransform;

    private Vector3 _moveAmount;

    protected override void Start()
    {
        base.Start();
        
        _camTransform = Define.MainCam.transform;
        
        _rb = transform.GetComponentCache<Rigidbody>();
        
    }

    private void Update()
    {
        if(IsFreeze) return;

         Vector3 moveInput = GetInput();
         SetState(moveInput);
         
        Move(moveInput);
    }

    /// <summary>
    /// Input Vector 리턴
    /// </summary>
    /// <returns></returns>
    private Vector3 GetInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
        return new Vector3(horizontal, 0f, vertical).normalized;
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

        Vector3 dir = (right * input.x + forward * input.z).normalized;

        _moveAmount = Vector3.Lerp(_moveAmount, dir * _speed, Time.deltaTime * _lerpSmooth) * TimeManager.PlayerTimeScale;
        _moveAmount.y = _rb.velocity.y;

        _rb.velocity = _moveAmount;
    }



}
