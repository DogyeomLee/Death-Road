using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[System.Serializable]
public class UpgradeData
{
    public string name;         
    public int currentLevel;    
    public int maxLevel;        
    public float value;   
    public int cost;
    [SerializeField] private int[] levelCosts;
    [SerializeField] private int[] levelValues;

    public UpgradeData(string name, int maxLevel, int[] costs, int[] values)
    {
        this.name = name;
        this.currentLevel = 0;
        this.maxLevel = maxLevel;
        this.levelCosts = costs;
        this.levelValues = values;
    }

    public int GetCostByLevel()
    {
        // 현재 레벨이 리스트 인덱스 범위 내에 있는지 확인
        if (currentLevel < levelCosts.Length)
        {
            return levelCosts[currentLevel];
        }

        return -1; // 최대 레벨 도달 시 -1 반환
    }

    public float GetValueByLevel()
    {
        if(currentLevel < levelValues.Length)
        {
            return levelValues[currentLevel];
        }

        return -1;
    }
}
public struct UndoAction
{
    public int carIndex;
    public string key;
    public int refundCost; // 환불할 금액을 저장

    public UndoAction(int carIndex, string key, int refundCost)
    {
        this.carIndex = carIndex;
        this.key = key;
        this.refundCost = refundCost;
    }
}

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    // [차량ID][업그레이드항목명] = 업그레이드 데이터
    public Dictionary<int, Dictionary<string, UpgradeData>> carUpgradeData = new Dictionary<int, Dictionary<string, UpgradeData>>();

    // 되돌리기 스택 생성
    private Stack<UndoAction> undoStack = new Stack<UndoAction>();

    public event Action OnFailUpgrade;
    public event Action<int> OnSuccessUpgrade; // 어떤 차가 업그레이드 되었는지 ID 전달

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 테스트용: 인덱스 0번과 1번 차량 데이터 초기화
        InitializeCarData(0);
        InitializeJeepData(1);
        InitializeTruckData(2);
    }

    private void InitializeCarData(int carIndex)
    {
        if (!carUpgradeData.ContainsKey(carIndex))
        {
            carUpgradeData[carIndex] = new Dictionary<string, UpgradeData>();

            // 각 차마다 데이터를 독립적으로 생성
            carUpgradeData[carIndex].Add("Gun", new UpgradeData("Gun", 1, new int[] { 2000 }, new int[] { 10 }));
            carUpgradeData[carIndex].Add("Fuel", new UpgradeData("Fuel", 10, new int[] { 100, 150, 200, 300, 450, 700, 900, 1200, 1600, 2000 }, new int[] { 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 70 }));
            carUpgradeData[carIndex].Add("Booster", new UpgradeData("Booster", 3, new int[] { 1000, 3000, 5000 }, new int[] { 0, 10, 30, 50 }));
            carUpgradeData[carIndex].Add("Engine", new UpgradeData("Engine", 3, new int[] { 1000, 1500, 2000 }, new int[] { 2000, 2000, 3500, 5000 }));
            carUpgradeData[carIndex].Add("Bumper", new UpgradeData("Bumper", 1, new int[] { 2000 }, new int[] { 10 }));
        }
    }
    private void InitializeJeepData(int carIndex)
    {
        if (!carUpgradeData.ContainsKey(carIndex))
        {
            carUpgradeData[carIndex] = new Dictionary<string, UpgradeData>();

            // 각 차마다 데이터를 독립적으로 생성
            carUpgradeData[carIndex].Add("Gun", new UpgradeData("Gun", 1, new int[] { 2500 }, new int[] { 10 }));
            carUpgradeData[carIndex].Add("Fuel", new UpgradeData("Fuel", 10, new int[] { 150, 200, 250, 350, 550, 800, 1000, 1300, 1800, 2500 }, new int[] { 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 70 }));
            carUpgradeData[carIndex].Add("Booster", new UpgradeData("Booster", 3, new int[] { 1500, 4000, 6000 }, new int[] { 0, 10, 30, 50 }));
            carUpgradeData[carIndex].Add("Engine", new UpgradeData("Engine", 3, new int[] { 100, 500, 1000 }, new int[] { 2000, 2000, 3000, 4000 }));
            carUpgradeData[carIndex].Add("Bumper", new UpgradeData("Bumper", 1, new int[] { 3500 }, new int[] { 10 }));
        }
    }
    private void InitializeTruckData(int carIndex)
    {
        if (!carUpgradeData.ContainsKey(carIndex))
        {
            carUpgradeData[carIndex] = new Dictionary<string, UpgradeData>();

            // 각 차마다 데이터를 독립적으로 생성
            carUpgradeData[carIndex].Add("Gun", new UpgradeData("Gun", 1, new int[] { 3000 }, new int[] { 10 }));
            carUpgradeData[carIndex].Add("Fuel", new UpgradeData("Fuel", 10, new int[] { 100, 150, 200, 300, 450, 700, 900, 1200, 1600, 2000 }, new int[] { 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 70 }));
            carUpgradeData[carIndex].Add("Booster", new UpgradeData("Booster", 3, new int[] { 2000, 4500, 7000 }, new int[] { 0, 10, 30, 50 }));
            carUpgradeData[carIndex].Add("Engine", new UpgradeData("Engine", 3, new int[] { 1000, 2500, 3000 }, new int[] { 2000, 2000, 3000, 4000 }));
            carUpgradeData[carIndex].Add("Bumper", new UpgradeData("Bumper", 1, new int[] { 4000 }, new int[] { 10 }));
        }
    }

    public void Upgrade(string key, int targetCarIndex)
    {
        if (carUpgradeData.TryGetValue(targetCarIndex, out var carData))
        {
            if (carData.TryGetValue(key, out UpgradeData data))
            {
                int costToPay = data.GetCostByLevel(); 

                if (data.currentLevel < data.maxLevel && PlayerMoney.Instance.GetMoney >= data.GetCostByLevel())
                {
                    undoStack.Push(new UndoAction(targetCarIndex, key, costToPay));

                    data.currentLevel++;
                    data.value = data.GetValueByLevel();
                    PlayerMoney.Instance.SpendMoney(costToPay);

                    OnSuccessUpgrade?.Invoke(targetCarIndex); // 해당 차량 ID만 발송
                }
                else 
                {
                    OnFailUpgrade?.Invoke();
                }
            }
        }
    }
    public void UndoUpgrade()
    {
        if (undoStack.Count > 0)
        {
            UndoAction lastAction = undoStack.Pop();

            if (carUpgradeData.TryGetValue(lastAction.carIndex, out var carData))
            {
                if (carData.TryGetValue(lastAction.key, out UpgradeData data))
                {
                    // 레벨 감소 및 데이터 복구
                    data.currentLevel--;
                    data.value = data.GetValueByLevel();

                    // 돈 환불
                    PlayerMoney.Instance.AddMoney(lastAction.refundCost);

                    Debug.Log($"{lastAction.key} 업그레이드 취소! {lastAction.refundCost} 환불 완료.");
                    OnSuccessUpgrade?.Invoke(lastAction.carIndex);
                }
            }
        }
        else
        {
            Debug.Log("되돌릴 업그레이드가 없습니다.");
        }
    }

    public void ClearUndoStack()
    {
        undoStack.Clear();
        Debug.Log("Undo 스택이 초기화되었습니다.");
    }
}
