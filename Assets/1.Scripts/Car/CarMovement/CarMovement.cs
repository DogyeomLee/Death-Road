using System;
using UnityEngine;

//이 스크립트는 자동차 의 기본 이동, 회전 을 구현하는 스크립트다.
public class CarMovement : MonoBehaviour
{
    [Header("차량 기본 설정")]
    [SerializeField] private float speed; 
    [SerializeField] private float maxTorque;
    [SerializeField] private float rotationSpeed;
    
    private CarBase car;

    public WheelJoint2D BackWheel;
    public Rigidbody2D rb;


    //차량이 멈췄는지 확인하는 변수
    public float stopTimer = 0f;
    public static event Action<float> OnStopTime;

    //public float CurrentSpeed
    //{
    //    get { return rb.linearVelocity.magnitude; }
    //}
    //위 코드와 똑같이 작동하지만, 훨씬 간결하게 표현 가능하다.
    public float CurrentSpeed => rb.linearVelocity.magnitude;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        car = GetComponent<CarBase>();
    }

    private void OnEnable()
    {
        CarBase.OutOfFuel += Stop;
    }

    private void OnDisable()
    {
        CarBase.OutOfFuel -= Stop;
    }

    /// <summary>
    /// 이동 전용 
    /// </summary>
    /// <param name = "movementInput"> 이동 전용 입력 값</param>
    public void Move(float movementInput)
    {
        if (movementInput != 0)
        {
            //바퀴를 돌리 모터를 킴
            BackWheel.useMotor = true;
            //복사본을 가져옴
            JointMotor2D motor = BackWheel.motor;
            // - 음수, 시계 방향으로 돎
            motor.motorSpeed = -movementInput * speed;
            motor.maxMotorTorque = maxTorque;
            BackWheel.motor = motor;
        }
        else
        {
            BackWheel.useMotor = false;
        }
    }

    /// <summary>
    /// 회전 전용 
    /// </summary>
    /// <param name = "rotationInput"> 회전 전용 입력 값</param>
    public void Rotation(float rotationInput)
    {
        if (rotationInput != 0)
        {
            rb.AddTorque(-rotationInput * rotationSpeed);
        }
    }

    /// <summary>
    /// 자동차 정지 
    /// </summary>
    ///     /// <param name = "isOutOfFuel"> 기름 유무</param>
    public void Stop()
    {
        BackWheel.useMotor = false;
        rb.linearDamping = 3.0f;
    }

    public void UpgradeEngine(float upSpeed)
    {
        if (upSpeed <= 0)
        {
            Debug.Log("업그레이드 될 엔진의 값은 0 이나 음수가 될수 없습니다.");
            return;
        }

        speed = upSpeed;
    }

    public bool CheckStopCondition()
    {
        if (Mathf.Abs(CurrentSpeed) < 0.3f)
        {
            stopTimer += Time.deltaTime;

            OnStopTime?.Invoke(stopTimer);

            if (stopTimer >= 3.0f)
            {
                return true;
            }
        }
        else
        {
            // 움직이면 초기화
            if (stopTimer != 0)
            {
                stopTimer = 0;
                OnStopTime?.Invoke(0f); // 0을 보내서 UI 초기화 요청
                return false;
            }
        }
        return false;
    }
}
