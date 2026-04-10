using System;
using UnityEngine;

[RequireComponent(typeof(CarMovement))]
[RequireComponent(typeof(CarInput))]
[RequireComponent(typeof(CarFuel))]
[RequireComponent(typeof(CarBooster))]

//모든 자동차 컴포넌트들을 관리하고, 논리적으로 구현하는 스크립트
public class CarBase : MonoBehaviour
{
    //참조할 컴포넌트들
    private CarMovement carMovement;
    private CarInput carInput;
    private CarFuel carFuel;
    private CarBooster carBooster;

    //차량 이펙트를 위한 이벤트,
    public event Action<float> OnDirectionChanged;

    public event Action OnStop;

    public float Movement => carInput.Movement;

    private bool isStopped;

    private void Awake()
    {
        carMovement = GetComponent<CarMovement>();
        carInput = GetComponent<CarInput>();
        carFuel = GetComponent<CarFuel>();
        carBooster = GetComponent<CarBooster>();
    }

    private void FixedUpdate()
    {
        Move();
        Booster();
    }

    private void Update()
    {
        Fuel();
    }

    private void Move()
    {
        //연료 없을시 이동 불가.
        if (!carFuel.IsOutOfFuel)
        {
            //물리 기반 이동은 FixedUpdate 에서.
            carMovement.Move(carInput.Movement);
            //이벤트 발생
            OnDirectionChanged?.Invoke(carInput.Movement);
            carMovement.Rotation(carInput.Rotation);
        }
    }

    private void Booster()
    {
        //부스터용량 없을시 부스터 불가
        if (!carBooster.IsOutOfBooster)
        {
            //물리 기반 이동..
            carBooster.Booster(carInput.IsOnBooster);
        }
    }

    private void Fuel()
    {
        if (carFuel.IsOutOfFuel) // 1. 일단 연료가 없는지 확인
        {
            if (!isStopped) // 2. "이미 멈췄다고 이벤트를 보냈었나?" 확인
            {
                isStopped = true; // 3. "이제 보낼 거니까 멈춤 상태로 기록해!"
                OnStop?.Invoke(); // 4. "멈췄다!"고 이벤트 발송
            }
            return; // 5. 연료가 없으니 아래 연료 소모 로직은 실행 안 함
        }
        // 6. 연료가 있다면 다시 달릴 수 있는 상태이므로 false로 리셋
        isStopped = false;

        //차량 이동시 연료 소모
        if (carInput.Movement != 0)
        {
            carFuel.UseFuel();
        }
    }
}