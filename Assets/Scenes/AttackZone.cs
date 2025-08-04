using UnityEngine;
using UnityEngine.WSA;
using System.Collections;

public class AttackZone : MonoBehaviour
{
    public GameObject weapon;
    public GameObject player;
    Player_Platformer player_P;
    Collider2D col;
    bool isVisible;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        player_P = player.GetComponent<Player_Platformer>();
        col = GetComponent<Collider2D>();
    }

    private void Start()
    {
        isVisible = false;
    }
    IEnumerator DisableColliderTemporarily()
    {
        col.enabled = false;
        yield return new WaitForSeconds(0.3f);
        col.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        weapon.SetActive(isVisible);

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

        if (Input.GetKeyDown(KeyCode.Q))
        {
            gameObject.SetActive(true);
        }


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
        if (collision.gameObject.layer == 6 && Input.GetKey(KeyCode.Q)) // ���� �±� Ȯ��
        {
            //col.enabled = false;
            StartCoroutine(DisableColliderTemporarily());
            Debug.Log("���� �ǰݵ�: " + collision.gameObject.name);
        }
        /*else
        {
            col.enabled = true;
        }*/

    }
}
