using System;
using UnityEngine;

[RequireComponent(typeof(CarMovement))]
[RequireComponent(typeof(CarInput))]
[RequireComponent(typeof(CarFuel))]
[RequireComponent(typeof(CarBooster))]
[RequireComponent(typeof(CarUpgrade))]

//모든 자동차 컴포넌트들을 관리하고, 논리적으로 구현하는 스크립트
public class CarBase : MonoBehaviour
{


    [Header("참조할 컴포넌트들")]
    [SerializeField] private CarMovement carMovement;
    [SerializeField] private CarInput carInput;
    [SerializeField] private CarFuel carFuel;
    [SerializeField] private CarBooster carBooster;
    [SerializeField] private CarUpgrade carUpgrade;

    //차량 이펙트를 위한 이벤트,
    public event Action<float> OnDirectionChanged;

    public static event Action OnStop;
    public event Action OutOfFuel;

    public float Movement => carInput.Movement;

    //UI 에서도 접근가능. 프로퍼티의 프로퍼티
    public float CurrentSpeed => carMovement.CurrentSpeed;
    public float CurrentFuel => carFuel.CurrentFuel;
    public float MaxFuel => carFuel.MaxFuel;
    public float MaxLevelFuel => carFuel.MaxLevelFuel;  
    public float CurrentBooster => carBooster.CurrentBooster;
    public float MaxBooster => carBooster.MaxBooster;
    public float MaxLevelBooster => carBooster.MaxLevelBooster;

    private bool hasBooster = false;

    private bool isStopped = false;

    private bool hasFuelEvent = false;

    private void Awake()
    {
        //참조
        if (carMovement == null || carInput == null || carFuel == null || carBooster == null || carUpgrade == null)
        {
            carMovement = GetComponent<CarMovement>();
            carInput = GetComponent<CarInput>();
            carFuel = GetComponent<CarFuel>();
            carBooster = GetComponent<CarBooster>();
            carUpgrade = GetComponent<CarUpgrade>();
        }
    }

    private void OnEnable()
    {
        if(UpgradeManager.Instance != null)
        {
            UpgradeManager.Instance.OnSuccessUpgrade += ApplyAllUpgrades;
        }
    }

    private void OnDisable()
    {
        if (UpgradeManager.Instance != null)
        {
            UpgradeManager.Instance.OnSuccessUpgrade -= ApplyAllUpgrades;
        }
    }
    private void Start()
    {
        if (GhostManager.Instance != null)
        {
            GhostManager.Instance.SetTargetCar(this);
        }

        if (carUpgrade.HasBooster())
        {
            hasBooster = true;
        }

        ApplyAllUpgrades();
    }

    private void FixedUpdate()
    {
        Move();

        if(hasBooster)
        {
            Booster();
        }
    }

    private void Update()
    {
        Fuel();

        if (isStopped)
        {
            return;
        }

        if (carMovement.CheckStopCondition())
        {
            isStopped = true;
          
            OnStop?.Invoke();
        }

        if(carFuel.IsOutOfFuel)
        {
            if(hasFuelEvent)
            {
                return;
            }

            OutOfFuel?.Invoke();
            hasFuelEvent = true;
        }
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
            damagable.Destroy(carMovement.CurrentSpeed, carMovement.CurrentSpeed * 0.5f);
        }
    }
    public void ApplyAllUpgrades()
    {
        carUpgrade.UpgradeGun();
        carUpgrade.UpgradeBumper();
        carUpgrade.UpgradeBooster(carBooster);
        carUpgrade.UpgradeEngine(carMovement);
        carUpgrade.UpgradeFuel(carFuel);
    }
}