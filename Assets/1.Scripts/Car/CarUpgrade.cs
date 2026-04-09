using TMPro;
using UnityEngine;

public class CarUpgrade : MonoBehaviour
{
    public CarBase car;
    public PlayerMoney playerMoney;
    public TextMeshProUGUI currentMoney;
    public GameObject[] upgradeMaxLevel;
    public int currentUpgradeLevel { get; private set; }
    public int upgradeCost;
    public int costRIse;
    public TextMeshProUGUI costText;

    private void Awake()
    {
        for(int i = 0; i < upgradeMaxLevel.Length; i++)
        {
            upgradeMaxLevel[i].gameObject.SetActive(false);
        }
        costText.text = upgradeCost.ToString();
    }

    private void Start()
    {
        car = FindFirstObjectByType<CarBase>();
        playerMoney = FindFirstObjectByType<PlayerMoney>();

        currentMoney.text = playerMoney.gold.ToString();
    }
    public void Upgrage()
    {
        if(currentUpgradeLevel < upgradeMaxLevel.Length)
        {
            if(playerMoney.gold >= upgradeCost)
            {
                playerMoney.gold -= upgradeCost;
                upgradeMaxLevel[currentUpgradeLevel].gameObject.SetActive(true);
                currentUpgradeLevel++;
                upgradeCost *= costRIse;
                costText.text = upgradeCost.ToString();
                currentMoney.text = playerMoney.gold.ToString();
            }
            else
            {
                Debug.Log("돈 이 부족합니다.");
            }
        }
        else
        {
            Debug.Log("업그레이드 를 최고치 까지 하였습니다.");
        }

        car.CheckUpgradeLevel();
    }
}
