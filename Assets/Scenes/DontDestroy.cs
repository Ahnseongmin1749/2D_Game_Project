using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    /*void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }*/

    private static DontDestroy instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // 중복 생성 방지
        }
    }
}
