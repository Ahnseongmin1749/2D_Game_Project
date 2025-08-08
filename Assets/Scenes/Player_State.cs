using UnityEngine;

public class Player_State : MonoBehaviour
{
    Player_Platformer plyaer_Platformer;
    public GameObject Weapon_Manager;
    Weapon_Manager Weapon_Manager_s;
    public int weapon_index;
    public float hp;
    public float speed;
    public float atk;
    public float def;

    GameObject weapon;
    SpriteRenderer w_spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Weapon_Manager_s = Weapon_Manager.GetComponent<Weapon_Manager>();
        plyaer_Platformer= GetComponent<Player_Platformer>();

        weapon = Weapon_Manager_s.Visible_Weapon(weapon_index);
        w_spriteRenderer = weapon.GetComponent<SpriteRenderer>();

    }

    private void Start()
    {
        
        float plus_atk = Weapon_Manager_s.Get_Weapon_Atk(weapon_index);
        atk += plus_atk;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vec;
        if (plyaer_Platformer.isrightlooking)
        {
            vec = new Vector3(0.5f, 0, 0);
            w_spriteRenderer.flipX = true;
        }
        else
        {
            vec = new Vector3(-0.5f, 0, 0);
            w_spriteRenderer.flipX = false;
        }
        weapon.transform.position = transform.position + vec;
    }
}

/*int prev_weapon_index = -1;

void Update()
{
    if (weapon_index != prev_weapon_index)
    {
        weapon = Weapon_Manager_s.Visible_Weapon(weapon_index);
        spriteRenderer = weapon.GetComponent<SpriteRenderer>();
        prev_weapon_index = weapon_index;
    }

    // 나머지 동일
}*/
