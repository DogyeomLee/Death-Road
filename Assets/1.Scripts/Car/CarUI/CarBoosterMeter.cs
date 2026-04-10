using UnityEngine;

public class CarBoosterMeter : MonoBehaviour
{
    //[Header("UI 참조")]
    //public GameObject needle;

    //[Header("설정")]
    //public float minAngle;  
    //public float maxAngle; 
    //public float smoothTime = 0.1f; 

    //public CarBase car;
    //private float velocity = 0.2f; 
    //private float currentAngle;

    //private void Start()
    //{
    //    if (car == null)
    //        car = FindFirstObjectByType<CarBase>();

    //    currentAngle = maxAngle;
    //}

    //void Update()
    //{
    //    if (car == null || needle == null) return;

    //    float boosterNormalized = Mathf.Clamp01(car.currentBooster / car.booster);
    //    float targetAngle = Mathf.Lerp(minAngle, maxAngle, boosterNormalized);
    //    currentAngle = Mathf.SmoothDamp(currentAngle, targetAngle, ref velocity, smoothTime);
    //    needle.transform.localRotation = Quaternion.Euler(0, 0, currentAngle);
    //}
}
