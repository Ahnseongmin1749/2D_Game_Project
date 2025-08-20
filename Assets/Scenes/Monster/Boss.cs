using UnityEngine;

public class Boss : MonoBehaviour
{
    public GameObject player;

    Rigidbody2D rigid;
    float boss_hp;
    float boss_atk;
    float boss_def;

    bool boss_jumping;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("JumpAttack", 3);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(boss_jumping);
    }

    private void FixedUpdate()
    {
        if (rigid.linearVelocity.y < 0.5f)
        {
            rigid.AddForce(new Vector2(0,-10), ForceMode2D.Impulse);
            Debug.Log("Check");
        }

        JumpCheckFunc();
    }

    void JumpCheckFunc()
    {
        Debug.DrawRay(transform.position, Vector2.down * 3, new Color(1, 0, 0, 0.7f));

        RaycastHit2D downray = Physics2D.Raycast(transform.position, Vector2.down,
            3, LayerMask.GetMask("Platform"));
        if (downray.collider != null)
        {
            if (downray.collider.gameObject.layer == 10)
            {
                boss_jumping = false;
            }
        }
        else
        {
            boss_jumping = true;
        }
    }

    void JumpAttack()
    {
        Vector2 playervec = (player.transform.position.x - gameObject.transform.position.x) > 0 ?
            Vector2.right : Vector2.left;

        rigid.AddForce(new Vector2 (playervec.x * 3,15), ForceMode2D.Impulse);

        
        
    }

    void Rush()
    {

    }

    void Laser()
    {

    }

}
