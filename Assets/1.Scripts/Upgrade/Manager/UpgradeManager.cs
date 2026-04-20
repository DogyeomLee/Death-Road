using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeData
{
    public string name;         
    public int currentLevel;    
    public int maxLevel;        
    public float upValue;   
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

    public int GetNextCost()
    {
        // 현재 레벨이 리스트 인덱스 범위 내에 있는지 확인
        if (currentLevel < levelCosts.Length)
        {
            return levelCosts[currentLevel];
        }

        return -1; // 최대 레벨 도달 시 -1 반환
    }

    public int GetNextValue()
    {
        if(currentLevel < levelValues.Length)
        {
            return levelValues[currentLevel];
        }

        return -1;
    }
}

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        AddUpdradePart("Gun", new UpgradeData("Gun", 1, new int[] { 100 },new int[] { 10 }));
        AddUpdradePart("Fuel", new UpgradeData("Fuel", 10, new int[] { 100 }, new int[] { 10 }));
        AddUpdradePart("Booster", new UpgradeData("Booster", 3, new int[] { 100 }, new int[] { 10 }));
        AddUpdradePart("Engine", new UpgradeData("Engine", 3, new int[] { 100 }, new int[] { 10 }));
        AddUpdradePart("Bumper", new UpgradeData("Bumper", 1, new int[] { 100 }, new int[] { 10 }));
    }

    public Dictionary<string, UpgradeData> upgradeData = new Dictionary<string, UpgradeData>();

    //UI 와 사운드 를 위한 이벤트
    public event Action OnUpgrade;
    //실패 전용 UI, 사운드 를 위한 이벤트
    public event Action OnFailUpgrade;


    private void AddUpdradePart(string key, UpgradeData data)
    {
        //중복 방지.
        if(!upgradeData.ContainsKey(key))
        {
            upgradeData.Add (key, data);
        }
    }

    public void Upgrade(string key)
    {
        //따로 순회해 찾지 않고 키로 바로 데이터를 가져온다.
        if (upgradeData.TryGetValue(key, out UpgradeData data))
        {
            if(data.currentLevel < data.maxLevel && PlayerMoney.Instance.money > data.GetNextCost())
            {
                OnUpgrade?.Invoke();

                data.currentLevel++;
                data.upValue = data.GetNextValue();
            }
            else
            {
                OnFailUpgrade?.Invoke();    
            }
        }
    }
}
