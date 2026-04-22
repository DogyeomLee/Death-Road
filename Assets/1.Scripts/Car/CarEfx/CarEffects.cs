using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CarEffects : MonoBehaviour
{
    [Header("기본 참조 요소들")]
    [SerializeField] private CarBase car;
    [SerializeField] private Light2D backLight;

    [Header("기본 소리")]
    [SerializeField] private AudioSource carSource;
    [SerializeField] private AudioClip engineSound;
    [SerializeField] private AudioClip engineStopSound;

    private void Awake()
    {
        car = GetComponent<CarBase>();
    }
    private void Start()
    {
        carSource.clip = engineSound;
        carSource.loop = true;
        carSource.Play();
    }

    private void OnEnable() // 이벤트 연결
    {
        car.OnDirectionChanged += HandleDirectionLight;
        CarBase.OutOfFuel += HandleStopSound;
    }

    private void OnDisable() // 이벤트 해제 (메모리 누수 방지)
    {
        car.OnDirectionChanged -= HandleDirectionLight;
        CarBase.OutOfFuel -= HandleStopSound;
    }

    private void Update()
    {
        HandleRPMSound(car.Movement);    
    }

    void HandleRPMSound(float movement)
    {
        float currentPitch = carSource.pitch;

        if (movement != 0)
        {
            currentPitch += 0.5f * Time.deltaTime;
            currentPitch = Mathf.Clamp(currentPitch, 1f, 2f);
        }
        else
        {
            currentPitch -= 0.5f * Time.deltaTime;
            currentPitch = Mathf.Max(currentPitch, 1f);
        }

        carSource.pitch = currentPitch;
    }

    void HandleDirectionLight(float movement)
    {
        if (movement < 0)
        {
            backLight.intensity = 3f;
        }
        else
        {
            backLight.intensity = 1f;
        }
    }

    private void HandleStopSound()
    {
        carSource.Stop();
        carSource.PlayOneShot(engineStopSound);
    }
}
