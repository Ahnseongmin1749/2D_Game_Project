using UnityEngine;

public class MonsterBase : MonoBehaviour
{
    public GameObject healthBarPrefab; // �̰� ����Ƽ���� �������� (MonsterHealthBar ������)
    Transform healthBar;

    void Start()
    {
        // ������ �����ؼ� ����
        GameObject bar = Instantiate(healthBarPrefab);

        // �θ� �� ���ͷ� ���� (�Ӹ� ���� ������� ��)
        bar.transform.SetParent(transform);

        // �Ӹ� ���� ��ġ ���� (y�� ���� �ø�)
        bar.transform.localPosition = new Vector3(0, 1, 0); // ���� ũ�⿡ ���� ����

        // ī�޶� ���� ���� �Ϸ��� ���� LookAt ó�� �ʿ�
        healthBar = bar.transform;
    }

    void Update()
    {
        // ����: ü�� �پ�� �� fillAmount ����
        // bar.transform.GetChild(0)�� HPFill Image
        float hpRatio = 0.5f; // 50%
        healthBar.GetChild(0).GetComponent<UnityEngine.UI.Image>().fillAmount = hpRatio;
    }
    public virtual void SlimeDamage(int amount)
    {
        //Debug.Log($"{gameObject.name} �⺻ ������ {amount} ����");
    }
}
