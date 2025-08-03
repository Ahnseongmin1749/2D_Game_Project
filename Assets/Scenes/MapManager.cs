using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    Dictionary<int, GameObject[]> PortalData;
    public GameObject[] map_arr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PortalData = new Dictionary<int, GameObject[]>();
        MapListAdd();
    }

    void MapListAdd()
    {
        PortalData.Add(1, new GameObject[] { map_arr[0], map_arr[1] });
        PortalData.Add(2, new GameObject[] { map_arr[1], map_arr[0] });
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
}
