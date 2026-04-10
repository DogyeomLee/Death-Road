using UnityEngine;

//빠른 속도로 쫒아까는 특수 좀비
public class FastZombie : ZombieBase
{
    [SerializeField] private float fastMoveSpeed;

    protected override void FixedUpdate()
    {
        if(zombieFSMManager.CurrentState == ZombieState.Chase)
        {
            Move();
        }
    }

    protected override void Move()
    {
        zombieMovement.moveSpeed = fastMoveSpeed;
        zombieMovement.anim.speed = 2;

        zombieMovement.MoveZombie(targetCar.transform);
    }
}
