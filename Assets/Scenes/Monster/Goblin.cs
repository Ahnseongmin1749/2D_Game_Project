using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using static UnityEngine.UI.Image;

public class Goblin : MonsterBase
{
    float HP = 100;
    float direction;
    Rigidbody2D rigid;
    public int nextJumpdirection;
    public int nextJumpTime;
    RaycastHit2D dawnray;
    RaycastHit2D playercheckray;
    public bool isplayerchecking;
    public bool seeright;
    public bool isFlip;
    Animator anim;
    SpriteRenderer spriteRenderer;
    public GameObject player;
    public GameObject attackEffect;
    public GameObject excalEffect;
    Animator attackAnim;
    Animator exclaAnim;

    public GameObject healthBarPrefab; // �̰� ����Ƽ���� �������� (MonsterHealthBar ������)
    Transform healthBar;

    CapsuleCollider2D capsuleCollider;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        attackAnim = attackEffect.GetComponent<Animator>();
        exclaAnim = excalEffect.GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();

        gameObject.SetActive(true);

        EnemyMoveDirection();
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


    void EnemyMoveDirection()
    {
        direction = isplayerchecking ? Mathf.Sign(player.transform.position.x - transform.position.x) :
            Random.Range(-1, 2);

        float invoketime = Random.Range(3, 6);

        Invoke("EnemyMoveDirection", invoketime);
    }

    

    



    void Update()
    {
        HP_UI_Update();
        DirectionFlip(direction);

    }

    

    void HP_UI_Update()
    {
        // ����: ü�� �پ�� �� fillAmount ����
        // bar.transform.GetChild(0)�� HPFill Image
        float hpRatio = HP / 100f;
        healthBar.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Image>().fillAmount = hpRatio;
    }

    

    void DirectionFlip(float dir)
    {
        if (dir > 0)
            seeright = true;
        else if (dir < 0)
            seeright = false;
    }

    

    // Update is called once per frame
    void FixedUpdate()
    {

        // �ٴ� ���� Ray �Լ�
        PlatfromCheckRay();

        // �÷��̾� ���� Ray �Լ�
        PlayerCheckRay();

        // ��� �̵� �Լ�
        EnemyMove();

        if (isplayerchecking)
        {
            CancelInvoke();
            EnemyMoveDirection();
        }

        
    }

    void EnemyMove()
    {
        Vector2 movevec = new Vector2(direction, 0);

        rigid.AddForce(movevec, ForceMode2D.Impulse);


        if (!isplayerchecking && rigid.linearVelocity.x > 3)
        {
            rigid.linearVelocity = new Vector2(3, rigid.linearVelocity.y);
        }
        else if (!isplayerchecking && rigid.linearVelocity.x < -3)
        {
            rigid.linearVelocity = new Vector2(-3, rigid.linearVelocity.y);
        }
        else if (isplayerchecking && rigid.linearVelocity.x > 5)
        {
            rigid.linearVelocity = new Vector2(5, rigid.linearVelocity.y);
        }
        else if (isplayerchecking && rigid.linearVelocity.x < -5)
        {
            rigid.linearVelocity = new Vector2(-5, rigid.linearVelocity.y);
        }
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
                    anim.SetTrigger("test");
                }
                isplayerchecking = true;
            }
            else
            {
                isplayerchecking = false;
            }
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            direction = direction * -1;
        }
    }


    public override void GoblinDamage(int dir)
    {
        // ��� �´� ����
        rigid.AddForce(new Vector2(dir, 1) * 5, ForceMode2D.Impulse);
        if (!isplayerchecking && direction == 0)
        {
            seeright = !seeright;
        }
        else if (!isplayerchecking && direction != 0)
        {
            direction = direction * -1;
        }
        attackAnim.SetTrigger("isDamaging");

        Player_State player_State = player.GetComponent<Player_State>();
        HP -= player_State.atk;

        if (HP <= 0)
        {
            Die_Effect_Goblin();
            player_State.total_exp += 10;
        }
    }

    void Die_Effect_Goblin()
    {
        CancelInvoke("NextMoveSelect");
        spriteRenderer.color = new Color(0.78f, 0.78f, 0.78f);
        spriteRenderer.flipY = true;
        capsuleCollider.enabled = false;
        Invoke("Disappear_Goblin", 3);
    }

    void Disappear_Goblin()
    {
        capsuleCollider.enabled = false;
        gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        AnimationFunc();
    }

    void AnimationFunc()
    {
        exclaAnim.SetBool("check!", isplayerchecking);
        anim.SetInteger("xVelocity", (int)direction);
        anim.SetBool("seeRight", seeright);
        anim.SetBool("playercheck", isplayerchecking);

    }


}



