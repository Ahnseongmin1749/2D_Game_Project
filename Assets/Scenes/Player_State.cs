using UnityEngine;

public class Player_State : MonoBehaviour
{
    public GameObject Weapon_Manager;
    Weapon_Manager Weapon_Manager_s;
    public int weapon_index;
    public float hp;
    public float speed;
    public float atk;
    public float def;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Weapon_Manager_s = Weapon_Manager.GetComponent<Weapon_Manager>();
    }

    private void Start()
    {
        float plus_atk = Weapon_Manager_s.Get_Weapon(weapon_index);
        atk += plus_atk;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
