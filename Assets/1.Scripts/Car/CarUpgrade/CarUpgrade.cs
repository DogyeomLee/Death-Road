using UnityEngine;

public class CarUpgrade : MonoBehaviour
{
    [SerializeField] private GameObject bumper;
    [SerializeField] private GameObject boosterObj;
    [SerializeField] private GameObject gun;

    public void UpgradeFuel(CarFuel fuel)
    {
        if(UpgradeManager.Instance.upgradeData.TryGetValue("Fuel", out var data))
        {
            fuel.UpgradeFuel(data.GetValueByLevel());
        }
    }

    public void UpgradeBooster(CarBooster booster)
    {
        if (UpgradeManager.Instance.upgradeData.TryGetValue("Booster", out var data))
        {
            //1일 때만, 따로 게임 오브젝트를 활성화
            if(data.currentLevel == 1)
            {
                boosterObj.gameObject.SetActive(true);
            }

            booster.UpgradeBooster(data.GetValueByLevel());
        }
    }

    public void UpgradeBumper()
    {
        if (UpgradeManager.Instance.upgradeData.TryGetValue("Bumper", out var data))
        {
            //범퍼 업글이 됬으면
            if(data.currentLevel == 1)
            {
                bumper.gameObject.SetActive(true);
            }
        }
    }

    public void UpgradeGun()
    {
        if(UpgradeManager.Instance.upgradeData.TryGetValue("Gun", out var data))
        {
            if(data.currentLevel == 1)
            {
                gun.gameObject.SetActive(true);
            }
        }
    }

    public void UpgradeEngine(CarMovement engine)
    {
        if (UpgradeManager.Instance.upgradeData.TryGetValue("Engine", out var data))
        {
            engine.UpgradeEngine(data.GetValueByLevel());
        }
    }
}
