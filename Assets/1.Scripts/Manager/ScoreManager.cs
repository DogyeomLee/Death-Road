using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [Header("거리 설정")]
    [SerializeField]
    private Transform startPoint;
    [SerializeField]
    private Transform endPoint;
    [SerializeField]
    private float totalDistance;
    [SerializeField]
    private TMP_Text scoreText;

    [Header("위치 설정")]
    [SerializeField]
    private Slider distanceUI;
    [SerializeField] 
    private Slider carSlider;

    [Header("차량 설정")]
    [SerializeField] private CarBase car;

    [Header("카운트 다운 설정")]
    [SerializeField]
    private TMP_Text countDownText;
    private float remainingTime;

    [Header("점수 설정")]
    private int dieScore;
    private int destroyScore;
    [SerializeField]
    private GameObject scorePanel;

    [Header("점수 패널 설정")]
    [SerializeField]
    private TMP_Text distanceText;
    [SerializeField]
    private TMP_Text dieText;
    [SerializeField]
    private TMP_Text destroyText;
    [SerializeField]
    private TMP_Text totalText;

    private void OnEnable()
    {
        CarBase.OnStop += ScoreToMoney;
        CarMovement.OnStopTime += CountDown;

        ZombieBase.OnDieForUI += DieUI;
        ZombieBase.OnDestoryForUI += DestroyUI;
    }

    private void OnDisable()
    {
        CarBase.OnStop -= ScoreToMoney;
        CarMovement.OnStopTime -= CountDown;

        ZombieBase.OnDieForUI -= DieUI;
        ZombieBase.OnDestoryForUI -= DestroyUI;
    }

    private void Awake()
    {
        totalDistance = endPoint.position.x - startPoint.position.x;

        distanceUI.value = totalDistance;

        scorePanel.SetActive(false);
    }

    private void Start()
    {
        car = FindFirstObjectByType<CarBase>();
    }

    private void FindCar()
    {
        if (car == null)
        {
            car = FindFirstObjectByType<CarBase>();
        }
    }

    void Update()
    {
        if (car == null)
        {
            FindCar();
        }

        carSlider.value = car.transform.position.x / totalDistance;

        scoreText.text = (carSlider.value * totalDistance).ToString("F1") + "M";
    }

    private void ScoreToMoney()
    {
        int scoreForDistance = (int)(carSlider.value * totalDistance) * 10;
        int scoreForDie = dieScore * 5;
        int scoreForDestroy = destroyScore * 10;

        int totalScore = scoreForDistance + scoreForDie + scoreForDestroy;

        Debug.Log($"정산 시작: 거리({scoreForDistance}) + 사망({scoreForDie}) + 파괴({scoreForDestroy}) = 총합({totalScore})");
        PlayerMoney.Instance.AddMoney(totalScore);

        OnScorePanel();
    }

    private void CountDown(float time)
    {
        // time이 0보다 크면 카운트다운 중임
        if (time > 0f)
        {
            // 3초에서 흐른 시간을 빼서 남은 시간 표시
            remainingTime = 3.0f - time;
            countDownText.text = remainingTime.ToString("F1"); // 소수점 첫째자리까지
        } 
        else
        {
            countDownText.text = "";
        }
    }

    private void DieUI()
    {
        dieScore++;
    }

    private void DestroyUI()
    {
        destroyScore++;
    }

    private void OnScorePanel()
    {
        scorePanel.SetActive(true);

        int distScore = (int)(carSlider.value * totalDistance) * 10;
        int dieScoreText = (dieScore * 5);
        int destoryScoreText = (destroyScore * 10);

        distanceText.text = distScore.ToString();
        dieText.text = dieScoreText.ToString();
        destroyText.text = destoryScoreText.ToString();

        totalText.text = (distScore + dieScoreText + destoryScoreText).ToString();
    }

    public void GotoRepairShop()
    {
        GameSceneManager.Instance.LoadSceneByName("RepairScene");
    }
}
