using UnityEngine;

public class CarBooster : MonoBehaviour
{
    [Header("차량 부스터 설정")]
    [SerializeField] private float maxBooster;
    [SerializeField] private float maxLevelBooster;
    [SerializeField] private float boosterPower;
    [SerializeField] private float currentBooster;
    [SerializeField] private float boosterpendAmount;
    [SerializeField] private BoosterAnimation boosterAnimation;

    public Rigidbody2D rb;

    //UI 용 접근제한자
    public float MaxLevelBooster => maxLevelBooster;    
    public float MaxBooster => maxBooster;
    public float CurrentBooster => currentBooster;
    
    //부스터 사용 유무
    public bool IsOutOfBooster => currentBooster <= 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentBooster = maxBooster;
    }

    /// <summary>
    /// 부스터 전용 
    /// </summary>
    /// <param name = "isOnBooster"> 부스터 입력 유무</param>
    public void Booster(bool isOnBooster)
    {
        if(IsOutOfBooster)
        {
            Debug.Log("부스터 를 다 소모 하였습니다");
            return;
        }

        if(isOnBooster)
        {
            //부스터 애니메이션 발동
            boosterAnimation.PlayAnimation();

            //부스터 발동
            rb.AddRelativeForce(Vector2.right * boosterPower, ForceMode2D.Force);
            //연료 소모
            currentBooster -= boosterpendAmount * Time.fixedDeltaTime;

            currentBooster = Mathf.Max(0, currentBooster);
        }
    }

    public void UpgradeBooster(float upBooster)
    {
        if (upBooster <= 0)
        {
            Debug.Log("업그레이드 될 연료의 값은 0 이나 음수가 될수 없습니다.");
            return;
        }

        maxBooster = upBooster;
    }
}
