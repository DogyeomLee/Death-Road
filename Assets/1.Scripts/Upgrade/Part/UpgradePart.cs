using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePart : MonoBehaviour
{
    [SerializeField] private int carIndex;
    [SerializeField] private string keyName;
    [SerializeField] private string description;
    [SerializeField] private Sprite[] icon;
    [SerializeField] private RawImage upgradeLevelUI;
    [SerializeField] private Texture[] currentLevelImage;

    [SerializeField] private TMP_Text cost;

    [SerializeField] private CarBase car;

    private void OnEnable()
    {
        UpgradeManager.Instance.OnSuccessUpgrade += HandleUpgradeEvent;
    }

    private void OnDisable()
    {
        UpgradeManager.Instance.OnSuccessUpgrade -= HandleUpgradeEvent;
    }

    private void Start()
    {
        UpdateUI();
    }

    public void OnClickUpgradeButton()
    {
        UpgradeManager.Instance.Upgrade(keyName, carIndex);
    }

    private void HandleUpgradeEvent(int upgradedCarIndex)
    {
        // 만약 업데이트된 차가 내 차(carIndex)와 같다면 UI를 갱신
        if (upgradedCarIndex == this.carIndex)
        {
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        // 이중 딕셔너리 구조에 맞게 접근
        if (UpgradeManager.Instance.carUpgradeData.TryGetValue(carIndex, out var carUpgrades))
        {
            if (carUpgrades.TryGetValue(keyName, out var upgradeData))
            {
                // 비용 텍스트 갱신
                int nextCost = upgradeData.GetCostByLevel();
                cost.text = (nextCost == -1) ? "MAX" : nextCost.ToString();

                // 이미지 갱신
                if (upgradeLevelUI != null && currentLevelImage.Length > upgradeData.currentLevel)
                {
                    upgradeLevelUI.texture = currentLevelImage[upgradeData.currentLevel];
                }
            }
        }
    }
}
