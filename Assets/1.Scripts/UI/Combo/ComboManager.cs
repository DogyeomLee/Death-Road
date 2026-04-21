using TMPro;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    [Header("ÄÞº¸ ¼³Á¤")]
    [SerializeField] private GameObject ComboText;
    [SerializeField] private TMP_Text text;
    [SerializeField] private int currentComboScore;
    [SerializeField] private float comboTime;
    [SerializeField] private string[] strings;

    private void OnEnable()
    {
        ZombieBase.OnDestoryForUI += AddCombo;
    }

    private void OnDisable()
    {
        ZombieBase.OnDestoryForUI -= AddCombo;
    }

    private void AddCombo()
    {
        if(ComboText.activeSelf)
        {
            SetFalse();
        }

        int randomString = Random.Range(0, strings.Length);
        ComboText.SetActive(true);
        text.text = strings[randomString];
        Time.timeScale = 0.5f;

        Invoke("SetFalse", comboTime);
    }
    private void SetFalse()
    {
        Time.timeScale = 1;
        ComboText.SetActive(false);
    }
}
