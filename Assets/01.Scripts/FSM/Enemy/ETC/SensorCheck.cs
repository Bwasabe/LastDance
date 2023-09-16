using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorCheck : MonoBehaviour
{
    private bool EnterPlayer = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.parent != null && other.transform.parent.CompareTag("Player"))
        {
            EnterPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent != null &&  other.transform.parent.CompareTag("Player"))
        {
            EnterPlayer = false;
        }
    }

    public bool CurrentState()
    {
        return EnterPlayer;
    }
}
