using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CarEffects : MonoBehaviour
{
    public CarBase car;
    public AudioSource engineSound;
    public Light2D backLight;

    private void Awake()
    {
        car = GetComponent<CarBase>();
    }
    void Update()
    {
        CarSound();

        CarLight();
    }

    void CarSound()
    {
        // 속도에 따라 엔진 소리 피치 조절
        engineSound.pitch = 0.5f + car.CurrentSpeed / 20f;
    }

    void CarLight()
    {
        if (car.movement < 0)
        {
            backLight.intensity = 3f;
        }
        else
        {
            backLight.intensity = 1f;
        }
    }
}
