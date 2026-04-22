using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("카메라 설정")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 2, -10);
    [SerializeField] private float smoothSpeed = 0.2f;

    [Header("2D 줌 설정")]
    [SerializeField] private Camera cam; // 카메라 컴포넌트
    [SerializeField] private float minSize = 5f; // 평소 시야 (작게)
    [SerializeField] private float maxSize = 15f; // 속도 빠를 때 (넓게)
    [SerializeField] private float zoomSpeed = 0.05f;

    private CarBase targetCar;

    private void Start()
    {
        FindCar();
    }

    private void FindCar()
    {
        if (target == null)
        {
            GameObject car = GameObject.FindGameObjectWithTag("Car");

            if (car != null)
            {
                target = car.transform;
                targetCar = car.GetComponent<CarBase>();
            }
        }
    }

    private void LateUpdate()
    {
        if (target == null || targetCar == null)
        {
            FindCar();
        }

        //위치 이동
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // 속도에 따라 사이즈 결정 (속도가 0일 때 minSize, 최대 속도일 때 maxSize)
        float targetSize = Mathf.Lerp(minSize, maxSize, Mathf.Abs(targetCar.CurrentSpeed) / 100f);

        //부드럽게 사이즈 변경
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, zoomSpeed);
    }
}
