using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using static UnityEngine.UI.Image;

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

    public GameObject healthBarPrefab; // 이걸 유니티에서 연결해줘 (MonsterHealthBar 프리팹)
    Transform healthBar;

    CircleCollider2D circleCollider;

    bool tryInvoke;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        subanim = AttackEffect.GetComponent<Animator>();
        circleCollider = GetComponent<CircleCollider2D>();

        gameObject.SetActive(true);
        NextMoveSelect();
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
            Debug.Log("왜 안오노");
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


    
    void Update()
    {
        anim.SetBool("isJumping", isjumping);
        anim.SetBool("isPlayerChecking", isplayerchecking);


        HP_UI_Update();
        Die_Effect_Slime();

        
        /*if (isplayerchecking != prevPlayerChecking)
        {
            Debug.Log("Trun");
            CancelInvoke();
            EnemyAngryMove();
        }
        prevPlayerChecking = isplayerchecking;*/
    }

    void HP_UI_Update()
    {
        // 예시: 체력 줄어들 때 fillAmount 조절
        // bar.transform.GetChild(0)는 HPFill Image
        float hpRatio = HP / 100f;
        healthBar.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Image>().fillAmount = hpRatio;
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

    // Update is called once per frame
    void FixedUpdate()
    {
        // 바닥 감지 Ray 함수
        PlatfromCheckRay();

        // 플레이어 감지 Ray 함수
        PlayerCheckRay();



        spriteRenderer.flipX = !seeright;

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
                    isjumping = false;
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
                /*if (!isplayerchecking)
                {
                    isplayerchecking = true; // ★ 먼저 true로 설정


                    tryInvoke = true;
                    InvokeControl();

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
    

    void InvokeControl()
    {
        if (tryInvoke)
        {
            CancelInvoke("NextMoveSelect");
            Invoke("NextMoveSelect", 0.5f);

            tryInvoke = false;
        }
    }




    /*public void OnDamaged(int dir)
    {
        rigid.AddForce(new Vector2 (dir , 1) * 6, ForceMode2D.Impulse);
    }*/

    public override void SlimeDamage(int dir)
    {
        // 슬라임 맞는 연출
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


