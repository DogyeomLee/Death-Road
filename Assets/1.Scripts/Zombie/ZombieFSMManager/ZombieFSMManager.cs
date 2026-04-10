using UnityEngine;
using UnityEngine.UIElements;

public enum ZombieState
{
    Idle,
    Chase,
    Attack,
    hang,
    Dead
}

public class ZombieFSMManager : MonoBehaviour
{
    [Header("FSM 값 세팅")]
    [SerializeField] private ZombieState currentState = ZombieState.Idle;
    [SerializeField] private float chaseDistance;
    [SerializeField] private float attackDistance;
    [SerializeField] private float hangDistance;

    public ZombieState CurrentState => currentState;

    private void ChangeState(ZombieState nextState)
    {
        if(currentState == nextState)
        {
            return;
        }

        currentState = nextState;
        //이벤트 구현시 발동 구간// 여기
    }

    /// <summary>
    /// 타켓과의 거리에 따라 상태 변환
    /// </summary>
    /// <param name = "targetCar"> 목표 자동차</param>
    public void ChangeStateByCondition(Transform targetCar)
    {
        //sqrMagnitude는 루트 계산을 하지 않아 magnitude나 Vector3.Distance보다 연산 속도가 빠름
        float sqrDist = (transform.position - targetCar.position).sqrMagnitude;

        float chaseSqr = chaseDistance * chaseDistance;
        float attackSqr = attackDistance * attackDistance;
        float hangSqr = hangDistance * hangDistance;

        switch (currentState)
        {
            case ZombieState.Idle:
                if(sqrDist <= chaseSqr)
                {
                    ChangeState(ZombieState.Chase);
                }
                break;
            case ZombieState.Chase:
                if(sqrDist <= attackSqr)
                {
                    ChangeState(ZombieState.Attack);
                }
                //약간의 여유분을 줌으로써 도망 거리를 표현
                else if(sqrDist > chaseSqr * 1)
                {
                    ChangeState(ZombieState.Idle);
                }
                else if(sqrDist <=  hangSqr )
                {
                    ChangeState(ZombieState.hang);
                }
                    break;
            case ZombieState.Attack:
                if (sqrDist > attackSqr)
                {
                    ChangeState(ZombieState.Chase);
                }
                break;
            case ZombieState.hang:
                if(sqrDist > hangSqr)
                {
                    ChangeState(ZombieState.Chase);
                }
                break;
        }
    }
}
