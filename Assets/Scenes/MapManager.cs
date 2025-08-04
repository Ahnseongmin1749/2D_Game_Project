using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    Dictionary<int, string> mapList;
    public GameObject[] map_arr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mapList = new Dictionary<int, string>();
        MapListAdd();
    }

    void MapListAdd()
    {
        mapList.Add(1, "WildMap");
        mapList.Add(2, "Village");
    }

    public string GetMapList(int id)
    {
        return mapList[id];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
