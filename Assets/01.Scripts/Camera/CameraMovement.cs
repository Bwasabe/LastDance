using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private float _sensitiveness = 3f;

    [SerializeField]
    private Vector2 _rotateClamp = new (-89f, 89f);

    private float _rotateX = 0f;
    private float _rotateY = 0f;

    private Coroutine _rotationValueCoroutine;
    
    [field:SerializeField]
    public Vector3 RotationValue{ get; set; }
    
    private void LateUpdate()
    {
        Move();
    }

    private void Move()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        _rotateX += x * _sensitiveness;
        _rotateY += y * _sensitiveness;
        
        _rotateY = Mathf.Clamp(_rotateY, _rotateClamp.x, _rotateClamp.y);

        transform.localRotation = Quaternion.Euler(-_rotateY + RotationValue.x,_rotateX + RotationValue.y, RotationValue.z);
    }

    public void SetRotationZ(float value)
    {
        Vector3 rotationValue = RotationValue;
        rotationValue.z = value;
        RotationValue = rotationValue;
    }
}
