using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct GhostData //구조체로 메모리 상에서 성능 유리. 복사 또한 편하게
{
    public float time;
    public Vector2 position;
    public float rotation;

    //생성자로 초기화 한다.
    public GhostData(float t, Vector2 pos, float rot)
    {
        time = t;
        position = pos;
        rotation = rot;
    }
}

public class GhostManager : MonoBehaviour
{
    public static GhostManager Instance; // 매니저에 접근할 수 있도록 Instance 변수 추가

    private void Awake()
    {
        // 이미 인스턴스가 존재한다면, 지금 생성된 이 오브젝트는 파괴
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // 처음 생성되는 것이라면 인스턴스로 지정하고 파괴 방지
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [Header("차량 이미지 설정")]
    [SerializeField]
    private SpriteRenderer thisCar;
    [SerializeField]
    private SpriteRenderer wheel1;
    [SerializeField]
    private SpriteRenderer wheel2;
    [SerializeField] 
    private CarBase car;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Transform thisGun;
    [SerializeField]
    private Transform thisBooster;
    [SerializeField]
    private Transform thisBumper;
    [SerializeField]
    private Transform gun;
    [SerializeField]
    private Transform booster;
    [SerializeField]
    private Transform bumper;

    // 데이터는 외부 에서 받아옴
    public List<GhostData> playData;

    private float recordTimer = 0f; // 기록용 타이머
    private float playTimer = 0f;   // 재생용 타이머

    private int currentIndex = 0;

    public bool isPlaying = false;

    private List<GhostData> currentRecording = new List<GhostData>(); // 지금 움직임을 저장 중
    public List<GhostData> savedGhostData = new List<GhostData>();    // 이전 기록(고스트로 재생할 데이터)

    [Header("기록 임계값")]
    public float recordInterval = 0.1f; // 0.1초마다 기록

    private float lastRecordTime = -1f; // 마지막 기록 시간 추적

    private bool hasChecked = false;


    private void Update()
    {
        PlayRecord();

        if (GameStateManager.Instance.GetCurrentState == GameState.Play)
        {
            if (hasChecked)
            {
                return;
            }

            ResetRecordingSession();

            //이전 기록(savedGhostData)을 재생 시작
            StartPlayback();
            // 새로운 기록 시작을 위해 현재 기록 초기화
            currentRecording.Clear();

            hasChecked = true;
        }
        else
        {
            if (hasChecked)
            {
                // 현재 기록 중이던 데이터를 저장된 고스트 데이터로 복사
                savedGhostData = new List<GhostData>(currentRecording);

                // 현재 기록 초기화
                currentRecording.Clear();
                isPlaying = false;
                hasChecked = false;
            }
        }
    }
    public void SetTargetCar(CarBase newCar)
    {
        this.car = newCar;
        // 여기서 필요한 스프라이트 등 정보를 갱신
        this.spriteRenderer = car.GetComponent<SpriteRenderer>();
        thisCar.sprite = spriteRenderer.sprite;
        wheel1.sprite = car.transform.Find("BackWheel").GetComponent<SpriteRenderer>().sprite;
        wheel2.sprite = car.transform.Find("FrontWheel").GetComponent<SpriteRenderer>().sprite;

        gun = car.transform.Find("Gun");
        booster = car.transform.Find("Booster");
        bumper = car.transform.Find("Bumper");

        thisGun.gameObject.SetActive(gun.gameObject.activeSelf);
        thisBooster.gameObject.SetActive(booster.gameObject.activeSelf);
        thisBumper.gameObject.SetActive(bumper.gameObject.activeSelf);
    }

    private void FixedUpdate()
    {
        if (GameStateManager.Instance.GetCurrentState == GameState.Play)
        {
            RecordGhost();
        }
    }

    private void RecordGhost()
    {
        recordTimer += Time.fixedDeltaTime;

        // 시작점 보장
        if (recordTimer - lastRecordTime >= recordInterval)
        {
            // 데이터 기록
            currentRecording.Add(new GhostData(recordTimer, car.transform.position, car.transform.eulerAngles.z));

            //이렇게 하면, if 문에서 두 시간사이의 초 를 가져올수있다.
            lastRecordTime = recordTimer;
        }
    }

    public void StartPlayback()
    {
        if (savedGhostData == null || savedGhostData.Count < 2)
        {
            return;
        }

        // 여기서만 딱 한 번 복사하고 초기화합니다.
        playData = new List<GhostData>(savedGhostData);
        playTimer = 0f;
        currentIndex = 0;
        isPlaying = true;
    }
    public void ResetRecordingSession()
    {
        recordTimer = 0f;
        lastRecordTime = -1f; // 이게 중요합니다! 0보다 작게 설정해야 바로 다음 프레임에 기록됩니다.
        currentRecording.Clear();
    }

    private void PlayRecord()
    {

        //차량 고스트의 데이터가 없으면 X
        if (!isPlaying || playData == null || playData.Count < 2)
        {
            thisCar.gameObject.SetActive(false);
            return;
        }

        if(!thisCar.gameObject.activeSelf)
        {
            thisCar.gameObject.SetActive(true);
        }

        playTimer += Time.deltaTime;

        //현재 시간보다 미래에 있는 데이터 찾기
        while (currentIndex < playData.Count - 2 && playData[currentIndex + 1].time < playTimer)
        {
            currentIndex++;
        }

        //끝까지 왔으면 끄기.
        if (currentIndex >= playData.Count - 1)
        {
            isPlaying = false;
            return;
        }

        //보간(Interpolation) 수행
        GhostData start = playData[currentIndex];
        GhostData end = playData[currentIndex + 1];

        float duration = end.time - start.time;
        float ratio = (playTimer - start.time) / duration;

        //Vector2.Lerp와 Mathf.LerpAngle로 부드럽게 보간
        transform.position = Vector2.Lerp(start.position, end.position, ratio);
        transform.rotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(start.rotation, end.rotation, ratio));
    }
}