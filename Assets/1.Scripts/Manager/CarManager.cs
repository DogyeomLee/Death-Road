using Unity.VisualScripting;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] carPanels;

    [SerializeField]
    private GameObject jeepLockPanel;
    [SerializeField]
    private GameObject truckLockPanel;

    private void OnEnable()
    {
        RepairButton.ChangeCarPanel += UpdateUI;
    }

    private void OnDisable()
    {
        RepairButton.ChangeCarPanel -= UpdateUI;  
    }

    private void Start()
    {
        GetCurrentCar();
        UpdateUI();

        if(SendCarData.Instance.IsJeepUnLock)
        {
            Destroy(jeepLockPanel);
        }
        if (SendCarData.Instance.IsTruckUnLock)
        {
            Destroy(truckLockPanel);
        }
    }

    private void GetCurrentCar()
    {
        for (int i = 0; i < carPanels.Length; i++)
        {
            if (carPanels[i].activeSelf)
            {
                // 인덱스를 저장합니다.
                SendCarData.Instance.SelectedCarIndex = i;
            }
        }
    }

    private void UpdateUI()
    {

        foreach (var panel in carPanels)
        {
            panel.SetActive(false);
        }

        int index = RepairButton.CurrentIndex;

        if (index >= 0 && index < carPanels.Length)
        {
            carPanels[index].SetActive(true);
        }    

        GetCurrentCar();
    }
}
