using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CarSpeedMeter : MonoBehaviour
{
    [Header("UI 참조")]
    public GameObject needle;

    [Header("각도 설정")]
    public float speedDivide = 10f;
    public float minAngle;    // 속도 0일 때 (예: 120도)
    public float maxAngle; // 최대 속도일 때 (예: -120도, 음수면 시계방향)
    public float smoothTime = 0.1f; // 바늘의 떨림이나 부드러운 움직임 조절

    public CarBase car;
    private float velocity = 0.2f; // SmoothDamp용 변수
    private float currentAngle;

    private void Start()
    {
        car = FindFirstObjectByType<CarBase>();

        currentAngle = minAngle;
    }
    private void FindCar()
    {
        if (car == null)
        {
            car = FindFirstObjectByType<CarBase>();
        }
    }

    void Update()
    {
        if (car == null)
        {
            FindCar();
        }

        // 제 차의 속도 범위를 확인하고 그에 맞춰 나눈다
        float normalizedSpeed = Mathf.Clamp01(car.CurrentSpeed / speedDivide);

        // 목표 각도 계산
        float targetAngle = Mathf.Lerp(minAngle, maxAngle, normalizedSpeed);

        // SmoothDamp의 smoothTime을 조절해 바늘의 반응 속도를 맞춘다
        currentAngle = Mathf.SmoothDamp(currentAngle, targetAngle, ref velocity, smoothTime);

        // 회전 적용 
        needle.transform.localRotation = Quaternion.Euler(0, 0, currentAngle);
    }
}
