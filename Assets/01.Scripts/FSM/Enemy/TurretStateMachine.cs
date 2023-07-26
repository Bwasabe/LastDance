
public class TurretStateMachine : StateMachine
{
    public TurretController Turret { get; }

    public TurretPatrolState PatrolState { get; }
    public TurretShootState ShootState { get; }
    public TurretDeathState DeathState { get; }

    public TurretStateMachine(TurretController turret)
    {
        Turret = turret;

        PatrolState = new TurretPatrolState(this);
        ShootState = new TurretShootState(this);
        DeathState = new TurretDeathState(this);
    }
}
