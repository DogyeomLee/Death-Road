using UnityEngine;

public class CarFuelMeter : MonoBehaviour
{
    [Header("UI 참조")]
    public GameObject needle;

    [Header("설정")]
    public float minAngle;    // 속도 0일 때 (예: 120도)
    public float maxAngle; // 최대 속도일 때 (예: -120도, 음수면 시계방향)
    public float smoothTime = 0.1f; // 바늘의 떨림이나 부드러운 움직임 조절

    public CarBase car;
    private float velocity = 0.2f; // SmoothDamp용 변수
    private float currentAngle;

    private void Start()
    {
        if (car == null)
        {
            car = FindFirstObjectByType<CarBase>();
        }

        currentAngle = maxAngle;
    }

    void Update()
    {
        if (car == null || needle == null)
        {
            return; 
        }

        float fuelNormalized = Mathf.Clamp01(car.CurrentFuel / car.MaxLevelFuel);
        float targetAngle = Mathf.Lerp(minAngle, maxAngle, fuelNormalized);
        currentAngle = Mathf.SmoothDamp(currentAngle, targetAngle, ref velocity, smoothTime);
        needle.transform.localRotation = Quaternion.Euler(0, 0, currentAngle);
    }
}
