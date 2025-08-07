using NUnit.Framework.Internal;
using UnityEditor;
using UnityEngine;

public class Player_Platformer : MonoBehaviour
{
    Rigidbody2D rigid;
    public float move;
    public float speed;
    public bool isrightlooking;
    public float jump;
    bool isDamageing;
    bool isLie;

    Animator anim;
    SpriteRenderer spriteRenderer;
    bool isjumping;
    RaycastHit2D ray;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        CheckRightLooking();

        //Jump
        if (Input.GetButtonDown("Jump") && !isjumping)
        {
            rigid.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
            isjumping = true;
        }

        //Jump Animation
        anim.SetBool("isJump", isjumping);
        
        //Lie Animation
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            isLie = true;
            anim.SetBool("isLie",isLie);
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            isLie = false;
            anim.SetBool("isLie", isLie);
        }


        //Move Animation
        if (rigid.linearVelocity.x < 0)
        {
            anim.SetBool("ismoving", true);
            anim.SetInteger("xVelocity", (int)move);
        }
        else if (rigid.linearVelocity.x > 0)
        {
            anim.SetBool("ismoving", true);
            anim.SetInteger("xVelocity", (int)move);
        }
        else if (rigid.linearVelocity.x == 0)
        {
            anim.SetBool("ismoving", false);
            anim.SetInteger("xVelocity", (int)move);
        }
        
        
    }

    void CheckRightLooking()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0); // 0은 Base Layer

        if (stateInfo.IsName("Left_Walk") || stateInfo.IsName("Left_Idle") || stateInfo.IsName("Left_Jump") ||
            stateInfo.IsName("Left_Lie") || stateInfo.IsName("Left_Crawl"))
        {
            isrightlooking = false;
            anim.SetBool("isRight", isrightlooking);
        }
        else if (stateInfo.IsName("Right_Walk") || stateInfo.IsName("Right_Idle") || stateInfo.IsName("Right_Jump") ||
            stateInfo.IsName("Right_Lie") || stateInfo.IsName("Right_Crawl"))
        {
            isrightlooking = true;
            anim.SetBool("isRight", isrightlooking);
        }
    }


    private void FixedUpdate()
    {
        //Move
        if (!isDamageing)
        {
            move = Input.GetAxisRaw("Horizontal");

            float adjustedSpeed = isLie ? speed * 0.5f : speed;


            rigid.linearVelocity = new Vector2(move * adjustedSpeed, rigid.linearVelocity.y);

            Debug.Log(rigid.linearVelocity);
        }
        

        //Ray
        if (rigid.linearVelocity.y < 0)
        {
            Debug.DrawRay(transform.position, Vector2.down * 1, new Color(1, 0, 0, 0.7f));

            ray = Physics2D.Raycast(transform.position, Vector2.down,
            1, LayerMask.GetMask("Platform"));
            if (ray.collider != null)
            {
                if (ray.collider.gameObject.layer == 10)
                {
                    isjumping = false;
                }

            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.layer == 6)
        {
            OnDamaged(collision);
        }
    }

    void OnDamaged(Collision2D col)
    {
        //피격시 색깔 바꾸기
        spriteRenderer.color = new Color(0.78f, 0.78f, 0.78f, 0.7f);

        //피격시 넉백
        int xKnockback = transform.position.x - col.transform.position.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(xKnockback, 1) * 7, ForceMode2D.Impulse);
        isDamageing = true;

        Invoke("EndDamaged", 0.3f);
    }

    void EndDamaged()
    {
        //색깔 원상복귀
        spriteRenderer.color = new Color(1, 1, 1, 1f);

        //데미지상태 풀기
        isDamageing = false;
    }
}
