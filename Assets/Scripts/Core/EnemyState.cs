using UnityEngine;

public abstract class EnemyState
{
    protected EnemyFlightController enemy;

    public EnemyState(EnemyFlightController enemyController)
    {
        this.enemy = enemyController;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
}