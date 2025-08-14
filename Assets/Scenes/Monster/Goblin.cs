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

    public GameObject healthBarPrefab; // 이걸 유니티에서 연결해줘 (MonsterHealthBar 프리팹)
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
        // 프리팹 복사해서 생성
        GameObject bar = Instantiate(healthBarPrefab);

        // 부모를 이 몬스터로 설정 (머리 위에 따라오게 함)
        bar.transform.SetParent(transform);

        // 머리 위에 위치 조정 (y를 조금 올림)
        bar.transform.localPosition = new Vector3(0, 1, 0); // 몬스터 크기에 따라 조절
        /*localPosition은 부모(몬스터) 기준의 위치
        position은 월드(전체 맵) 기준의 위치*/

        // 카메라 쪽을 보게 하려면 따로 LookAt 처리 필요
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
        // 예시: 체력 줄어들 때 fillAmount 조절
        // bar.transform.GetChild(0)는 HPFill Image
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

        // 바닥 감지 Ray 함수
        PlatfromCheckRay();

        // 플레이어 감지 Ray 함수
        PlayerCheckRay();

        // 고블린 이동 함수
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
        // 바닥 감지 ray
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
        // 플레이어 감지 ray, 플립기준 삼항연산자 ray 방향 판단
        Vector3 xRayDirection = seeright ? Vector2.right : Vector2.left;
        Debug.DrawRay(transform.position, xRayDirection * 7f, new Color(1, 1, 0, 0.7f));
        Debug.DrawRay(transform.position + new Vector3(0, -0.3f, 0), xRayDirection * 7f, new Color(1, 1, 0, 0.7f));
        Debug.DrawRay(transform.position + new Vector3(0, 0.3f, 0), xRayDirection * 7f, new Color(1, 1, 0, 0.7f));

        Vector2[] rayOrigins = new Vector2[]
        {
        transform.position,                             // 가운데
        transform.position + new Vector3(0, -0.3f, -1), // 아래쪽
        transform.position + new Vector3(0,  0.3f, -1)  // 위쪽
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
        // 고블린 맞는 연출
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



