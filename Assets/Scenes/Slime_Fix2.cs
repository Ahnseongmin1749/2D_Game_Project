using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using static UnityEngine.UI.Image;

public class Slime_Fix2 : MonsterBase
{
    bool test;

    float HP = 100;
    Rigidbody2D rigid;
    public int nextJumpTime;
    RaycastHit2D dawnray;
    RaycastHit2D playercheckray;
    bool isjumping;
    public bool isplayerchecking;
    bool prevPlayerChecking;
    public bool seeright;
    public bool isFlip;
    Animator anim;
    SpriteRenderer spriteRenderer;
    public GameObject player;

    public GameObject AttackEffect;
    Animator subanim;

    public GameObject healthBarPrefab; // �̰� ����Ƽ���� �������� (MonsterHealthBar ������)
    Transform healthBar;

    CircleCollider2D circleCollider;

    bool tryInvoke;

    

    //private GameManager gm;



    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        subanim = AttackEffect.GetComponent<Animator>();
        circleCollider = GetComponent<CircleCollider2D>();


        gameObject.SetActive(true);
        //NextMoveSelect();
        
    }

    private void Start()
    {
        monster_atk = 10;
        HP_UI_Setting();
    }

    void HP_UI_Setting()
    {
        // ������ �����ؼ� ����
        GameObject bar = Instantiate(healthBarPrefab);

        // �θ� �� ���ͷ� ���� (�Ӹ� ���� ������� ��)
        bar.transform.SetParent(transform);

        // �Ӹ� ���� ��ġ ���� (y�� ���� �ø�)
        bar.transform.localPosition = new Vector3(0, 1, 0); // ���� ũ�⿡ ���� ����
        /*localPosition�� �θ�(����) ������ ��ġ
        position�� ����(��ü ��) ������ ��ġ*/

        // ī�޶� ���� ���� �Ϸ��� ���� LookAt ó�� �ʿ�
        healthBar = bar.transform;
    }
    
    

    

    void NextMoveSelect()
    {
        
    }

    



    void Update()
    {
        anim.SetBool("isJumping", isjumping);
        anim.SetBool("isPlayerChecking", isplayerchecking);

        HP_UI_Update();
        
        if (!isplayerchecking && !isjumping)
        {
            float nextJumpTime = Random.Range(1, 4);
            StartCoroutine(EnemyUsualMove());
        }
        else if (isplayerchecking && !isjumping)
        {
            StartCoroutine(EnemyAngryMove());
        }
    }

    IEnumerator EnemyUsualMove()
    {
        while (HP > 0) {
            float nextJumpdirection = Random.Range(-1, 2);
            yield return new WaitForSeconds(nextJumpTime);
            isjumping = true;
            rigid.AddForce(new Vector2(nextJumpdirection * 3, 5), ForceMode2D.Impulse);
            DirectionFlip(nextJumpdirection);
        }
        
    }

    IEnumerator EnemyAngryMove()
    {
        while (HP > 0)
        {
            yield return new WaitForSeconds(1);
            float direction = Mathf.Sign(player.transform.position.x - transform.position.x);
            isjumping = true;
            rigid.AddForce(new Vector2(direction * 4, 5), ForceMode2D.Impulse);
            DirectionFlip(direction);
        }
    }
    void DirectionFlip(float dir)
    {
        if (dir > 0)
            seeright = true;
        else if (dir < 0)
            seeright = false;
    }

    void HP_UI_Update()
    {
        // ����: ü�� �پ�� �� fillAmount ����
        // bar.transform.GetChild(0)�� HPFill Image
        float hpRatio = HP / 100f;
        healthBar.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Image>().fillAmount = hpRatio;
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        // �ٴ� ���� Ray �Լ�
        PlatfromCheckRay();

        // �÷��̾� ���� Ray �Լ�
        PlayerCheckRay();



        spriteRenderer.flipX = !seeright;

    }

    void PlatfromCheckRay()
    {
        // �ٴ� ���� ray
        if (rigid.linearVelocity.y < 0)
        {
            Debug.DrawRay(transform.position, Vector2.down * 0.5f, new Color(1, 0, 0, 0.7f));

            dawnray = Physics2D.Raycast(transform.position, Vector2.down,
            0.5f, LayerMask.GetMask("Platform"));
            if (dawnray.collider != null)
            {
                if (dawnray.collider.gameObject.layer == 10)
                {
                    isjumping = false;
                }

            }
        }
    }

    void PlayerCheckRay()
    {
        // �÷��̾� ���� ray, �ø����� ���׿����� ray ���� �Ǵ�
        Vector3 xRayDirection = seeright ? Vector2.right : Vector2.left;
        Debug.DrawRay(transform.position, xRayDirection * 7f, new Color(1, 1, 0, 0.7f));
        Debug.DrawRay(transform.position + new Vector3(0, -0.3f, 0), xRayDirection * 7f, new Color(1, 1, 0, 0.7f));
        Debug.DrawRay(transform.position + new Vector3(0, 0.3f, 0), xRayDirection * 7f, new Color(1, 1, 0, 0.7f));

        Vector2[] rayOrigins = new Vector2[]
        {
        transform.position,                             // ���
        transform.position + new Vector3(0, -0.3f, -1), // �Ʒ���
        transform.position + new Vector3(0,  0.3f, -1)  // ����
        };


        foreach (Vector3 rayOrigin in rayOrigins)
        {
            playercheckray = Physics2D.Raycast(rayOrigin, xRayDirection, 7f, LayerMask.GetMask("Player"));

            if (playercheckray.collider != null && playercheckray.collider.gameObject.layer == 3)
            {
                if (!isplayerchecking)
                {
                    Debug.Log("test");
                }

                isplayerchecking = true;

            }
            else
            {
                isplayerchecking = false;
            }
        }


    }


    void InvokeControl()
    {
        if (tryInvoke)
        {
            CancelInvoke("NextMoveSelect");
            Invoke("NextMoveSelect", 0.5f);

            tryInvoke = false;
        }
    }




    public override void SlimeDamage(int dir)
    {
        // ������ �´� ����
        rigid.AddForce(new Vector2(dir, 1) * 3, ForceMode2D.Impulse);
        if (!isplayerchecking)
        {
            seeright = !seeright;
        }
        subanim.SetTrigger("isDamaging");

        HP -= GameManager.Instance.atk;


        if (HP <= 0)
        {
            Die_Effect_Slime();

            GameManager.Instance.total_exp += 10;
        }
    }

    void Die_Effect_Slime()
    {
        if (HP <= 0)
        {
            CancelInvoke("NextMoveSelect");
            spriteRenderer.color = new Color(0.78f, 0.78f, 0.78f);
            spriteRenderer.flipY = true;
            circleCollider.enabled = false;

            Invoke("Disappear_Slime", 3);
        }
    }

    void Disappear_Slime()
    {
        circleCollider.enabled = false;
        gameObject.SetActive(false);
    }


}


