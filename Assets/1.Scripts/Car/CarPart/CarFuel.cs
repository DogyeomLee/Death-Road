using UnityEngine;

public class CarFuel : MonoBehaviour
{
    [Header("차량 연료 설정")]
    [SerializeField] private float maxFuel;
    [SerializeField] private float currentFuel;
    [SerializeField] private float fuelSpendAmount;
    
    //UI 용 접근제한자
    public float MaxFuel => maxFuel;
    public float CurrentFuel => currentFuel;

    //이동을 제한하기 위한 불 타입의 변수
    public bool IsOutOfFuel => currentFuel <= 0;

    private void Awake()
    {
        //현재의 기름 값을 초기화.
        currentFuel = maxFuel;
    }

    //외부에서 안전하게 maxFuel 값을 set
    /// <summary>
    /// 기름 업그레이드
    /// </summary>
    /// <param name = "upFuel"> 늘어난 기름의 용량 값</param>
    public void UpgradeFuel(float upFuel)
    {
        if(upFuel <= 0)
        {
            Debug.Log("업그레이드 될 연료의 값은 0 이나 음수가 될수 없습니다.");
            return;
        }

        maxFuel += upFuel;
    }

    public void UseFuel()
    {
        if(IsOutOfFuel)
        {
            Debug.Log("연료가 다 소모 되었습니다");
            return;
        }
        //연료 소모
        currentFuel -= fuelSpendAmount * Time.deltaTime;

        //currentFuel 을 max 를 통해 최솟값 0 최댓값 currentFuel 로 지정.
        currentFuel = Mathf.Max(0, currentFuel);
    }
}
