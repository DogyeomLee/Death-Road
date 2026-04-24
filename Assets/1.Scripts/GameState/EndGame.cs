using UnityEngine;

public class EndGame : MonoBehaviour
{
    [SerializeField] private LayerMask CarLayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((CarLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            GameSceneManager.Instance.LoadSceneAsyncByName("EndScene");
        }
    }
}
