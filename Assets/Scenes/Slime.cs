using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class Slime : MonoBehaviour
{
    Rigidbody2D rigid;
    public int nextJumpdirection;
    public int nextJumpTime;
    RaycastHit2D dawnray;
    RaycastHit2D playercheckray;
    bool isjumping;
    bool isplayerchecking;
    bool prevPlayerChecking;
    bool seeright;
    bool isFlip;
    Animator anim;
    SpriteRenderer spriteRenderer;
    public GameObject player;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        NextMoveSelect();
    }

    private void Start()
    {
    }

    /*void EnemyMove()
    {
        if (!isjumping && !isplayerchecking)
        {
            nextJumpdirection = Random.Range(-1, 2);
            nextJumpTime = Random.Range(1, 4);
            isjumping = true;
            rigid.AddForce(new Vector2(nextJumpdirection * 3, 5), ForceMode2D.Impulse);
            Invoke("EnemyMove", nextJumpTime);

        }
        else if (!isjumping && isplayerchecking)
        {
            float direction = Mathf.Sign(player.transform.position.x - transform.position.x);
            isjumping = true;
            rigid.AddForce(new Vector2(direction * 3, 5), ForceMode2D.Impulse);
            Invoke("EnemyMove", 0.5f);
        }
    }*/

    void EnemyUsualMove()
    {
        Debug.Log("평소모습함수");
        float nextJumpdirection = Random.Range(-1, 2);

        isjumping = true;
        rigid.AddForce(new Vector2(nextJumpdirection * 3, 5), ForceMode2D.Impulse);
        DirectionFlip(nextJumpdirection);
        NextMoveSelect();
    }

    void EnemyAngryMove()
    {
        Debug.Log("앵그리모습함수");
        float direction = Mathf.Sign(player.transform.position.x - transform.position.x);
        isjumping = true;
        rigid.AddForce(new Vector2(direction * 4, 6), ForceMode2D.Impulse);
        DirectionFlip(direction);
        NextMoveSelect();
    }

    void NextMoveSelect()
    {

        if (!isplayerchecking && !isjumping)
        {
            int nextJumpTime = Random.Range(1, 4);
            Invoke("EnemyUsualMove", nextJumpTime);
        }
        else if (isplayerchecking && !isjumping)
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
        // 바닥 감지 Ray 함수
        PlatfromCheckRay();

        // 플레이어 감지 Ray 함수
        PlayerCheckRay();

        
        /*if (rigid.linearVelocity.x < 0)
        {
            Debug.Log("좌");
            isFlip = true;
        }
        else if (rigid.linearVelocity.x > 0)
        {
            Debug.Log("우");
            isFlip = false;
        }*/

        if (!seeright)
        {
            isFlip = true;
        }
        else if (seeright)
        {
            isFlip = false;
        }

        spriteRenderer.flipX = isFlip;

        /*//GPT 솔루션
        if (!prevPlayerChecking && isplayerchecking)
        {
            Debug.Log("플레이어 발견! 공격 전환");
            CancelInvoke("EnemyUsualMove");
            CancelInvoke("nextMoveSelect");
            EnemyAngryMove(); // 단발 호출
        }

        // 상태 업데이트는 반드시 마지막에!
        prevPlayerChecking = isplayerchecking;*/


    }
    void Update()
    {
        anim.SetBool("isJumping", isjumping);
        anim.SetBool("isPlayerChecking", isplayerchecking);

        Debug.Log(isplayerchecking);
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
        Vector2 xRayDirection = isFlip ? Vector2.left : Vector2.right;
        Debug.DrawRay(transform.position, xRayDirection * 7f, new Color(1, 1, 0, 0.7f));
        Debug.DrawRay(transform.position + new Vector3(0, -0.5f, 0), xRayDirection * 7f, new Color(1, 1, 0, 0.7f));
        Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), xRayDirection * 7f, new Color(1, 1, 0, 0.7f));

        Vector2[] rayOrigins = new Vector2[]
        {
        transform.position,                             // 가운데
        transform.position + new Vector3(0, -0.5f, -1), // 아래쪽
        transform.position + new Vector3(0,  0.5f, -1)  // 위쪽
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
                if (!isplayerchecking)
                {
                    Debug.Log("sample");
                    isplayerchecking = true;            // ★ 먼저 true로 설정
                    CancelInvoke("EnemyUsualMove");
                    Invoke("NextMoveSelect", 0.5f);     // 이제 바로 추격으로 진입함
                }
                else
                {
                    isplayerchecking = true;
                }
            }
            else
            {
                isplayerchecking = false;
            }
            prevPlayerChecking = isplayerchecking;
        }
        
    }

    
}


