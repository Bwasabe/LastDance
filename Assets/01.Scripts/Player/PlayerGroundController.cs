using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundController : MonoBehaviour
{
    
    [SerializeField]
    private float _groundCheckDistance = 0.3f;

    [SerializeField]
    private LayerMask _groundLayer;
    
    public bool CustomIsGround{ private get; set; }

    public bool IsGround{
        get {
            return _isGround || CustomIsGround;
        }
    }

    public bool GroundValue{
        get => _isGround;
    }

    private bool _isGround;


    public RaycastHit GroundInfo => _groundInfo;

    private RaycastHit _groundInfo;

    private void Update()
    {
        OnGUIManager.Instance.SetGUI("IsGround", IsGround);
        CheckGround();
    }

    private void CheckGround()
    {
        const float tolerance = 0.1f;
        Ray ray = new(transform.position + Vector3.up * tolerance, Vector3.down);

        _isGround = Physics.Raycast(ray, out _groundInfo, _groundCheckDistance + tolerance, _groundLayer.value);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, Vector3.down * _groundCheckDistance);
    }
}
