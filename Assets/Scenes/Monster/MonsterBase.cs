using UnityEngine;
using UnityEngine.UIElements;

public class MonsterBase : MonoBehaviour
{
    public float monster_atk;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public virtual void SlimeDamage(int amount)
    {
        //Debug.Log($"{gameObject.name} �⺻ ������ {amount} ����");
    }

    public virtual void GoblinDamage(int amount)
    {
        //Debug.Log($"{gameObject.name} �⺻ ������ {amount} ����");
    }
}
