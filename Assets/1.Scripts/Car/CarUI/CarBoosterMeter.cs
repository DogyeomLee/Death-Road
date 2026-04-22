using UnityEngine;

public class CarBoosterMeter : MonoBehaviour
{
    [Header("UI 참조")]
    public GameObject needle;

    [Header("설정")]
    public float minAngle;
    public float maxAngle;
    public float smoothTime = 0.1f;

    public CarBase car;
    private float velocity = 0.2f;
    private float currentAngle;

    private void Start()
    {
        car = FindFirstObjectByType<CarBase>();

        currentAngle = maxAngle;
    }
    private void FindCar()
    {
        car = FindFirstObjectByType<CarBase>();
    }

    void Update()
    {
        if (!car.gameObject.activeSelf)
        {
            FindCar();
        }

        float boosterNormalized = Mathf.Clamp01(car.CurrentBooster / car.MaxLevelBooster);
        float targetAngle = Mathf.Lerp(minAngle, maxAngle, boosterNormalized);
        currentAngle = Mathf.SmoothDamp(currentAngle, targetAngle, ref velocity, smoothTime);
        needle.transform.localRotation = Quaternion.Euler(0, 0, currentAngle);
    }
}
