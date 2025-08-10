using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    Dictionary<int, GameObject[]> PortalData;
    public GameObject[] map_arr;

    public GameObject player;
    public Animator animator;
    public RuntimeAnimatorController topDownAnimator;
    public RuntimeAnimatorController platformerAnimator;
    Player_State state;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PortalData = new Dictionary<int, GameObject[]>();
        MapListAdd();

        state = player.GetComponent<Player_State>();

        CheckCurrentMap();
        
    }

    void MapListAdd()
    {
        PortalData.Add(1, new GameObject[] { map_arr[0], map_arr[1] });
        PortalData.Add(2, new GameObject[] { map_arr[1], map_arr[0] });

        PortalData.Add(3, new GameObject[] { map_arr[1], map_arr[2] });
        PortalData.Add(4, new GameObject[] { map_arr[2], map_arr[1] });
    }

    public GameObject GetCurrentMap(int id)
    {
        return PortalData[id][0];
    }
    public GameObject GetTargetMap(int id)
    {
        
        return PortalData[id][1];
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
                Debug.Log(map);

                if (map.tag == "Village")
                {
                    SwitchToTopDown();
                }
                else if(map.tag == "Wild")
                {
                    SwitchToPlatformer();
                }

            }
        }

    }

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
