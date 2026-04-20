using Unity.Mathematics;
using UnityEngine;
public class PlayerMoney : MonoBehaviour
{
    public static PlayerMoney Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] private int money;

    public int GetMoney => money;

    public void SpendMoney(int cost)
    {
        if(cost <= 0)
        {
            Debug.Log("올바르지 않는 가격");
            return;
        }

        money -= cost;
        money = Mathf.Clamp(money, 0, int.MaxValue);
    }
}
