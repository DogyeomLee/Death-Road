using UnityEngine;

public class GameCarManager : MonoBehaviour
{ 
    [SerializeField] private CarBase[] cars;

    [SerializeField] private CarBase thisCar;

    private void Start()
    {
        // 씬 시작 시 인덱스를 가져와서 차량 활성화
        ActivateSelectedCar();
        Debug.Log(thisCar); 
    }

    private void ActivateSelectedCar()
    {
        int index = SendCarData.Instance.SelectedCarIndex;

        // 모든 차량 끄기
        foreach (var car in cars)
        {
            car.gameObject.SetActive(false);
        }

        // 인덱스에 맞는 차량만 켜기
        if (index >= 0 && index < cars.Length)
        {
            cars[index].gameObject.SetActive(true);
        }
    }
}
