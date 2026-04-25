using Unity.VisualScripting;
using UnityEngine;

public class ZombieMovement : MonoBehaviour
{
    [Header("기본 값 세팅")]
    public float moveSpeed;
    public Animator animator;
    public Rigidbody2D rb;

    private float direction;

    public float GetDirection => direction;

    private void Awake()
    {
        if(rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        if(animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    /// <summary>
    /// 좀비 이동
    /// </summary>
    /// <param name = "targetCar"> 목표 자동차</param>
    public void MoveZombie(Transform targetCar)
    {
        //방향 결정
        direction = (targetCar.position.x > transform.position.x) ? 1 : -1;

        animator.SetBool("isMoving", true);

        // 이동: X축은 속도에 맞게, Y축은 기존의 중력 속도(linearVelocity.y) 그대로 유지
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

        RotateZombie(direction);
    }

    private void RotateZombie(float direction)
    {
        if (direction == 1)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void DieZombie()
    {
        animator.enabled = false;
    }

}
