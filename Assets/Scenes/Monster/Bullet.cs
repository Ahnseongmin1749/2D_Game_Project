using UnityEngine;

public class Bullet : MonoBehaviour
{
    //Player_Platformer playerplatformer_cs;
    public float maxDistance = 20f; // 최대 이동 거리
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (Vector3.Distance(startPos, transform.position) > maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.layer == 10)
        {
            Destroy(gameObject);
        }
        else if (collision != null && collision.gameObject.layer == 3 && !(gameObject.layer == 9))
        {
            Debug.Log("Hit!");
            Boss boss_cs = gameObject.GetComponentInParent<Boss>();
            boss_cs.Player_Attack();
            //GameManager.Instance.Hp -= 10;
            //playerplatformer_cs.OnDamaged(playervec);
        }
        else if (collision != null && collision.gameObject.layer == 6)
        {
            Debug.Log("parring hit!");

            MonsterBase mb = collision.gameObject.GetComponent<MonsterBase>();
            mb.BossDamage(10);

            Destroy(gameObject);
        }
    }
}