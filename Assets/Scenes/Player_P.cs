using UnityEngine;

public class Player_Platformer : MonoBehaviour
{
    Rigidbody2D rigid;
    float move;
    public float speed;
    public float jump;
    bool isDamageing;

    Animator anim;
    SpriteRenderer spriteRenderer;
    bool isjumping;
    RaycastHit2D ray;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        

        //Jump
        if (Input.GetButtonDown("Jump") && !isjumping)
        {
            rigid.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
            isjumping = true;
        }

        //Jump Animation
        anim.SetBool("isJump", isjumping);
        


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

    private void FixedUpdate()
    {
        //Move
        if (!isDamageing)
        {
            move = Input.GetAxisRaw("Horizontal");

            rigid.linearVelocity = new Vector2(move * speed, rigid.linearVelocity.y);
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
        //몬스터에 닿았을때 넉백
        if (collision.gameObject.layer == 6)
        {
            int xKnockback = transform.position.x - collision.transform.position.x > 0 ? 1 : -1;
            rigid.AddForce(new Vector2(xKnockback, 1) * 7, ForceMode2D.Impulse);
            isDamageing = true;

            Invoke("EndDamaging", 0.3f);
        }
    }

    void EndDamaging()
    {
        //데미지상태 풀기
        isDamageing = false;
    }
}
