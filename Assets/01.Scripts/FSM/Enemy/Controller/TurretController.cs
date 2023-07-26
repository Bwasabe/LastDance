using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [field: Header("References")]
    [field: SerializeField] public TurretSO Data { get; private set; }

    private TurretStateMachine stateMachine;

    [field: SerializeField]
    public Transform Head { get; private set; }

    [field: SerializeField]
    public Transform BulletPos { get; private set; }

    [field: SerializeField]
    public ParticleSystem MuzzleFlash { get; private set; }

    public LaserCheck LaserCheck { get; private set; }
    public MeshRenderer LaserRenderer { get; private set; }
    public Rigidbody HeadRigidbody { get; private set; }

    // To do PoolManager으로 변환
    [SerializeField]
    private GameObject bullet;

    private void Awake()
    {
        stateMachine = new TurretStateMachine(this);
        LaserCheck = gameObject?.GetComponentInChildren<LaserCheck>();

        LaserRenderer = LaserCheck.GetComponent<MeshRenderer>();
        HeadRigidbody = Head.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        stateMachine.ChangeState(stateMachine.PatrolState);
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

    public void SpawnBullet(Vector3 pos, Quaternion rot)
    {
        Instantiate(bullet, pos, rot);
    }

}
