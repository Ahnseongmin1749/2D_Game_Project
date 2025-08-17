using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private GameObject permanentPrefab;

    void Awake()
    {
        /*// 이미 Permanent가 씬에 존재하면 중복 생성 방지
        if (GameObject.Find("Permanent") != null)
        {
            Destroy(gameObject);
            return;
        }

        // Permanent 없으면 프리팹 생성
        var obj = Instantiate(permanentPrefab);
        obj.name = "Permanent"; // 이름 지정 필수
        DontDestroyOnLoad(obj);*/
    }
}