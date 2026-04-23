using System;
using UnityEngine;

public class RepairButton : MonoBehaviour
{
    public static int CurrentIndex { get; private set; }

    public static event Action ChangeCarPanel;

    public static event Action BuyCarAction;
    public static event Action NeedMoneyAction;

    [SerializeField] private int cost;

    private int maxPanelCount = 3; // 실제 패널 개수에 맞게 설정

    public void NextButton()
    {
        // 0 -> 1 -> 2 -> 0 식으로 순환하게 만듭니다.
        CurrentIndex = (CurrentIndex + 1) % maxPanelCount;
        ChangeCarPanel?.Invoke();
    }

    public void PrevButton()
    {
        // 이전 버튼은 음수가 되지 않도록 처리
        CurrentIndex = (CurrentIndex - 1 + maxPanelCount) % maxPanelCount;
        ChangeCarPanel?.Invoke();
    }

    public void OnClickGoButton()
    {
        GameSceneManager.Instance.LoadSceneAsyncByName("GameScene");
    }

    public void BuyCar()
    {
        if (PlayerMoney.Instance.GetMoney >= cost)
        {
            PlayerMoney.Instance.SpendMoney(cost);

            BuyCarAction?.Invoke();
          
            transform.parent.gameObject.SetActive(false);

            //jeep 차량을 삿을떄
            if(CurrentIndex == 1)
            {
                SendCarData.Instance.IsJeepUnLock = true;
            }
            //Truck 을 삿을떈
            if(CurrentIndex == 2)
            {
                SendCarData.Instance.IsTruckUnLock = true;
            }
        }
        else
        {
            NeedMoneyAction?.Invoke();
        }
    }

    public void UndoButton()
    {
        UpgradeManager.Instance.UndoUpgrade();
    }
}
