using System;
using DG.Tweening;
using UnityEngine;

public class DefaultWeapon : ItemBase
{
    private readonly int AttackHash = Animator.StringToHash("Attack");
    private readonly int AttackAniHash = Animator.StringToHash("AttackAni");
    
    [SerializeField]
    private float _attackDuration = 0.5f;

    [SerializeField]
    private Vector3 _innerParticleRotation;

    [SerializeField]
    private Vector3 _outerParticleRotation;

    [SerializeField]
    private Transform _attackParticle;
    [SerializeField]
    private SphereCollider _attackCollider;

    [SerializeField]
    private float _attackTweenDuration = 0.3736f;
    [SerializeField]
    private AnimationCurve _tweenAnimCurve;
    
    [SerializeField]
    private Vector3 _innerRotationValue;
    [SerializeField]
    private Vector3 _outerRotationValue;

    private float _timer;

    private float _attackAni;

    private Animator _playerAnimator;

    private CameraMovement _cameraMovement;
    
    private AnimationEventHandler _animationEventHandler;
    private PlayerStateController _playerStateController;

    private Tweener _rotationValueTweener;
    
    protected void Start()
    {
        _playerAnimator = transform.GetComponentCache<Animator>();

        _cameraMovement = Define.MainCam.GetComponent<CameraMovement>();

        _animationEventHandler = transform.GetComponentCache<AnimationEventHandler>();

        _playerStateController = transform.GetComponentCache<PlayerStateController>();
        
        _animationEventHandler.AddEvent(nameof(SetInnerAttackParticleRotation), SetInnerAttackParticleRotation);
        _animationEventHandler.AddEvent(nameof(SetOuterAttackParticleRotation), SetOuterAttackParticleRotation);
        
        _animationEventHandler.AddEvent(nameof(DOInnerRotationValue),DOInnerRotationValue);
        _animationEventHandler.AddEvent(nameof(DOOuterRotationValue),DOOuterRotationValue);
        
        _animationEventHandler.AddEvent(nameof(StopRotationValueTweener), StopRotationValueTweener);
        
        _animationEventHandler.AddEvent(nameof(EnableAttackCollider), EnableAttackCollider);
        _animationEventHandler.AddEvent(nameof(DisableAttackCollider), DisableAttackCollider);
        
        _rotationValueTweener = DOTween.To(
            () => _cameraMovement.RotationValue,
            value => _cameraMovement.RotationValue = value,
            _cameraMovement.RotationValue, _attackTweenDuration).SetEase(_tweenAnimCurve).SetAutoKill(false);
    }
    private void StopRotationValueTweener()
    {
        if(_rotationValueTweener is not null && _rotationValueTweener.IsActive() && _rotationValueTweener.IsPlaying())
            _rotationValueTweener.Pause();

        _cameraMovement.RotationValue = new(0f, 0f, _cameraMovement.RotationValue.z);
    }
    [ContextMenu("TestOuter")]
    private void DOOuterRotationValue()
    {
        _rotationValueTweener.ChangeEndValue(_outerRotationValue, true).Restart();
    }
    
    [ContextMenu("TestInner")]
    private void DOInnerRotationValue()
    {
        _rotationValueTweener.ChangeEndValue(_innerRotationValue, true).Restart();
    }

    public override void Execute()
    {
        if(_playerStateController.HasState(Player_State.Climbing)) return;
        
        // 애니메이션 실행
        if(Time.time - _timer < _attackDuration) return;
        _timer = Time.time;

        _attackAni = (_attackAni + 1) % 2;
        
        _playerAnimator.SetTrigger(AttackHash);
        _playerAnimator.SetFloat(AttackAniHash, _attackAni);
    }

    private void EnableAttackCollider()
    {
        _attackCollider.gameObject.SetActive(true);
    }

    private void DisableAttackCollider()
    {
        _attackCollider.gameObject.SetActive(false);
    }
    
    
    private void SetOuterAttackParticleRotation()
    {
        _attackParticle.localRotation = Quaternion.Euler(_outerParticleRotation);
    }
    
    private void SetInnerAttackParticleRotation()
    {
        _attackParticle.localRotation = Quaternion.Euler(_innerParticleRotation);
    }
    
}
