using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePart : MonoBehaviour
{
    [SerializeField] private string keyName;
    [SerializeField] private string description;
    [SerializeField] private Sprite[] icon;
    [SerializeField] private RawImage upgradeLevelUI;
    [SerializeField] private Texture[] currentLevelImage;

    [SerializeField] private TMP_Text cost;

    [SerializeField] private CarBase car;

    private void OnEnable()
    {
        UpgradeManager.Instance.OnSuccessUpgrade += UpdateCost;
        UpgradeManager.Instance.OnSuccessUpgrade += UpdatecurrentLevel;
    }

    private void OnDisable()
    {
        UpgradeManager.Instance.OnSuccessUpgrade -= UpdateCost;
        UpgradeManager.Instance.OnSuccessUpgrade -= UpdatecurrentLevel;
    }

    private void Start()
    {
        if (UpgradeManager.Instance.upgradeData.TryGetValue(keyName, out var upgradeData))
        {
            cost.text = upgradeData.GetCostByLevel().ToString();
        }
    }

    public void OnClickUpgradeButton()
    {
        UpgradeManager.Instance.Upgrade(keyName);
    }

    private void UpdatecurrentLevel()
    {
        if (UpgradeManager.Instance.upgradeData.TryGetValue(keyName, out var upgradeData))
        {
            //미리 지정해둔 레벨 표시 이미지를 현재의 이미지 배열에 맞게 할당.
            upgradeLevelUI.texture = currentLevelImage[upgradeData.currentLevel];
        }
    }

    private void UpdateCost()
    {
        if(UpgradeManager.Instance.upgradeData.TryGetValue(keyName, out var upgradeData))
        {
            int nextCost = upgradeData.GetCostByLevel();

            cost.text = (nextCost == -1) ? "MAX" : nextCost.ToString();
        }
    }
}
