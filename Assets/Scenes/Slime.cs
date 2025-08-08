using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class Slime : MonsterBase
{
    float HP = 100;
    Rigidbody2D rigid;
    public int nextJumpdirection;
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


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        subanim = AttackEffect.GetComponent<Animator>();
        gameObject.SetActive(true);
        NextMoveSelect();
    }

    private void Start()
    {
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

    /*void EnemyMove()
    {
        if (!isjumping && !isplayerchecking)
        {
            nextJumpdirection = Random.Range(-1, 2);
            nextJumpTime = Random.Range(1, 4);
            isjumping = true;
            rigid.AddForce(new Vector2(nextJumpdirection * 3, 5), ForceMode2D.Impulse);
            DirectionFlip(nextJumpdirection);
            Invoke("EnemyMove", nextJumpTime);

        }
        else if (!isjumping && isplayerchecking)
        {
            Debug.Log("�� �ȿ���");
            int direction = (int)Mathf.Sign(player.transform.position.x - transform.position.x);
            isjumping = true;
            rigid.AddForce(new Vector2(direction * 3, 5), ForceMode2D.Impulse);
            DirectionFlip(direction);
            Invoke("EnemyMove", 0.5f);
        }
    }*/

    void EnemyUsualMove()
    {
        float nextJumpdirection = Random.Range(-1, 2);

        isjumping = true;
        rigid.AddForce(new Vector2(nextJumpdirection * 3, 5), ForceMode2D.Impulse);
        DirectionFlip(nextJumpdirection);
        NextMoveSelect();
    }

    void EnemyAngryMove()
    {
        float direction = Mathf.Sign(player.transform.position.x - transform.position.x);
        isjumping = true;
        rigid.AddForce(new Vector2(direction * 4, 5), ForceMode2D.Impulse);
        DirectionFlip(direction);
        NextMoveSelect();
    }

    void NextMoveSelect()
    {

        if (!isplayerchecking)
        {
            int nextJumpTime = Random.Range(1, 4);
            Invoke("EnemyUsualMove", nextJumpTime);
        }
        else if (isplayerchecking)
        {
            Invoke("EnemyAngryMove", 0.5f);
        }
    }

    void DirectionFlip(float dir)
    {
        if (dir > 0)
            seeright = true;
        else if (dir < 0)
            seeright = false;
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            Invoke("EnemyMove", nextJumpTime);
            nextJumpdirection = nextJumpdirection * -1;
            rigid.AddForce(new Vector2(nextJumpdirection * 3, 1), ForceMode2D.Impulse);
        }
    }*/

    // Start is called once before the first execution of Update after the MonoBehaviour is created


    // Update is called once per frame
    void FixedUpdate()
    {
        // �ٴ� ���� Ray �Լ�
        PlatfromCheckRay();

        // �÷��̾� ���� Ray �Լ�
        PlayerCheckRay();

        
        /*if (rigid.linearVelocity.x < 0)
        {
            Debug.Log("��");
            isFlip = true;
        }
        else if (rigid.linearVelocity.x > 0)
        {
            Debug.Log("��");
            isFlip = false;
        }*/

        /*if (!seeright)
        {
            isFlip = true;
        }
        else if (seeright)
        {
            isFlip = false;
        }*/

        spriteRenderer.flipX = !seeright;

        /*//GPT �ַ��
        if (!prevPlayerChecking && isplayerchecking)
        {
            Debug.Log("�÷��̾� �߰�! ���� ��ȯ");
            CancelInvoke("EnemyUsualMove");
            CancelInvoke("nextMoveSelect");
            EnemyAngryMove(); // �ܹ� ȣ��
        }

        // ���� ������Ʈ�� �ݵ�� ��������!
        prevPlayerChecking = isplayerchecking;*/


    }
    void Update()
    {
        anim.SetBool("isJumping", isjumping);
        anim.SetBool("isPlayerChecking", isplayerchecking);


        HP_UI_Update();
        Die_Slime();
    }

    void HP_UI_Update()
    {
        // ����: ü�� �پ�� �� fillAmount ����
        // bar.transform.GetChild(0)�� HPFill Image
        float hpRatio = HP / 100f;
        healthBar.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Image>().fillAmount = hpRatio;
    }

    void Die_Slime()
    {
        if (HP <= 0)
        {
            gameObject.SetActive(false);
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
                    isjumping = false;
                }

            }
        }
    }

    void PlayerCheckRay()
    {
        // �÷��̾� ���� ray, �ø����� ���׿����� ray ���� �Ǵ�
        Vector2 xRayDirection = seeright ? Vector2.right : Vector2.left;
        Debug.DrawRay(transform.position, xRayDirection * 7f, new Color(1, 1, 0, 0.7f));
        Debug.DrawRay(transform.position + new Vector3(0, -0.5f, 0), xRayDirection * 7f, new Color(1, 1, 0, 0.7f));
        Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), xRayDirection * 7f, new Color(1, 1, 0, 0.7f));

        Vector2[] rayOrigins = new Vector2[]
        {
        transform.position,                             // ���
        transform.position + new Vector3(0, -0.5f, -1), // �Ʒ���
        transform.position + new Vector3(0,  0.5f, -1)  // ����
        };


        foreach (Vector3 rayOrigin in rayOrigins)
        {
            playercheckray = Physics2D.Raycast(rayOrigin, xRayDirection, 7f, LayerMask.GetMask("Player"));
            /*if (playercheckray.collider != null)
            {
                if (playercheckray.collider.gameObject.layer == 3)
                {
                    isplayerchecking = true;
                    
                }

            }*/

            if (playercheckray.collider != null && playercheckray.collider.gameObject.layer == 3)
            {
                /*if (!isplayerchecking)
                {
                    Debug.Log("sample");
                    isplayerchecking = true;            // �� ���� true�� ����
                    CancelInvoke("NextMoveSelect");
                    Invoke("NextMoveSelect", 0.5f);     // ���� �ٷ� �߰����� ������
                }
                else
                {
                    isplayerchecking = true;
                }*/
                isplayerchecking = true;
            }
            else
            {
                isplayerchecking = false;
            }
        }
        
    }

    /*public void OnDamaged(int dir)
    {
        rigid.AddForce(new Vector2 (dir , 1) * 6, ForceMode2D.Impulse);
    }*/

    public override void SlimeDamage(int dir)
    {
        // ������ �´� ����
        rigid.AddForce(new Vector2(dir, 1) * 3, ForceMode2D.Impulse);
        if (!isplayerchecking)
        {
            seeright = !seeright;
        }
        subanim.SetTrigger("isDamaging");

        Player_State player_State = player.GetComponent<Player_State>();
        HP -= player_State.atk;
    }


}


