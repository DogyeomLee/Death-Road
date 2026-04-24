using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cheat : MonoBehaviour
{
    [SerializeField]
    private InputAction CheatAction;

    public static event Action OnShowMeTheMoney;

    private void OnEnable()
    {
        CheatAction.Enable();
    }

    private void OnDisable()
    {
        CheatAction.Disable();
    }

    private void Update()
    {
        if (CheatAction != null)
        {
            if(CheatAction.WasPressedThisFrame())
            {
                ShowMeTheMoney();
            }
        }
    }

    //매개변수는 그냥 이벤트 와 타입을 맞추기 위해 선언하넛
    private void ShowMeTheMoney()
    {
        PlayerMoney.Instance.AddMoney(1000);
        OnShowMeTheMoney?.Invoke();
    }
}
