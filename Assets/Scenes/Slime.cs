using Unity.VisualScripting;
using UnityEngine;

public class Enemy_P : MonoBehaviour
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
        EnemyUsualMove();
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
        nextJumpdirection = Random.Range(-1, 2);
        nextJumpTime = Random.Range(1, 4);
        isjumping = true;
        rigid.AddForce(new Vector2(nextJumpdirection * 3, 5), ForceMode2D.Impulse);
        if (!isplayerchecking)
        {
            Invoke("EnemyUsualMove", nextJumpTime);
        }
        else if (isplayerchecking)
        {
            Invoke("EnemyAngryMove", 0.5f);
        }
    }

    void EnemyAngryMove()
    {
        float direction = Mathf.Sign(player.transform.position.x - transform.position.x);
        isjumping = true;
        rigid.AddForce(new Vector2(direction * 4, 6), ForceMode2D.Impulse);
        if (!isplayerchecking)
        {
            Invoke("EnemyUsualMove", 0.5f);
        }
        else if (isplayerchecking)
        {
            Invoke("EnemyAngryMove", 0.5f);
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


    // Update is called once per frame
    void FixedUpdate()
    {
        // 바닥 감지 Ray 함수
        PlatfromCheckRay();

        // 플레이어 감지 Ray 함수
        PlayerCheckRay();

        Debug.Log("플레이어 체크 " + isplayerchecking);
        Debug.Log("플립 체크 " +  spriteRenderer.flipX);


        if (rigid.linearVelocity.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (rigid.linearVelocity.x > 0)
        {
            spriteRenderer.flipX = false;
        }
            
    }
    void Update()
    {
        anim.SetBool("isJumping", isjumping);
        anim.SetBool("isPlayerChecking", isplayerchecking);
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
        Vector2 xRayDirection = spriteRenderer.flipX ? Vector2.left : Vector2.right;
        Debug.DrawRay(transform.position, xRayDirection * 5f, new Color(1, 1, 0, 0.7f));
        Debug.DrawRay(transform.position + new Vector3(0, -0.5f, 0), xRayDirection * 5f, new Color(1, 1, 0, 0.7f));
        Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), xRayDirection * 5f, new Color(1, 1, 0, 0.7f));

        Vector3[] rayOrigins = new Vector3[]
        {
        transform.position,                          // 가운데
        transform.position + new Vector3(0, -0.5f, 0), // 아래쪽
        transform.position + new Vector3(0,  0.5f, 0)  // 위쪽
        };

        isplayerchecking = false;

        foreach (Vector3 rayOrigin in rayOrigins)
        {
            playercheckray = Physics2D.Raycast(rayOrigin, xRayDirection, 5f, LayerMask.GetMask("Player"));
            if (playercheckray.collider != null)
            {
                if (playercheckray.collider.gameObject.layer == 3)
                {
                    isplayerchecking = true;
                }

            }
        }

    }
}
