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

    public int money = 50000;

}
