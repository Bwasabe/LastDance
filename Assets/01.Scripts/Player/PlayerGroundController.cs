using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundController : MonoBehaviour
{

    [SerializeField]
    private float _groundCheckDistance = 0.3f;

    [SerializeField]
    private float _maxSlopeAngle = 60f;

    [SerializeField]
    private LayerMask _groundLayer;
    
    public bool IsGround{ get; private set; }

    public bool IsOnSlope{ get; private set; }
    
    public RaycastHit GroundInfo => _groundInfo;
    private RaycastHit _groundInfo;

    private void Update()
    {
        OnGUIManager.Instance.SetGUI("IsGround", IsGround);
        CheckGround();
        CheckOnSlope();
    }

    private void CheckGround()
    {
        const float tolerance = 0.1f;
        Ray ray = new(transform.position + Vector3.up * tolerance, Vector3.down);

        IsGround = Physics.Raycast(ray, out _groundInfo, _groundCheckDistance + tolerance, _groundLayer.value);
    }

    private void CheckOnSlope()
    {
        if(IsGround)
        {
            float angle = Vector3.Angle(Vector3.up, _groundInfo.normal);
            IsOnSlope = angle < _maxSlopeAngle && angle != 0;
        }
        else
            IsOnSlope = false;
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.green;
    //     Gizmos.DrawRay(transform.position, Vector3.down * _groundCheckDistance);
    // }
}
