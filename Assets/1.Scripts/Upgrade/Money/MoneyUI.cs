using System;
using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    [Header("기본 세팅")]
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text upgradeText;
    [Header("성공 유무")]
    [SerializeField] private string successString;
    [SerializeField] private string failString;

    [SerializeField] private AudioClip[] moneySFX;

    private void Start()
    {
        moneyText.text = PlayerMoney.Instance.GetMoney.ToString();
    }

    private void OnEnable()
    {
        // 이제 SuccessUpgrade가 int를 받는 함수이므로 연결 가능합니다.
        UpgradeManager.Instance.OnSuccessUpgrade += SuccessUpgrade;
        UpgradeManager.Instance.OnFailUpgrade += FailUpgrade;

        RepairButton.BuyCarAction += BuyCar;
        RepairButton.NeedMoneyAction += FailUpgrade;
    }

    private void OnDisable()
    {
        UpgradeManager.Instance.OnSuccessUpgrade -= SuccessUpgrade;
        UpgradeManager.Instance.OnFailUpgrade -= FailUpgrade;

        RepairButton.BuyCarAction -= BuyCar;
        RepairButton.NeedMoneyAction -= FailUpgrade;
    }

    // 수정: int 매개변수를 받도록 변경 (전달받은 carIndex를 사용하지 않더라도 선언은 필수)
    private void SuccessUpgrade(int carIndex)
    {
        upgradeText.text = successString;
        SoundManager.Instance.PlaySfxOneShot(moneySFX[0], 1);
        moneyText.text = PlayerMoney.Instance.GetMoney.ToString();

        CancelInvoke("EmptyText"); // 중복 실행 방지를 위해 기존 예약 취소
        Invoke("EmptyText", 1.0f);
    }

    private void BuyCar()
    {
        upgradeText.text = successString;
        SoundManager.Instance.PlaySfxOneShot(moneySFX[0], 1);
        moneyText.text = PlayerMoney.Instance.GetMoney.ToString();

        CancelInvoke("EmptyText"); // 중복 실행 방지를 위해 기존 예약 취소
        Invoke("EmptyText", 1.0f);
    }

    private void FailUpgrade()
    {
        upgradeText.text = failString;
        SoundManager.Instance.PlaySfxOneShot(moneySFX[1], 1);

        CancelInvoke("EmptyText");
        Invoke("EmptyText", 1.0f);
    }

    private void EmptyText()
    {
        upgradeText.text = " ";
    }
}
