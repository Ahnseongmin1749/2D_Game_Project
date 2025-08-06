using UnityEngine;

public class MonsterBase : MonoBehaviour
{
    public GameObject healthBarPrefab; // 이걸 유니티에서 연결해줘 (MonsterHealthBar 프리팹)
    Transform healthBar;

    void Start()
    {
        // 프리팹 복사해서 생성
        GameObject bar = Instantiate(healthBarPrefab);

        // 부모를 이 몬스터로 설정 (머리 위에 따라오게 함)
        bar.transform.SetParent(transform);

        // 머리 위에 위치 조정 (y를 조금 올림)
        bar.transform.localPosition = new Vector3(0, 1, 0); // 몬스터 크기에 따라 조절

        // 카메라 쪽을 보게 하려면 따로 LookAt 처리 필요
        healthBar = bar.transform;
    }

    void Update()
    {
        // 예시: 체력 줄어들 때 fillAmount 조절
        // bar.transform.GetChild(0)는 HPFill Image
        float hpRatio = 0.5f; // 50%
        healthBar.GetChild(0).GetComponent<UnityEngine.UI.Image>().fillAmount = hpRatio;
    }
    public virtual void SlimeDamage(int amount)
    {
        //Debug.Log($"{gameObject.name} 기본 데미지 {amount} 받음");
    }
}
