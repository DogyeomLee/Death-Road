using UnityEngine;

public class FastZombie : ZombieBase
{
    void Awake()
    {
        // 이동 속도 2배
        moveSpeed *= 2f;

        // 애니메이션 속도 2배
        if (anim != null)
            anim.speed = 3f;
    }
}
