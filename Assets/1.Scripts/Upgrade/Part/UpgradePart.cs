using UnityEngine;
using UnityEngine.UI;

public class UpgradePart : MonoBehaviour
{
    [SerializeField] private string keyName;
    [SerializeField] private string description;
    [SerializeField] private Sprite[] icon;
    [SerializeField] private RawImage upgradeLevelUI;
    [SerializeField] private Texture[] currentLevelImage;

    [SerializeField] private int[] levelCosts;
    [SerializeField] private int[] levelValues;

    private void Start()
    {
        CheckCurrentLevel();
        SetCostByLevel();
        SetValueByLevel();
    }

    public void OnClickUpgradeButton()
    {
        UpgradeManager.Instance.Upgrade(keyName);
    }

    private void CheckCurrentLevel()
    {
        if (UpgradeManager.Instance.upgradeData.TryGetValue(keyName, out var upgradeData))
        {
            //미리 지정해둔 레벨 표시 이미지를 현재의 이미지 배열에 맞게 할당.
            upgradeLevelUI.texture = currentLevelImage[upgradeData.currentLevel];
        }
    }

    //레벨에 따라 다른 비용 설정
    private void SetCostByLevel()
    {

    }

    //레벨에 따라 다른 업글 수치 설정
    private void SetValueByLevel()
    {

    }
}
