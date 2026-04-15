using UnityEngine;

public class TankZombie : ZombieBase
{
    [SerializeField] private float MoveSpeed;

    protected override void FixedUpdate()
    {
        if (zombieFSMManager.CurrentState == ZombieState.Chase)
        {
            Move();
        }
    }

    protected override void Move()
    {
        zombieMovement.moveSpeed = MoveSpeed;

        zombieMovement.MoveZombie(targetCar.transform);
    }
}
