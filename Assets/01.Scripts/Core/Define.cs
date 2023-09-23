using System;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class Bound
{
    [SerializeField]
    private Vector2 _lt;
    [SerializeField]
    private Vector2 _rb;

    public Vector2 Center => new Vector2(_lt.x + (_rb.x - _lt.x) * 0.5f, _lt.y + (_rb.y - _lt.y) * 0.5f);
    public Vector2 Size => new Vector2(_rb.x - _lt.x, _rb.y - _lt.y);

    public Vector2 GetRandomPos()
    {
        float x = Random.Range(_lt.x, _rb.x);
        float y = Random.Range(_lt.y, _rb.y);
        return new Vector2(x, y);
    }
}
public static class Define
{
    private static Camera _mainCam;
    public static Camera MainCam
    {
        get
        {
            if (_mainCam == null)
            {
                _mainCam = Camera.main;
            }
            return _mainCam;
        }
    }

    private static GameObject _player;
    public static GameObject Player
    {
        get
        {
            if(_player == null)
            {
                _player = GameObject.FindAnyObjectByType<PlayerStateController>().gameObject;
            }
            return _player;
        }
    }
    
    public static Vector3 GetInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
        return new Vector3(horizontal, 0f, vertical).normalized;
    }
    
    public static Vector2 MousePos => MainCam.ScreenToWorldPoint(Input.mousePosition);

    public static T GetRandomEnum<T>(int startPos = 0,int? length = null ) where T : System.Enum
    {
        Array values = Enum.GetValues(typeof(T));
        if(length == null)
        {
            return (T)values.GetValue(Random.Range(startPos, values.Length));
        }
        else
            return (T)values.GetValue(Random.Range(startPos,length.Value));
    }

    public static Bound GetRandomBound(params Bound[] bounds)
    {

        float random = Random.Range(0f, 1f);
        float sizeAmount = 0f;

        float currentSizeRandom = 0f;

        foreach (Bound bound in bounds)
        {
            sizeAmount += bound.Size.magnitude;
        }
        
        foreach (Bound bound in bounds)
        {
            if (random <= bound.Size.magnitude / sizeAmount + currentSizeRandom)
            {
                return bound;
            }
            else
            {
                currentSizeRandom += bound.Size.magnitude / sizeAmount;
            }
        }

        int rand = Random.Range(0, bounds.Length);
        return bounds[rand];
    }

    public static Vector3 AngleToVector3(float angle)
    {
        return new(Mathf.Cos(angle * Mathf.Deg2Rad), 0f, Mathf.Sin(angle * Mathf.Deg2Rad));
    }

    // public static Vector3 RotatedVector(Vector3 vec)
    // {
    //     float xAngle = Mathf.Atan2(vec.y, vec.z) * Mathf.Rad2Deg;
    //
    //     float yAngle = Mathf.Atan2(vec.z, vec.x) * Mathf.Rad2Deg;
    //
    //     float zAngle = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
    //     
    //     return new(xAngle, yAngle, zAngle);
    // }
}

namespace System.Runtime.CompilerServices
{
    public static class IsExternalInit {}
}
