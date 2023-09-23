using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UGTurretBasicState : IState
{
    protected UGTurretStateMachine stateMachine;
    protected UpgradeTurretController turret;



    public UGTurretBasicState(UGTurretStateMachine turretStateMachine)
    {
        stateMachine = turretStateMachine;
    }

    public virtual void Enter()
    {
        turret = stateMachine.Turret;
    }

    public virtual void Exit()
    {

    }

    public virtual void HandleInput()
    {

    }

    public virtual void OnCollisionEnter(Collision collision)
    {

    }

    public virtual void OnCollisionExit(Collision collision)
    {

    }

    public virtual void OnTriggerEnter(Collider collider)
    {

    }

    public virtual void OnTriggerExit(Collider collider)
    {

    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void OnDrawGizmos()
    {

    }
}
