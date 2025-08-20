using System.Collections.Generic;
using UnityEngine;

public class BossMapManager : MonoBehaviour
{
    Dictionary<int, GameObject[]> PortalData;
    public GameObject[] map_arr;
    public GameObject[] portal_arr;

    public GameObject player;
    Player_State state;
    Player_Platformer platformer;

    /*public GameManager GM;
    GameManager gameManager;*/


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PortalData = new Dictionary<int, GameObject[]>();
        MapListAdd();

        state = player.GetComponent<Player_State>();
        platformer = player.GetComponent<Player_Platformer>();

        /*gameManager = GM.GetComponent<GameManager>();*/

        CheckCurrentMap();
    }

    void MapListAdd()
    {
        PortalData.Add(1, new GameObject[] { map_arr[0], map_arr[1], portal_arr[1] });
        PortalData.Add(2, new GameObject[] { map_arr[1], map_arr[0], portal_arr[0] });
    }

    public GameObject GetCurrentMap(int id)
    {
        return PortalData[id][0];
    }
    public GameObject GetTargetMap(int id)
    {

        return PortalData[id][1];
    }
    public GameObject GetTargetMapPortal(int id)
    {
        return PortalData[id][2];
    }



    // Update is called once per frame
    void Update()
    {

    }

    public void CheckCurrentMap()
    {
        foreach (var map in map_arr)
        {
            if (map.activeSelf)
            {

                if (map.tag == "Village")
                {
                    state.SwitchToTopDown();
                }
                else if (map.tag == "Wild")
                {
                    state.SwitchToPlatformer();
                }

            }
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            platformer.rigid.linearVelocity = Vector2.zero;
            collision.transform.position = platformer.last_vec;
            GameManager.Instance.Hp = GameManager.Instance.Hp * 0.5f;
        }
    }
}
