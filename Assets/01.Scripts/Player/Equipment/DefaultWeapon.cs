using System;
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
    
    private float _timer;

    private float _attackAni;

    private Animator _playerAnimator;
    
    private AnimationEventHandler _animationEventHandler;
    
    protected void Start()
    {
        _playerAnimator = transform.GetComponentCache<Animator>();

        _animationEventHandler = transform.GetComponentCache<AnimationEventHandler>();
        
        _animationEventHandler.AddEvent(nameof(SetInnerAttackParticleRotation), SetInnerAttackParticleRotation);
        _animationEventHandler.AddEvent(nameof(SetOuterAttackParticleRotation), SetOuterAttackParticleRotation);
        
        _animationEventHandler.AddEvent(nameof(EnableAttackCollider), EnableAttackCollider);
        _animationEventHandler.AddEvent(nameof(DisableAttackCollider), DisableAttackCollider);
    }
    
    public override void Execute()
    {
        // 애니메이션 실행
        if(_timer < _attackDuration) return;
        _timer = 0f;

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

    private void Update()
    {
        _timer += Time.deltaTime;
    }
}
