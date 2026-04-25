using System.ComponentModel;
using UnityEngine;

public class HangZombie : ZombieBase
{
    [Header("다이브 설정")]
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private float jumpPower;
    [SerializeField] private float divePower;

    [Header("매달리기 설정")]
    [SerializeField] private LayerMask hangLayer;
    [SerializeField] private FixedJoint2D[] hands;
    //매달리기의 최대 속도, 넘으면 차량에서 떨어짐
    [SerializeField] private float maxSpeed;

    private CarBase carBase;

    // 다이브를 한 번만 실행하기 위한 플래그
    private bool hasDived = false; 

    private bool isHang = false;

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

        if (zombieFSMManager.CurrentState == ZombieState.Hang)
        {
            if (!hasDived) // 아직 다이브를 안 했다면
            {
                Dive();
                hasDived = true; // 다이브 완료 처리
            }

            DeactiveHang();
        }
    }

    private void Dive()
    {
        ActivateAnimation();

        float direction = zombieMovement.GetDirection;

        Vector2 divePos = new Vector2(direction, jumpPower);

        //impulse 순간적으로 한순간에 만 힘을 주는 모드
        body.AddForce(divePos * divePower, ForceMode2D.Impulse);
    }

    private void Hang(Collision2D hangTarget)
    {
        // 이미 매달려 있다면 중복 생성 방지
        foreach(var hand in hands)
        {
            hand.enabled = true;
        }

        targetCar = hangTarget.gameObject;

        carBase = targetCar.GetComponent<CarBase>();

        //타겟의 Rigidbody 연결
        Rigidbody2D targetBody = targetCar.GetComponentInParent<Rigidbody2D>();

        if (targetBody == null)
        {
            return;
        }

        if (targetBody != null)
        {
            foreach (var hand in hands)
            {
                hand.connectedBody = targetBody;

                //앵커 설정 (충돌 지점을 매달리는 위치로 지정)
                hand.autoConfigureConnectedAnchor = false;
                hand.connectedAnchor = targetBody.transform.InverseTransformPoint(hangTarget.contacts[0].point);

                isHang = true;
            }
        }
    }

    private void DeactiveHang()
    {
        if(!isHang)
        {
            return;
        }

        if(carBase.CurrentSpeed > maxSpeed)
        {
            foreach (var hand in hands)
            {
                hand.enabled = false;
            }
        }
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if(hasDived)
        {
            //죽은게 아니라 그냥 레깅돌만
            zombieRagdoll.DieZombie();
            OffAnimation();
        }

        if ((hangLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            Hang(collision);
            zombieRagdoll.IngoreCar();
        }
    }

    private void ActivateAnimation()
    {
        zombieMovement.animator.SetTrigger("Dive");
        //여기서 레깅돌을 활성화 하는이유는, 애메션 에서 와의 바디 부위 위치와 똑같게
        //애니메이터가 끈 직후에도 이전의 속도나 물리적 충돌 정보가 꼬여서 래그돌이 이상하게 튀거나 고정되는 현상 방지
        zombieRagdoll.ActivateRagdoll();
    }

    private void OffAnimation()
    {
        zombieMovement.animator.enabled = false;
    }
}
