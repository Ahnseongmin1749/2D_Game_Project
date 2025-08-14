using UnityEngine;
using UnityEngine.UIElements;

public class MonsterBase : MonoBehaviour
{
    public float monster_atk;
    /*public GameObject player;
    Player_State state;*/

    /*private void Awake()
    {
        state = player.GetComponent<Player_State>();
    }*/

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public virtual void SlimeDamage(int amount)
    {
        //Debug.Log($"{gameObject.name} 기본 데미지 {amount} 받음");
    }

    public virtual void GoblinDamage(int amount)
    {
        //Debug.Log($"{gameObject.name} 기본 데미지 {amount} 받음");
    }

    /*public virtual void Die_Effect_Goblin()
    {
        state.hp += 10;
    }*/
}
