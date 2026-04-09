using UnityEngine;

public class CarSpeedMeter : MonoBehaviour
{
    [Header("UI 참조")]
    public GameObject needle;

    [Header("각도 설정")]
    public float maxSpeed = 10f;
    public float minAngle;    // 속도 0일 때 (예: 120도)
    public float maxAngle; // 최대 속도일 때 (예: -120도, 음수면 시계방향)
    public float smoothTime = 0.1f; // 바늘의 떨림이나 부드러운 움직임 조절

    public CarBase car;
    private float velocity = 0.2f; // SmoothDamp용 변수
    private float currentAngle;

    private void Start()
    {
        if (car == null)
            car = FindFirstObjectByType<CarBase>();

        currentAngle = minAngle;
    }

    void Update()
    {
        if (car == null || needle == null) return;

        // 1. 실제 차의 속도 범위를 확인하고 그에 맞춰 나눕니다.
        // 만약 속도가 너무 작게 나온다면 maxSpeed 값을 아주 작게(예: 5 or 10) 설정해보세요.
        float normalizedSpeed = Mathf.Clamp01(car.CurrentSpeed * 10 / maxSpeed);

        // 2. 목표 각도 계산
        float targetAngle = Mathf.Lerp(minAngle, maxAngle, normalizedSpeed);

        // 3. SmoothDamp의 smoothTime을 조절해 바늘의 반응 속도를 맞춥니다.
        currentAngle = Mathf.SmoothDamp(currentAngle, targetAngle, ref velocity, smoothTime);

        // 4. 회전 적용 (Quaternion 방식이 가장 안정적입니다)
        needle.transform.localRotation = Quaternion.Euler(0, 0, currentAngle);
    }
}
