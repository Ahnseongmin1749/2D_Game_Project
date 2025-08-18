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
    public float speed;
    public float atk;
    public float def;
    public GameObject attackZone;
    GameObject weapon;
    SpriteRenderer w_spriteRenderer;
    public GameObject die_ui;
    public TextMeshProUGUI die_text;
    public TextMeshProUGUI exp_text;
    public TextMeshProUGUI level_text;

    public Transform PlayerHPBar;
    public Transform PlayerEXPBar;

    public Animator animator;
    public RuntimeAnimatorController topDownAnimator;
    public RuntimeAnimatorController platformerAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        isTopdown = false;
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
        AttackZoneSetting();
        RigidSetting();

        //Switch();
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

        int level = Exp_Calculating(total_exp).Item1;
        float exp = Exp_Calculating(total_exp).Item2;


        exp_text.text = exp.ToString();
        level_text.text = level.ToString();

        float expRatio = exp / 100f;
        PlayerEXPBar.GetChild(0).GetComponent<UnityEngine.UI.Image>().fillAmount = expRatio;

        
        Player_Die();
    }


    (int, float) Exp_Calculating(float total_exp)
    {
        float current_exp;
        int level;
        if (total_exp >= 0 && total_exp < 100) { current_exp = total_exp; level = 1; }
        else if (total_exp < 200) { current_exp = total_exp - 100; level = 2; }
        else if (total_exp < 300) { current_exp = total_exp - 200; level = 3; }
        else if (total_exp < 400) { current_exp = total_exp - 300; level = 4; }
        else { current_exp = total_exp - 400; level = 5; }

        return (level, current_exp);
    }

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

    void Switch()
    {
        // 이후의 보스맵씬 키운 후 보수 필요
        SwitchToPlatformer();
    }
    public void SwitchToTopDown()
    {
        animator.runtimeAnimatorController = topDownAnimator;
        GetComponent<Player>().enabled = true;
        GetComponent<Player_Platformer>().enabled = false;
        isTopdown = true;
        RigidSetting();
        AttackZoneSetting();
    }

    public void SwitchToPlatformer()
    {
        animator.runtimeAnimatorController = platformerAnimator;
        GetComponent<Player>().enabled = false;
        GetComponent<Player_Platformer>().enabled = true;
        isTopdown = false;
        RigidSetting();
        AttackZoneSetting();
    }
}
