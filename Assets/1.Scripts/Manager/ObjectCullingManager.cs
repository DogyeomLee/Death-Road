using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectCullingManager : MonoBehaviour
{
    //카메라 오브젝트에 붙여서, 카메라의 스크립트를 따라간다.
    [SerializeField]
    private List<GameObject> targetObjects; // 끄고 켤 오브젝트들을 여기에 드래그해서 넣으세요
    [SerializeField]
    private float activeDistance = 707f; // 이 거리 안에 들어오면 켜짐


    void Start()
    {
        // "Object" 태그를 가진 모든 오브젝트를 배열로 가져옴
        GameObject[] foundObjects = GameObject.FindGameObjectsWithTag("Object");

        // 배열을 리스트로 변환하여 사용
        targetObjects = new List<GameObject>(foundObjects);

        // Update 대신 코루틴 호출 (성능 최적화)
        StartCoroutine(CullingRoutine());
    }

    private System.Collections.IEnumerator CullingRoutine()
    {
        // 검사 주기 (0.2초마다 검사하면 충분합니다)
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            foreach (var obj in targetObjects)
            {
                float sqrDistance = (this.transform.position - obj.transform.position).sqrMagnitude;

                // 3. 미리 계산된 sqrActiveDistance와 비교
                if (sqrDistance < activeDistance)
                {
                    if (!obj.activeSelf) obj.SetActive(true);
                }
                else
                {
                    if (obj.activeSelf) obj.SetActive(false);
                }
            }
            yield return wait;
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, activeDistance);
    }
}
