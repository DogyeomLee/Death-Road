using UnityEngine;

public class CarBase : MonoBehaviour
{
    [Header("차량 기본 설정")]
    public float speed;
    public float maxTorque;
    public float movement;
    public float rotationInput;
    public float rotationSpeed; // 차체 회전 강도

    [Header("차량 연료 설정")]
    public float fuel;
    public float currentFuel;
    public float fuelSpendAmount;

    [Header("차량 부스터 설정")]
    public float booster;
    public float boosterPower;
    public float currentBooster;
    public float boosterpendAmount;

    [Header("차량 업그레이드 설정")]
    public CarUpgrade engineLevel;
    [SerializeField]
    int currentEngineLevel;
    public CarUpgrade tireLevel;
    [SerializeField]
    int currentTireLevel;
    public CarUpgrade boosterLevel;
    [SerializeField]
    int currentBoosterLevel;
    public CarUpgrade bumperLevel;
    [SerializeField]
    int currentBumperLevel;
    public CarUpgrade gunLevel;
    [SerializeField]
    int currentGunLevel;
    public CarUpgrade fuelLevel;
    [SerializeField]
    int currentFuelLevel;

    public float CurrentSpeed
    {
        get { return rb.linearVelocity.magnitude; }
    }

    public WheelJoint2D BackWheel;
    public Rigidbody2D rb;

    protected virtual void Update()
    {
        movement = Input.GetAxisRaw("Vertical");
        rotationInput = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        if (currentFuel > 0)
        {
            Move();
            HandleBooster();
            HandleRotation();
        }
        else
        {
            currentFuel = 0;
            BackWheel.useMotor = false;
        }
    }

    public void ApplyUpgradeStats()
    {
        CheckUpgradeLevel();
    }

    void Move()
    {
        if (movement != 0)
        {
            BackWheel.useMotor = true;
            JointMotor2D motor = BackWheel.motor;
            motor.motorSpeed = -movement * speed;
            motor.maxMotorTorque = maxTorque;
            BackWheel.motor = motor;

            // 연료 소모
            currentFuel -= fuelSpendAmount * Time.fixedDeltaTime;
        }
        else
        {
            BackWheel.useMotor = false;
        }
    }

    void HandleBooster()
    {
        if (Input.GetKey(KeyCode.Space) && currentBooster > 0)
        {
            rb.AddRelativeForce(Vector2.right * boosterPower, ForceMode2D.Force);
            currentBooster -= boosterpendAmount * Time.fixedDeltaTime;
        }
    }

    void HandleRotation()
    {
        if (rotationInput != 0)
        {
            rb.AddTorque(-rotationInput * rotationSpeed);
        }
    }

    public void CheckUpgradeLevel()
    {
        currentEngineLevel = engineLevel.currentUpgradeLevel;
        currentTireLevel = tireLevel.currentUpgradeLevel;
        currentBoosterLevel = boosterLevel.currentUpgradeLevel;
        currentBumperLevel = bumperLevel.currentUpgradeLevel;
        currentGunLevel = gunLevel.currentUpgradeLevel;
        currentFuelLevel = fuelLevel.currentUpgradeLevel;
    }
}