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

    //인덱스 0 에는 성공 1 에는 실패 .
    [SerializeField] private AudioClip[] moneySFX;

    private void Start()
    {
        moneyText.text = PlayerMoney.Instance.GetMoney.ToString();
    }

    private void OnEnable()
    {
        UpgradeManager.Instance.OnSuccessUpgrade += SuccessUpgrade;
        UpgradeManager.Instance.OnFailUpgrade += FailUpgrade;
    }

    private void OnDisable()
    {
        UpgradeManager.Instance.OnSuccessUpgrade -= SuccessUpgrade;
        UpgradeManager.Instance.OnFailUpgrade -= FailUpgrade;
    }

    private void SuccessUpgrade()
    {
        upgradeText.text = successString;
        SoundManager.Instance.PlaySfxOneShot(moneySFX[0]);
        moneyText.text = PlayerMoney.Instance.GetMoney.ToString();

        Invoke("EmptyText", 1.0f);
    }

    private void FailUpgrade()
    {
        upgradeText.text = failString;
        SoundManager.Instance.PlaySfxOneShot(moneySFX[1]);

        Invoke("EmptyText", 1.0f);
    }

    private void EmptyText()
    {
        upgradeText.text = " ";
    }
}
