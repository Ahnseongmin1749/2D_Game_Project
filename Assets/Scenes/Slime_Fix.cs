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
            // �� �÷��̾� �������� ����
            float direction = Mathf.Sign(player.transform.position.x - transform.position.x);
            isjumping = true;
            rigid.AddForce(new Vector2(direction * 3, 5), ForceMode2D.Impulse);
            Invoke("EnemyMove", 0.5f);
        }
        else if (!isjumping && !isplayerchecking)
        {
            // �� ���� ���� (������� ����)
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
        // �� �ٴ� ���� �� isjumping ���� ����
        PlatfromCheckRay();

        // �� �̵� ���� �������� flipX ���� (�ӵ� ����)
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

        // �� flipX ���� �������� Ray �߻� �� ��Ȯ�� ���� ����
        PlayerCheckRay();

        // �� ����� ���
        /*Debug.Log("�÷��̾� üũ " + isplayerchecking);
        Debug.Log("�ø� üũ " + spriteRenderer.flipX);*/
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
        Vector2 rayDirection;

        // �� �׻� flipX ���� �������� �þ�
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
                isplayerchecking = true;            // �� ���� true�� ����
                CancelInvoke("EnemyMove");
                Invoke("EnemyMove", 0.5f);          // ���� �ٷ� �߰����� ������
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
        // ������ �´� ����
        rigid.AddForce(new Vector2(dir, 1) * 3, ForceMode2D.Impulse);
        Debug.Log(dir);
        if (!isplayerchecking)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }
}