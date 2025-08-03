using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rigid;
    public float speed;
    private float h;
    private float v;
    private bool isHorizontal;
    SpriteRenderer spriteRenderer;

    public GameObject Village;
    public GameObject WildMap;

    Animator anim;
    
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        //Move
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Horizontal"))
        {
            isHorizontal = true;
        }
        else if (Input.GetButtonDown("Vertical"))
        {
            isHorizontal = false;
        }
        else if (Input.GetButtonUp("Vertical") || Input.GetButtonUp("Horizontal"))
        {
            isHorizontal = h != 0;
        }


        //Animator Setting
        if (anim.GetInteger("xVelocity") != (int)rigid.linearVelocity.x)
        {
            anim.SetInteger("xVelocity", (int)rigid.linearVelocity.x);
        }
        else if (anim.GetInteger("yVelocity") != (int)rigid.linearVelocity.y)
        {
            anim.SetInteger("yVelocity", (int)rigid.linearVelocity.y);
        }

        if (rigid.linearVelocity.x != 0 || rigid.linearVelocity.y != 0)
        {
            anim.SetBool("Moving", true);
        }
        else
        {
            anim.SetBool("Moving", false);
        }

        /*if (rigid.linearVelocity.x == 5)
        {
            spriteRenderer.flipX = true;
        }
        else if (rigid.linearVelocity.x == -5)
        {
            spriteRenderer.flipX = false;
        }*/

    }

    private void FixedUpdate()
    {
        // Player Move
        if (isHorizontal)
        {
            rigid.linearVelocity = new Vector2(h, 0) * speed;
        }
        else
        {
            rigid.linearVelocity = new Vector2(0, v) * speed;
        }
    }

    /*private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Portal")
        {
            Debug.Log("c1");
            if (Input.GetButtonDown("Jump"))
            {
                Debug.Log("c2");
                Village.SetActive(false);
                WildMap.SetActive(true);
            }
        }
    }*/
}
