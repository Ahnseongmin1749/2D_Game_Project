using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour
{
    public GameObject player;

    public GameObject frontLaser;
    public GameObject earthquake;

    public GameObject bulletPrefab;

    Vector2 playervec;
    Vector2 Laservec;
    Vector2 Earthquakevec;

    Rigidbody2D rigid;
    float boss_hp;
    float boss_atk;
    float boss_def;
    float boss_speed = 5;

    float bulletCount;

    bool boss_jumping;

    IEnumerator ActiveAttackEffect(string effect)
    {

        switch (effect) {
            case "laser":
                {
                    frontLaser.SetActive(true);
                    yield return new WaitForSeconds(2f);
                    frontLaser.SetActive(false);
                    break;
                }
            case "earthquake":
                {
                    earthquake.SetActive(true);
                    yield return new WaitForSeconds(1f);
                    earthquake.SetActive(false);
                    break;
                }
        }

    }


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Invoke("JumpAttack", 3);
        //Invoke("Rush", 8);
        //Invoke("Laser", 10);
        Invoke("Bullet", 3);
    }

    // Update is called once per frame
    void Update()
    {
        playervec = (player.transform.position.x - gameObject.transform.position.x) > 0 ?
            Vector2.right : Vector2.left;


        Laservec = (player.transform.position.x - gameObject.transform.position.x) > 0 ?
            gameObject.transform.position + new Vector3(10, 0, 0) : transform.position + new Vector3(-10, 0, 0);

        Earthquakevec = gameObject.transform.position + new Vector3(0, -2f, 0);

        frontLaser.transform.position = Laservec;
        earthquake.transform.position = Earthquakevec;

    }

    private void FixedUpdate()
    {
        if (rigid.linearVelocity.y < 0.5f && boss_jumping)
        {
            rigid.AddForce(new Vector2(0,-10), ForceMode2D.Impulse);
        }

        if (Mathf.Abs(rigid.linearVelocity.x) < 0.3f)
        {
            //rigid.AddForce(new Vector2(playervec.x * 50, 1), ForceMode2D.Impulse);
            rigid.linearVelocity = new Vector2(0, 0);
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
                if (boss_jumping == true)
                {
                    Debug.Log("!!!");
                    StartCoroutine(ActiveAttackEffect("earthquake"));
                }
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
        /*Vector2 playervec = (player.transform.position.x - gameObject.transform.position.x) > 0 ?
            Vector2.right : Vector2.left;*/

        //rigid.AddForce(new Vector2 (playervec.x * 3,15), ForceMode2D.Impulse);
        rigid.linearVelocity = new Vector2(playervec.x * 3, 15);



    }

    void Rush()
    {
        /*Vector2 playervec = (player.transform.position.x - gameObject.transform.position.x) > 0 ?
            Vector2.right : Vector2.left;*/

        //if (Mathf.Abs(rigid.linearVelocity.x) < 0.3f)
        if (Mathf.Abs(rigid.linearVelocity.x) == 0)
        {
            //rigid.AddForce(new Vector2(playervec.x * 50, 1), ForceMode2D.Impulse);
            rigid.linearVelocity = new Vector2(playervec.x * 20, 1);
        }
    }

    void Laser()
    {
        StartCoroutine(ActiveAttackEffect("laser"));
    }

    void Bullet()
    {
        StartCoroutine(BulletAttack());
        if (bulletCount <= 3)
        {
            Invoke("Bullet", 1);
        }
    }

    IEnumerator BulletAttack()
    {
        // 1. 특정 위치로 이동
        Vector2 targetPos = new Vector2(-16, 5); // 보스가 이동할 위치 (예시)
        if ((Vector2)transform.position != targetPos)
        {
            //transform.position = Vector2.MoveTowards(transform.position, targetPos, boss_speed * Time.deltaTime);
            transform.position = targetPos;
            yield return null;
        }
        Vector3 firePoint = transform.position + new Vector3(0, 1, 0);
        // 2. 플레이어 방향 계산
        Vector2 dir = (player.transform.position - firePoint).normalized;

        // 3. 탄환 생성 & 발사
        GameObject bullet = Instantiate(bulletPrefab, firePoint, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = dir * 10;

        bulletCount++;

        
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null && collision.gameObject.layer == 3)
        {
            Debug.Log("Hit!");
        }
    }
}
