using UnityEngine;

public static class RigidbodyExtension
{
    public static void SetVelocityY(this Rigidbody rigidbody, float velocityY)
    {
        Vector3 velocity = rigidbody.velocity;
        velocity.y = velocityY;
        rigidbody.velocity = velocity;
    }
}
