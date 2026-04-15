using System;
using UnityEngine;

[RequireComponent(typeof(CarMovement))]
[RequireComponent(typeof(CarInput))]
[RequireComponent(typeof(CarFuel))]
[RequireComponent(typeof(CarBooster))]

//모든 자동차 컴포넌트들을 관리하고, 논리적으로 구현하는 스크립트
public class CarBase : MonoBehaviour
{
    public static CarBase Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        //참조
        if (carMovement == null && carInput == null && carFuel == null && carBooster == null)
        {
            carMovement = GetComponent<CarMovement>();
            carInput = GetComponent<CarInput>();
            carFuel = GetComponent<CarFuel>();
            carBooster = GetComponent<CarBooster>();
        }
    }

    [Header("참조할 컴포넌트들")]
    [SerializeField] private CarMovement carMovement;
    [SerializeField] private CarInput carInput;
    [SerializeField] private CarFuel carFuel;
    [SerializeField] private CarBooster carBooster;

    //차량 이펙트를 위한 이벤트,
    public event Action<float> OnDirectionChanged;

    public event Action OnStop;

    public float Movement => carInput.Movement;

    public float CurrentSpeed => carMovement.CurrentSpeed;

    private bool isStopped;


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
        if (carFuel.IsOutOfFuel) //일단 연료가 없는지 확인
        {
            if (!isStopped) //이벤트를 보냈나 확인
            {
                isStopped = true; // 멈춤 상태로 기록
                OnStop?.Invoke(); //멈췄다고 이벤트 발송
            }
            return; // 아래 연료 소모 로직은 실행 안 함
        }
        isStopped = false;

        //차량 이동시 연료 소모
        if (carInput.Movement != 0)
        {
            carFuel.UseFuel();
        }
    }

    //범퍼, 파괴
    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDestroyable damagable = collision.gameObject.GetComponent<IDestroyable>();

        if (damagable != null)
        {
            //차량의 속도에 따라 파괴 유무 , 파괴 흩날라기 정도.,
            damagable.Destory(carMovement.CurrentSpeed, carMovement.CurrentSpeed * 0.5f);
        }
    }
}