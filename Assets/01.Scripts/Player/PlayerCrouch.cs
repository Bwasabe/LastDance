using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerCrouch : PlayerComponentBase
{
    [field:SerializeField]
    public float CrouchSpeed{ get; private set; } = 3f;

    [SerializeField]
    private float _capsuleHeight = 1f;

    [SerializeField]
    private Vector3 _capsuleCenter = new(0f, .5f, 0f);
    
    [SerializeField]
    private float _camHeight = 0.5f;
    [SerializeField]
    private float _animDuration = 0.15f;

    private PlayerMove _playerMove;
    private PlayerGroundController _groundController;

    private CapsuleCollider _capsuleCollider;
    
    
    private Transform _camTransform;

    private Sequence _sequence;

    private Vector3 _capsuleOriginCenter;
    
    private float _capsuleOriginHeight;
    private float _camOriginHeight;

    protected override void Start()
    {
        _camTransform = Define.MainCam.transform;
        
        _capsuleCollider = transform.GetComponentCache<CapsuleCollider>();
        _playerMove = transform.GetComponentCache<PlayerMove>();
        _groundController = transform.GetComponentCache<PlayerGroundController>();
        
        _camOriginHeight = _camTransform.localPosition.y;
        _capsuleOriginHeight = _capsuleCollider.height;
        _capsuleOriginCenter = _capsuleCollider.center;
        
        base.Start();
    }

    private void StartCrouch()
    {
        if(_sequence.IsActive() && _sequence.IsPlaying())
            _sequence.Kill();
        
        Ease ease = Ease.InQuart;
        if(Define.GetInput() == Vector3.zero)
            ease = Ease.Linear;

        _sequence = DOTween.Sequence();
        
        _sequence.Join(DOTween.To(
            () => _capsuleCollider.center,
            value => _capsuleCollider.center = value,
            _capsuleCenter, _animDuration));
        
        _sequence.Join(DOTween.To(
            () => _capsuleCollider.height,
            value => _capsuleCollider.height = value,
            _capsuleHeight, _animDuration));

        _sequence.Join(_camTransform.DOLocalMoveY(_camHeight, _animDuration).SetEase(ease));
        
        _playerMove.Speed = CrouchSpeed;
    }

    private void EndCrouch()
    {
        if(_sequence.IsActive())
            _sequence.Kill();

        _sequence = DOTween.Sequence();
        
        _sequence.Join(DOTween.To(
            () => _capsuleCollider.center,
            value => _capsuleCollider.center = value,
            _capsuleOriginCenter, _animDuration));
        
        _sequence.Join(DOTween.To(
            () => _capsuleCollider.height,
            value => _capsuleCollider.height = value,
            _capsuleOriginHeight, _animDuration));

        _sequence.Join(_camTransform.DOLocalMoveY(_camOriginHeight, _animDuration).SetEase(Ease.Linear));
        
        _playerMove.ResetSpeed();
    }
    
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift) && _groundController.IsGround)
        {
            StartCrouch();
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            EndCrouch();
        }
    }
}
