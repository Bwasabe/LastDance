using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerCrouch : PlayerComponentBase
{
    [field:SerializeField]
    public float CrouchSpeed{ get; private set; }= 3f;
    
    [SerializeField]
    private float _camHeight = 0.5f;
    [SerializeField]
    private float _camMoveDuration = 0.8f;

    private PlayerMove _playerMove;
    private PlayerGroundController _groundController;
    
    
    private Transform _camTransform;

    private Tweener _camMoveTweener;
    
    private float _camOriginHeight;

    protected override void Start()
    {
        _camTransform = Define.MainCam.transform;

        _camOriginHeight = _camTransform.localPosition.y;
        
        
        _playerMove = transform.GetComponentCache<PlayerMove>();
        _groundController = transform.GetComponentCache<PlayerGroundController>();
        
        base.Start();
    }

    private void StartCrouch()
    {
        _camMoveTweener.Kill();
        
        Ease ease = Ease.InQuart;
        if(Define.GetInput() == Vector3.zero)
        {
            ease = Ease.Linear;
        }
        
        _camMoveTweener = _camTransform.DOLocalMoveY(_camHeight, _camMoveDuration).SetEase(ease);

        _playerMove.Speed = CrouchSpeed;
    }

    private void EndCrouch()
    {
        _camMoveTweener.Kill();
        _camMoveTweener = _camTransform.DOLocalMoveY(_camOriginHeight, _camMoveDuration).SetEase(Ease.Linear);
        
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
