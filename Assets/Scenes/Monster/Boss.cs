using UnityEngine;
using System.Collections;

public class Boss : MonsterBase
{
    public GameObject player;
    public GameObject frontLaser;
    public GameObject earthquake;
    public GameObject bulletPrefab;
    Player_Platformer playerplatformer_cs;

    Vector2 playervec;
    Vector2 Laservec;
    Vector2 Earthquakevec;

    Rigidbody2D rigid;
    public float boss_hp;
    float boss_atk;
    float boss_def;
    float boss_speed = 5;
    CapsuleCollider2D capsuleCollider;
    SpriteRenderer spriteRenderer;

    public Transform BossHPBar;

    float bulletCount;

    bool boss_jumping;
    bool isLending;
    bool isJumpAttack;

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
                    yield return new WaitForSeconds(0.5f);
                    earthquake.SetActive(false);
                    break;
                }
        }

    }


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerplatformer_cs = player.GetComponent<Player_Platformer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Invoke("JumpAttack", 3);
        //Invoke("Rush", 8);
        //Invoke("Laser", 10);
        //Invoke("Bullet", 3);
        Invoke("Bullet", 1);
    }

    void NextPattern()
    {
        isJumpAttack = false;
        int next_pattern_index = Random.Range(0, 4);

        Debug.Log(next_pattern_index);

        if (next_pattern_index == 0)
        {
            Rush();
        }
        else if (next_pattern_index == 1)
        {
            Laser();
        }
        else if (next_pattern_index == 2)
        {
            Bullet();
        }
        else if (next_pattern_index == 3)
        {
            JumpAttack();
        }

        
        
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


        float hpRatio = boss_hp / 100f;
        BossHPBar.GetComponent<UnityEngine.UI.Image>().fillAmount = hpRatio;
    }


    private void FixedUpdate()
    {


        /*if (Mathf.Abs(rigid.linearVelocity.x) < 0.3f)
        {
            rigid.linearVelocity = new Vector2(0, 0);
            rigid.gravityScale = 0;
        }*/

        if (rigid.linearVelocity.y < 0.5f && boss_jumping && isJumpAttack)
        {
            rigid.AddForce(new Vector2(0, -10), ForceMode2D.Impulse);
            Debug.Log("운");
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
                    StartCoroutine(ActiveAttackEffect("earthquake"));
                    isLending = true;
                }
                
                boss_jumping = false;
                isLending = false;
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
        isJumpAttack = true;
        rigid.linearVelocity = new Vector2(playervec.x * 3, 15);
        Invoke("NextPattern", 5);
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
        Invoke("NextPattern", 2);
    }

    void Laser()
    {
        StartCoroutine(ActiveAttackEffect("laser"));
        Invoke("NextPattern", 2);
    }

    void Bullet()
    {
        StartCoroutine(BulletAttack());
    }

    IEnumerator BulletAttack()
    {
        rigid.gravityScale = 0;
        // 1. 특정 위치로 이동
        Vector2 targetPos = new Vector2(-16, 5); // 보스가 이동할 위치 (예시)
        while ((Vector2)transform.position != targetPos)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, boss_speed * Time.deltaTime);
            //transform.position = targetPos;
            yield return null;
        }

        for (int i = 0; i < 3; i++)
        {
            Vector3 firePoint = transform.position + new Vector3(0, 1, 0);

            // 플레이어 방향 계산
            Vector2 dir = (player.transform.position - firePoint).normalized;

            // 탄환 생성 & 발사
            GameObject bullet = Instantiate(bulletPrefab, firePoint, Quaternion.identity,transform);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.linearVelocity = dir * 10;

            yield return new WaitForSeconds(1f); // 1초 대기 후 다음 발사
        }

        Invoke("NextPattern", 5);
        rigid.gravityScale = 1;

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null && collision.gameObject.layer == 3)
        {
            Debug.Log("몸통박치기");
            Player_Attack();
        }
    }

    public override void BossDamage(int dir)
    {
        boss_hp -= GameManager.Instance.atk;


        if (boss_hp <= 0)
        {
            Die_Effect_Boss();

            GameManager.Instance.total_exp += 10;
        }
    }

    void Die_Effect_Boss()
    {
        spriteRenderer.color = new Color(0.78f, 0.78f, 0.78f);
        spriteRenderer.flipY = true;
        capsuleCollider.enabled = false;

        Invoke("Disappear_Boss", 3);
    }

    void Disappear_Boss()
    {
        capsuleCollider.enabled = false;
        gameObject.SetActive(false);
    }

    public void Player_Attack()
    {
        GameManager.Instance.Hp -= 10;
        playerplatformer_cs.OnDamaged(playervec);
    }
}
