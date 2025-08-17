using UnityEngine;
using UnityEngine.WSA;
using System.Collections;

public class AttackZone : MonoBehaviour
{
    public GameObject weapon;
    public GameObject player;
    Player_Platformer player_P;
    Player_State player_State;
    public GameObject monsterManagerObject;
    MonsterManager monster_Manager;
    Collider2D col;
    bool isVisible;
    bool isAttack;
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

    // Update is called once per frame
    void Update()
    {

        

        Vector3 vec;
        if (player_P.isrightlooking)
        {
            vec = new Vector3(1.5f, 0, 0);
        }
        else
        {
            vec = new Vector3(-1.5f, 0, 0);
        }
        transform.position = player.transform.position + vec;

        /*if (Input.GetKeyDown(KeyCode.Q))
        {
            gameObject.SetActive(true);
        }*/

        if (Input.GetKeyDown(KeyCode.Q) && !isAttack)
        {
            tryAttack = true;

            StartCoroutine(FalseAttack());
        }

        player_P.anim.SetBool("isAttack", isAttack);

    }

    /*public void Activate()
    {
        // 콜라이더 비활성화
        col.enabled = false;
        Invoke("Deactivate", 0.2f);
    }

    public void Deactivate()
    {
        // 콜라이더 활성화
        col.enabled = true;
        gameObject.SetActive(false);
    }*/

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6 && tryAttack) // 몬스터 태그 확인
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
        /*else
        {
            col.enabled = true;
        }*/

    }
}
