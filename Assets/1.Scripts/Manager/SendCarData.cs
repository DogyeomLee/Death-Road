using UnityEngine;

public class SendCarData : MonoBehaviour
{
    public static SendCarData Instance { get; private set; }

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

    public int SelectedCarIndex { get; set; }

    public bool IsJeepUnLock { get; set; }
    public bool IsTruckUnLock { get; set; }

    private void Start()
    {
        SelectedCarIndex = 0;
    }

}
