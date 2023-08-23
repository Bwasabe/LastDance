using UnityEngine;

public static class RigidbodyExtensions
{
    public static void SetVelocityY(this Rigidbody rigidbody, float velocityY)
    {
        Vector3 velocity = rigidbody.velocity;
        velocity.y = velocityY;
        rigidbody.velocity = velocity;
    }
    
    public static void VelocityToward(this Rigidbody rigidbody, Vector3 toward, float smooth)
    {
        OnGUIManager.Instance.SetGUI("TowardMag", toward.magnitude);
        OnGUIManager.Instance.SetGUI("RBMag", rigidbody.velocity.magnitude);
        
        if(Mathf.Abs(rigidbody.velocity.magnitude - toward.magnitude) < 0.1f)
        {
            rigidbody.velocity = toward;
        }
        else
        {
            // 향할 Velocity까지의 방향벡터를 구한 후
            Vector3 dir = toward - rigidbody.velocity;
            
            dir.Normalize();
            
            // 현재 Velocity를 dir에smooth를 곱한 만큼 더해준다
            Vector3 velocity = rigidbody.velocity;
            velocity += dir * smooth;
            rigidbody.velocity = velocity;
        }
        
    }
}
