using UnityEngine;

public class Slimevision : MonoBehaviour
{
    public GameObject Slime;
    Slime slime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        slime = Slime.GetComponent<Slime>();
    }
    void Start()
    {

        


    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vec = slime.seeright ? new Vector3(5, 0, 0) : new Vector3(-5, 0, 0);
        transform.position = vec + Slime.transform.position;

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.layer == 3)
        {
            if (!slime.isplayerchecking)
            {
                slime.isplayerchecking = true;            // ★ 먼저 true로 설정
                CancelInvoke("NextMoveSelect");
                Invoke("NextMoveSelect", 0.5f);     // 이제 바로 추격으로 진입함
                Debug.Log("check");
            }
            else
            {
                slime.isplayerchecking = true;
            }
        }
        else
        {
            slime.isplayerchecking = false;
        }
    }
}
