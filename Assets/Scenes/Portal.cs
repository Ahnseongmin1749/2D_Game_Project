using UnityEngine;
using UnityEngine.Tilemaps;

public class Portal : MonoBehaviour
{
    public int portal_id;
    public MapManager mapManager;
    bool tryPortal;

    public GameObject player;
    public Animator animator;
    public RuntimeAnimatorController topDownAnimator;
    public RuntimeAnimatorController platformerAnimator;
    Player_State state;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = player.GetComponent<Player_State>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interaction"))
        {
            tryPortal = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && tryPortal)
        {
            tryPortal = false;
            GameObject currentmap = mapManager.GetCurrentMap(portal_id);
            GameObject targetmap = mapManager.GetTargetMap(portal_id);
            currentmap.SetActive(false);
            targetmap.SetActive(true);

            if (portal_id == 1)
            {
                SwitchToPlatformer();
            }
            else if (portal_id == 2)
            {
                SwitchToTopDown();
            }

        }
    }

    // 포탈 스크립트 예시
    void SwitchToTopDown()
    {
        animator.runtimeAnimatorController = topDownAnimator;
        player.GetComponent<Player>().enabled = true;
        player.GetComponent<Player_Platformer>().enabled = false;
        state.isTopdown = true;
        state.RigidSetting();
        state.AttackZoneSetting();
    }

    void SwitchToPlatformer()
    {
        animator.runtimeAnimatorController = platformerAnimator;
        player.GetComponent<Player>().enabled = false;
        player.GetComponent<Player_Platformer>().enabled = true;
        state.isTopdown = false;
        state.RigidSetting();
        state.AttackZoneSetting();
    }
}
