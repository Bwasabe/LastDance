using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTurretController : MonoBehaviour, ITakeDamageable
{
    private UGTurretStateMachine stateMachine;

    [field: SerializeField]
    public Rigidbody HeadRigidbody { get; private set; }
    [field: SerializeField]
    public Transform Head { get; private set; }
    [field: SerializeField]
    public Transform TargetPos { get; private set; }
    [field: SerializeField]
    public Transform BulletPos { get; private set; }
    [field: SerializeField]
    public ParticleSystem MuzzleFlash { get; private set; }
    [field: SerializeField]
    public GameObject bullet { get; private set; }

    [field: SerializeField]
    public float AngleRange { get; private set; } = 30f;
    [field: SerializeField]
    public float Radius { get; private set; } = 3f;
    [field: SerializeField]
    public float WaitTime { get; private set; } = 2f;

    private void Awake()
    {
        stateMachine = new UGTurretStateMachine(this);

        HeadRigidbody = Head.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        stateMachine.ChangeState(stateMachine.IdleState);
    }

    private void Update()
    {
        stateMachine.HandleInput();

        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        stateMachine.OnCollisionEnter(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        stateMachine.OnCollisionExit(collision);
    }

    private void OnTriggerEnter(Collider collider)
    {
        stateMachine.OnTriggerEnter(collider);
    }

    private void OnTriggerExit(Collider collider)
    {
        stateMachine.OnTriggerExit(collider);
    }

    private void OnDrawGizmos()
    {
        stateMachine?.OnDrawGizmos();
    }

    public void SpawnBullet(Vector3 pos, Quaternion rot)
    {
        GameObject g = PoolManager.Instantiate(bullet);

        g.transform.localPosition = pos;
        g.transform.localRotation = rot;

        // Debug.Log(g.transform.localRotation.eulerAngles);
    }

    public void Hit()
    {
        stateMachine.ChangeState(stateMachine.DeathState);
    }
}
