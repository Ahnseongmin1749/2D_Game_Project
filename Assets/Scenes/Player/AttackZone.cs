using UnityEngine;
using UnityEngine.WSA;
using System.Collections;

public class AttackZone : MonoBehaviour
{
    public GameObject swordPrefab;
    public GameObject weapon;
    public GameObject player;
    Player_Platformer player_P;
    Player_State player_State;
    public GameObject monsterManagerObject;
    MonsterManager monster_Manager;
    Collider2D col;
    bool isVisible;
    bool isAttack;
    bool rightAttack;
    bool tryAttack;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        player_P = player.GetComponent<Player_Platformer>();
        player_State = player.GetComponent<Player_State>();
        col = GetComponent<Collider2D>();
        monster_Manager = monsterManagerObject.GetComponent<MonsterManager>();
        isAttack = false;
    }

    private void Start()
    {
    }
    IEnumerator DisableColliderTemporarily()
    {
        col.enabled = false;
        yield return new WaitForSeconds(0.5f);
        col.enabled = true;
    }

    IEnumerator FalseAttack()
    {
        isAttack = true;
        yield return new WaitForSeconds(0.5f);
        tryAttack = false;
        isAttack = false;
    }

    IEnumerator LongDistanceAttack(Vector3 Mousevec)
    {
        isAttack = true;
        Vector3 firePoint = player.transform.position;
        // źȯ ���� & �߻�
        GameObject bullet = Instantiate(swordPrefab, firePoint, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Mousevec * 10;


        yield return new WaitForSeconds(1f); // 1�� ��� �� ���� �߻�
        isAttack = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f; // 2D �����̸� z�� 0���� ����
        Vector3 relativeMousePos = mouseWorldPos - Camera.main.transform.position;
        Vector3 shootDir = (mouseWorldPos - player.transform.position).normalized;


        Vector3 vec = Vector3.zero;
        if (relativeMousePos.x > 0)
        {
            vec = new Vector3(1.5f, 0, 0);
            rightAttack = true;
        }
        else if (relativeMousePos.x < 0)
        {
            vec = new Vector3(-1.5f, 0, 0);
            rightAttack = false;
        }
        transform.position = player.transform.position + vec;

        /*if (Input.GetKeyDown(KeyCode.Q))
        {
            gameObject.SetActive(true);
        }*/

        if (Input.GetMouseButtonDown(0) && !isAttack)
        {
            tryAttack = true;

            StartCoroutine(FalseAttack());
        }

        if(Input.GetMouseButtonUp(1) && !isAttack)
        {
            StartCoroutine(LongDistanceAttack(shootDir));
        }


        player_P.anim.SetBool("isAttack", isAttack);
        player_P.anim.SetBool("rightAttack", rightAttack);



    }

    /*public void Activate()
    {
        // �ݶ��̴� ��Ȱ��ȭ
        col.enabled = false;
        Invoke("Deactivate", 0.2f);
    }

    public void Deactivate()
    {
        // �ݶ��̴� Ȱ��ȭ
        col.enabled = true;
        gameObject.SetActive(false);
    }*/

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6 && tryAttack) // ���� �±� Ȯ��
        {
            tryAttack = false;

            StartCoroutine(DisableColliderTemporarily());


            int xKnockback = col.transform.position.x - player.transform.position.x > 0 ? 1 : -1;

            /*Slime slime = collision.GetComponent<Slime>();
            slime.OnDamaged(xKnockback);
            if (!slime.isplayerchecking)
            {
                slime.seeright = !slime.seeright;
            }*/

            //Debug.Log(collision);

            monster_Manager.GetWhoMonster(collision, xKnockback);

            /*MonsterBase mb = col.GetComponent<MonsterBase>();
            if (mb != null)
            {
                Debug.Log("Check");
                mb.TakeDamage(xKnockback);
            }*/

        }
        else if (collision.gameObject.layer == 8 && tryAttack)
        {
            Debug.Log("�и�!");
            //Transform parryingTarget = collision.gameObject.GetComponentInParent<Transform>();

            /*Vector3 vec = parryingTarget.position;
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            rb.linearVelocity = vec;*/

            // ���� ���� (�θ� ������ ������)
            Vector3 dir = (collision.transform.parent.position - collision.transform.position).normalized;

            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

            collision.gameObject.layer = 9;

            float reflectSpeed = rb.linearVelocity.magnitude; // ���� �Ѿ� �ӵ� ũ�� ����
            if (reflectSpeed < 1f) reflectSpeed = 5f; // �ּ� �ӵ� ����

            rb.linearVelocity = dir * reflectSpeed;

        }
        /*else
        {
            col.enabled = true;
        }*/

    }
}
