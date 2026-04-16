using UnityEngine;

public class ZombiePart : MonoBehaviour
{
    // 부모를 참조하여 충돌 이벤트 전달
    public ZombieBase parentZombie;

    private void Awake()
    {
        if(parentZombie == null)
        {
            parentZombie = GetComponentInParent<ZombieBase>();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 부모의 OnCollisionEnter2D를 직접 호출하거나 이벤트를 발생시킴
        if (parentZombie != null)
        {
            // 부모의 로직을 그대로 사용하도록 전달
            parentZombie.OnCollisionEnter2D(collision);
        }
    }
}
