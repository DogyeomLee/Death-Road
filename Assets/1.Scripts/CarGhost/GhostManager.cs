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
    //public static GhostManager Instance { get; private set; }

    //private void Awake()
    //{
    //    if (Instance != null && Instance != this)
    //    {
    //        Destroy(gameObject);
    //        return;
    //    }

    //    Instance = this;

    //    DontDestroyOnLoad(gameObject);
    //}

    [SerializeField] 
    private CarBase car;

    // 데이터는 외부 에서 받아옴
    public List<GhostData> playData;

    private float timer = 0f;
    private int currentIndex = 0;

    public bool isPlaying = false;

    public List<GhostData> ghostData = new List<GhostData>();

    [Header("기록 임계값")]
    public float recordInterval = 0.1f; // 0.1초마다 기록

    private float lastRecordTime = -1f; // 마지막 기록 시간 추적


    void Update()
    {
        PlayRecord();
    }
    // 재생을 시작할 때 호출
    public void StartPlayback()
    {
        playData = new List<GhostData>(ghostData); // 데이터 복사 (딱 한 번)
        timer = 0f;
        currentIndex = 0;
        isPlaying = true;

    }

    private void FixedUpdate()
    {
        RecordGhost();
    }

    private void RecordGhost()
    {
        timer += Time.fixedDeltaTime;

        // 시작점 보장
        if (timer - lastRecordTime >= recordInterval)
        {
            // 데이터 기록
            ghostData.Add(new GhostData(timer, car.transform.position, car.transform.eulerAngles.z));

            //이렇게 하면, if 문에서 두 시간사이의 초 를 가져올수있다.
            lastRecordTime = timer;
        }
    }

    private void PlayRecord()
    {

        //차량 고스트의 데이터가 없으면 X
        if (!isPlaying || playData == null || playData.Count < 2)
        {
            return;
        }

        timer += Time.deltaTime;

        //현재 시간보다 미래에 있는 데이터 찾기
        while (currentIndex < playData.Count - 2 && playData[currentIndex + 1].time < timer)
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
        float ratio = (timer - start.time) / duration;

        //Vector2.Lerp와 Mathf.LerpAngle로 부드럽게 보간
        transform.position = Vector2.Lerp(start.position, end.position, ratio);
        transform.rotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(start.rotation, end.rotation, ratio));
    }
}