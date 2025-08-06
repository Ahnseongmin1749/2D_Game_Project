using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public GameObject[] Monsters;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void GetWhoMonster(Collider2D col)
    {
        foreach (var monster in Monsters)
        {
            if (monster == col.gameObject)
            {
                /*Debug.Log(monster.name);
                monster.name monster = col.GetComponent<monster.name>();*/
            }
        }
    }

// Update is called once per frame
void Update()
    {
        
    }
}
