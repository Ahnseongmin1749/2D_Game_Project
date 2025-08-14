using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using static UnityEngine.UI.Image;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Player_State : MonoBehaviour
{
    public bool isTopdown;
    Rigidbody2D rigid;
    Player_Platformer plyaer_Platformer;
    public GameObject Weapon_Manager;
    Weapon_Manager Weapon_Manager_s;
    public int weapon_index;
    public float hp;
    public float total_exp;
    float exp;
    public float speed;
    public float atk;
    public float def;
    public GameObject attackZone;
    GameObject weapon;
    SpriteRenderer w_spriteRenderer;
    public GameObject die_ui;
    public TextMeshProUGUI die_text;

    public Transform PlayerHPBar;
    public Transform PlayerEXPBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        Weapon_Manager_s = Weapon_Manager.GetComponent<Weapon_Manager>();
        plyaer_Platformer= GetComponent<Player_Platformer>();

        weapon = Weapon_Manager_s.Visible_Weapon(weapon_index);
        w_spriteRenderer = weapon.GetComponent<SpriteRenderer>();

        
    }

    private void Start()
    {
        
        float plus_atk = Weapon_Manager_s.Get_Weapon_Atk(weapon_index);
        atk += plus_atk;
        isTopdown = true;
        AttackZoneSetting();
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

        
        float hpRatio = hp / 100f;
        PlayerHPBar.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Image>().fillAmount = hpRatio;

        //Exp_Calculating(total_exp);
        float expRatio = total_exp / 100f;
        PlayerEXPBar.GetChild(0).GetComponent<UnityEngine.UI.Image>().fillAmount = expRatio;

        
        Player_Die();
    }


    /*(int, float) Exp_Calculating(float total_exp)
    {
        int[] levelThresholds = {0, 100, 200, 300, 400 }; // 레벨 시작 경험치
        for (int i = 0; i < levelThresholds.Length; i++)
        {
            if (total_exp <= levelThresholds[i])
                exp = total_exp - levelThresholds[i];
                return (i + 1, exp); // i가 레벨
        }
        
    }*/

    void Player_Die()
    {
        if (hp <= 0)
        {
            die_ui.SetActive(true);
            die_text.text = "You Die";
            Time.timeScale = 0;
        }
    }

    public void Player_Retry()
    {
        die_ui.SetActive(false);
        hp = 100;
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }


    public void RigidSetting()
    {
        if (isTopdown)
        {
            rigid.mass = 1;
            rigid.linearDamping = 0;
            rigid.angularDamping = 0.05f;
            rigid.gravityScale = 0;
        }
        else if (!isTopdown)
        {
            rigid.mass = 0.8f;
            rigid.linearDamping = 2;
            rigid.angularDamping = 0.05f;
            rigid.gravityScale = 3;
        }
    }

    public void AttackZoneSetting()
    {
        if (isTopdown)
        {
            attackZone.SetActive(false);
            
        }


        else if (!isTopdown)
        {
            attackZone.SetActive(true);
            
        }
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
