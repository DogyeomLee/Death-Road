using UnityEngine;

public class ZombieMovement : MonoBehaviour
{
    [Header("기본 값 세팅")]
    public float moveSpeed;
    public Animator anim;
    public Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// 좀비 이동
    /// </summary>
    /// <param name = "targetCar"> 목표 자동차</param>
    public void MoveZombie(Transform targetCar)
    {
        anim.SetBool("Move", true);

        //좀비가 차보다 왼쪽에 있으면, -1 방향, 반대면 반대..
        float direction = (targetCar.position.x > transform.position.x) ? 1 : -1;

        Vector2 movePosition = new Vector2(direction * moveSpeed * Time.fixedDeltaTime, 0);

        RotateZombie(direction);

         rb.MovePosition(movePosition);
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
}
