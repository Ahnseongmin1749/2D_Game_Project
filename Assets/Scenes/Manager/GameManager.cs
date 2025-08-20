using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    public static GameManager Instance
    {
        get
        {

            if (instance == null)
            {
                return null;
            }
            return instance;

        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // �ߺ� ���� ����
        }
    }

    public GameObject Player;
    public int weapon_index;
    public float Hp;
    public float total_exp;
    public float speed;
    public float atk;
    public float def;

    private void Start()
    {
        Player = GameObject.Find("Player");
    }

}
