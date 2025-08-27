using System.Threading;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public GameObject[] monsters;

    public GameObject monsterPrefab; // ���� ������
    public Transform[] spawnPoints;  // ���� ��ġ��
    public float spawnInterval = 5f; // �� �ʸ��� ��������



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //InvokeRepeating(nameof(SpawnMonster), 0f, spawnInterval);
    }

    /*void SpawnMonster()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];

        Instantiate(monsterPrefab, spawnPoint.position, spawnPoint.rotation);
    }*/

    public void GetWhoMonster(Collider2D col, int dir)
    {

        if (col.gameObject.tag == "Slime")
        {
            MonsterBase mb = col.GetComponent<MonsterBase>();
            mb.SlimeDamage(dir);
        }
        else if (col.gameObject.tag == "Goblin")
        {
            MonsterBase mb = col.GetComponent<MonsterBase>();
            mb.GoblinDamage(dir);
        }
        else if (col.gameObject.tag == "Boss")
        {
            MonsterBase mb = col.GetComponent<MonsterBase>();
            mb.BossDamage(dir);
        }
    }


// Update is called once per frame
void Update()
    {
        
    }
}
