using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private GameObject permanentPrefab;

    void Awake()
    {
        /*// �̹� Permanent�� ���� �����ϸ� �ߺ� ���� ����
        if (GameObject.Find("Permanent") != null)
        {
            Destroy(gameObject);
            return;
        }

        // Permanent ������ ������ ����
        var obj = Instantiate(permanentPrefab);
        obj.name = "Permanent"; // �̸� ���� �ʼ�
        DontDestroyOnLoad(obj);*/
    }
}