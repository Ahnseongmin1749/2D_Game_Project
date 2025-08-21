using UnityEngine;

public class Bullet : MonoBehaviour
{
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
        else if (collision != null && collision.gameObject.layer == 3)
        {
            Debug.Log("Hit!");
        }
    }
}