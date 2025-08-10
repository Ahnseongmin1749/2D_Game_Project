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
        //Debug.Log($"{gameObject.name} 기본 데미지 {amount} 받음");
    }

    public virtual void GoblinDamage(int amount)
    {
        //Debug.Log($"{gameObject.name} 기본 데미지 {amount} 받음");
    }
}
