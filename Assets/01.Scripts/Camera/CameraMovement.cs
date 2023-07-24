using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private float _sencetive = 3f;

    [SerializeField]
    private Vector3 _maxRotate = new Vector3(0f,89f,0f);

    [SerializeField]
    private Vector3 _minRotate= new Vector3(0f,-89f,0f);

    private float _rotateX = 0f;
    private float _rotateY = 0f;

    
    private void LateUpdate()
    {
        Move();
    }

    private void Move()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        _rotateX += x * _sencetive;
        _rotateY += y * _sencetive;

        _rotateY = Mathf.Clamp(_rotateY, _minRotate.y, _maxRotate.y);

        transform.rotation = Quaternion.Euler(-_rotateY, _rotateX, 0f);
    }
}
