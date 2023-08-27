using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    [SerializeField]
    private BoxCollider katanaCollider;

    private void Awake()
    {
        katanaCollider.enabled = false;
    }

    public void EnableKatanaCollider()
    {
        katanaCollider.enabled = true;
    }

    public void DisableKatanaCollider()
    {
        katanaCollider.enabled = false;
    }
}
