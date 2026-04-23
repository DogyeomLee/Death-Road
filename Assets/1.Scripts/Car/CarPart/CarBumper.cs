using UnityEngine;
using UnityEngine.InputSystem.XR;

public class CarBumper : MonoBehaviour
{
    public LayerMask targetLayer;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 내(범퍼)가 직접 충돌을 감지함
        if ((targetLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            ZombieBase zombie = collision.gameObject.GetComponent<ZombieBase>();

            if (zombie != null)
            {
                zombie.Destroy();
            }

            DamagableItem damagableItem = collision.gameObject.GetComponent<DamagableItem>();

            if(damagableItem != null)
            {
                damagableItem.Destroy();
            }
        }
    }
}
