using UnityEngine;

public class MonsterBase : MonoBehaviour
{
    public virtual void SlimeDamage(int amount)
    {
        //Debug.Log($"{gameObject.name} 기본 데미지 {amount} 받음");
    }
}
