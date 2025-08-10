using System.Threading;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public GameObject[] monsters;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void GetWhoMonster(Collider2D col, int dir)
    {
        /*foreach (var monster in Monsters)
        {
            if (monster.tag == col.gameObject.tag)
            {
                MonsterBase mb = monster.GetComponentInChildren<MonsterBase>();
                mb.TakeDamage(dir);
            }
            else
            {
                Debug.Log("testDebug");
            }
        }*/

        /*if (col.gameObject.tag == "Slime")
        {
            Slime slime = col.GetComponent<Slime>();
            slime.TakeDamage(dir);
        }*/

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
    }


// Update is called once per frame
void Update()
    {
        
    }
}
