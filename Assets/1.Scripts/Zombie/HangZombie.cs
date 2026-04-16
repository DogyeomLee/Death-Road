using UnityEngine;

public class HangZombie : ZombieBase
{
    [Header("매달리기 설정")]
    //매달릴때의 손 
    [SerializeField] private float jumpPower;
    [SerializeField] private float divePower;

    [Header("다이브 설정")]
    [SerializeField] private Rigidbody2D body;

    // 다이브를 한 번만 실행하기 위한 플래그
    private bool hasDived = false; 

    private void Awake()
    {
        if(body == null)
        {
            Debug.Log("body 없다");
        }
    }

    protected override void Update()
    {
       base.Update();

        if (zombieFSMManager.CurrentState == ZombieState.hang)
        {
            if (!hasDived) // 아직 다이브를 안 했다면
            {
                Dive();
                hasDived = true; // 다이브 완료 처리
            }
        }
    }
    //
    //Todo : 다이브 했을때, 차량에 매달리기, 어느정도 속도가 되면 차량에서 떨어지기, 
    //아직  다이브가 정상적으로 작동하지 않음, 수정
    private void Dive()
    {
        ActivateAnimation();

        float direction = zombieMovement.GetDirection;

        Vector2 divePos = new Vector2(direction, jumpPower);

        body.AddForce(divePos * divePower * Time.fixedDeltaTime, ForceMode2D.Impulse);

        ActivateRagdoll();

        Invoke("OffAnimation", 0.3f);
    }

    private void ActivateAnimation()
    {
        zombieMovement.animator.SetTrigger("Dive");
    }
    private void ActivateRagdoll()
    {
        zombieRagdoll.ActivateRagdoll();
    }

    private void OffAnimation()
    {
        zombieMovement.animator.enabled = false;
    }
}
