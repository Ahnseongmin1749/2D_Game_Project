using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class Slime_Fix : MonsterBase
{
    Rigidbody2D rigid;
    public int nextJumpdirection;
    public int nextJumpTime;
    RaycastHit2D dawnray;
    RaycastHit2D playercheckray;
    bool isjumping;
    bool isplayerchecking;
    Animator anim;
    SpriteRenderer spriteRenderer;
    public GameObject player;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        Invoke("EnemyMove", 1);
    }

    void EnemyMove()
    {
        if (!isjumping && isplayerchecking)
        {
            // ● 플레이어 방향으로 점프
            float direction = Mathf.Sign(player.transform.position.x - transform.position.x);
            isjumping = true;
            rigid.AddForce(new Vector2(direction * 3, 5), ForceMode2D.Impulse);
            Invoke("EnemyMove", 0.5f);
        }
        else if (!isjumping && !isplayerchecking)
        {
            // ● 랜덤 점프 (기존대로 유지)
            nextJumpdirection = Random.Range(-1, 2);
            nextJumpTime = Random.Range(1, 4);
            isjumping = true;
            rigid.AddForce(new Vector2(nextJumpdirection * 3, 5), ForceMode2D.Impulse);
            Invoke("EnemyMove", nextJumpTime);
        }
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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // ● 바닥 감지 → isjumping 상태 갱신
        PlatfromCheckRay();

        // ● 이동 방향 기준으로 flipX 갱신 (속도 기준)
        float velocityX = rigid.linearVelocity.x;

        if (Mathf.Abs(velocityX) >= 0.01f)
        {
            if (velocityX < 0)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
        }

        // ● flipX 기준 방향으로 Ray 발사 → 정확한 감지 보장
        PlayerCheckRay();

        // ● 디버그 출력
        /*Debug.Log("플레이어 체크 " + isplayerchecking);
        Debug.Log("플립 체크 " + spriteRenderer.flipX);*/
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
        Vector2 rayDirection;

        // ● 항상 flipX 기준 방향으로 시야
        if (spriteRenderer.flipX)
        {
            rayDirection = Vector2.left;
        }
        else
        {
            rayDirection = Vector2.right;
        }

        Debug.DrawRay(transform.position, rayDirection * 5f, new Color(1, 1, 0, 0.7f));

        playercheckray = Physics2D.Raycast(transform.position, rayDirection, 5f, LayerMask.GetMask("Player"));

        if (playercheckray.collider != null && playercheckray.collider.gameObject.layer == 3)
        {
            if (!isplayerchecking)
            {
                isplayerchecking = true;            // ★ 먼저 true로 설정
                CancelInvoke("EnemyMove");
                Invoke("EnemyMove", 0.5f);          // 이제 바로 추격으로 진입함
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
    }

    public override void SlimeDamage(int dir)
    {
        // 슬라임 맞는 연출
        rigid.AddForce(new Vector2(dir, 1) * 3, ForceMode2D.Impulse);
        Debug.Log(dir);
        if (!isplayerchecking)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }
}