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
    //public int weapon_index;
    /*public float hp;
    public float total_exp;
    public float speed;
    public float atk;
    public float def;*/
    //private GameManager gm;

    public GameObject attackZone;
    public GameObject weapon;
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

        w_spriteRenderer = weapon.GetComponent<SpriteRenderer>();


        /*// --- Player 관련 ---
        attackZone = GameObject.Find("AttackZone");
        weapon = transform.Find("Weapon")?.gameObject;  // Player의 자식 안에 있다고 가정
        if (weapon != null)
            w_spriteRenderer = weapon.GetComponent<SpriteRenderer>();

        animator = GetComponent<Animator>();

        // --- UI 관련 ---
        // HP Bar
        PlayerHPBar = GameObject.Find("Canvas(Clone)/BackGround/PlayerHP").transform;

        // Die UI
        die_ui = GameObject.Find("Canvas(Clone)/Die_UI");
        die_text = GameObject.Find("Canvas(Clone)/Die_UI/DieText (TMP)").GetComponent<TextMeshProUGUI>();

        // EXP Bar + Text
        PlayerEXPBar = GameObject.Find("Canvas(Clone)/Ex_UI/PlayerEXPBar").transform;
        exp_text = GameObject.Find("Canvas(Clone)/Ex_UI/ExpText").GetComponent<TextMeshProUGUI>();

        // Level Text
        level_text = GameObject.Find("Canvas(Clone)/Level_UI/LevelText").GetComponent<TextMeshProUGUI>();*/

        // --- Player 관련 ---
      /*  attackZone = GameObject.Find("AttackZone");
        weapon = transform.Find("Weapon")?.gameObject; // Player 자식에 있을 때만
        if (weapon != null)
            w_spriteRenderer = weapon.GetComponent<SpriteRenderer>();

        animator = GetComponent<Animator>();

        // --- UI 관련 ---
        // HP Bar
        PlayerHPBar = GameObject.Find("Canvas/BackGround/PlayerHP").transform;

        // Die UI
        die_ui = GameObject.Find("Canvas/Die_UI");
        if (die_ui == null) Debug.LogError(" Die_UI 못 찾음!");
        die_text = GameObject.Find("Canvas/Die_UI/DieText (TMP)").GetComponent<TextMeshProUGUI>();
        if (die_text == null) Debug.LogError(" DieText 못 찾음!");

        // EXP Bar + Text
        PlayerEXPBar = GameObject.Find("Canvas/Ex_UI/PlayerEXPBar").transform;
        exp_text = GameObject.Find("Canvas/Ex_UI/ExpText").GetComponent<TextMeshProUGUI>();

        // Level Text
        level_text = GameObject.Find("Canvas/Level_UI/LevelText").GetComponent<TextMeshProUGUI>();*/
    }

    private void Start()
    {
        //var gm = GameManager.instance;
        //weapon = Weapon_Manager_s.Visible_Weapon(GameManager.Instance.weapon_index);
        /*float plus_atk = Weapon_Manager_s.Get_Weapon_Atk(GameManager.Instance.weapon_index);
        GameManager.Instance.atk += plus_atk;*/
        AttackZoneSetting();
        RigidSetting();

        GameManager.Instance.Player = this.gameObject;
        //Switch();
    }

    // Update is called once per frame
    void Update()
    {
        var gm = GameManager.Instance;

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

        float hpRatio = gm.Hp / 100f;
        PlayerHPBar.GetChild(0).GetComponent<UnityEngine.UI.Image>().fillAmount = hpRatio;

        int level = Exp_Calculating(gm.total_exp).Item1;
        float exp = Exp_Calculating(gm.total_exp).Item2;


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
        if (GameManager.Instance.Hp <= 0)
        {
            die_ui.SetActive(true);
            die_text.text = "You Die";
            Time.timeScale = 0;
        }
    }

    public void Player_Retry()
    {
        die_ui.SetActive(false);
        GameManager.Instance.Hp = 100;
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
