using UnityEngine;

public class CarUpgrade : MonoBehaviour
{
    [Header("차량 인덱스")]
    [SerializeField] private int carIndex; // [중요] 인스펙터에서 차량 번호(0, 1, 2...) 설정

    [Header("시각적 업그레이드 요소")]
    [SerializeField] private GameObject bumper;
    [SerializeField] private GameObject boosterObj;
    [SerializeField] private GameObject gun;

    // 공통 헬퍼 메서드: 딕셔너리 접근 로직 중복 제거
    private bool TryGetUpgradeData(string key, out UpgradeData data)
    {
        data = null;

        // 해당 차량ID로 데이터가 있는지 확인
        if (UpgradeManager.Instance.carUpgradeData.TryGetValue(carIndex, out var carUpgrades))
        {
            // 해당 키(항목)로 데이터가 있는지 확인
            return carUpgrades.TryGetValue(key, out data);
        }
        return false;
    }

    public bool HasBooster()
    {
        if (TryGetUpgradeData("Booster", out var data))
        {
            return data.currentLevel >= 1;
        }
        return false;
    }

    public void UpgradeFuel(CarFuel fuel)
    {
        if (TryGetUpgradeData("Fuel", out var data))
        {
            fuel.UpgradeFuel(data.GetValueByLevel());
        }
    }

    public void UpgradeBooster(CarBooster booster)
    {
        if (TryGetUpgradeData("Booster", out var data))
        {
            // 부스터 레벨이 1 이상이면 활성화
            if (data.currentLevel >= 1 && boosterObj != null)
            {
                boosterObj.SetActive(true);
            }
            booster.UpgradeBooster(data.GetValueByLevel());
        }
    }

    public void UpgradeBumper()
    {
        if (TryGetUpgradeData("Bumper", out var data))
        {
            // 범퍼 레벨이 1이면 활성화
            if (data.currentLevel == 1 && bumper != null)
            {
                bumper.SetActive(true);
            }
        }
    }

    public void UpgradeGun()
    {
        if (TryGetUpgradeData("Gun", out var data))
        {
            // 건 레벨이 1이면 활성화
            if (data.currentLevel == 1 && gun != null)
            {
                gun.SetActive(true);
            }
        }
    }

    public void UpgradeEngine(CarMovement engine)
    {
        if (TryGetUpgradeData("Engine", out var data))
        {
            engine.UpgradeEngine(data.GetValueByLevel());
        }
    }
}
