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

    bool islookright;
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
        //시선 bool값
        if (rigid.linearVelocity.x > 0)
            islookright = true;
        else if ((rigid.linearVelocity.x < 0))
            islookright = false;

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
