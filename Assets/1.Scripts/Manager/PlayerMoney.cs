using UnityEngine;
public class PlayerMoney : MonoBehaviour
{
    public int gold = 500;

    public bool SpendGold(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            return true;
        }
        else
        {
            return false;
        }
    }
}
