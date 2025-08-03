using UnityEngine;
using UnityEngine.Tilemaps;

public class Portal : MonoBehaviour
{
    public int portal_id;
    public MapManager mapManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (Input.GetButtonDown("Jump"))
            {
                /*Debug.Log("c2");
                string presentmap = collision.transform.parent.name;
                string targetmap = mapManager.GetMapList(portal_id);
                GameObject P_map = FindMapObjectByName(presentmap);
                GameObject T_map = FindMapObjectByName(targetmap);
                Debug.Log(presentmap);
                Debug.Log(targetmap);
                P_map.SetActive(false);
                T_map.SetActive(true);*/

                Debug.Log("c2");
                GameObject currentmap = mapManager.GetCurrentMap(portal_id);
                GameObject targetmap = mapManager.GetTargetMap(portal_id);
                Debug.Log(currentmap);
                Debug.Log(targetmap);
                currentmap.SetActive(false);
                targetmap.SetActive(true);
            }


        }
    }

    GameObject FindMapObjectByName(string name)
    {
        foreach (GameObject map in mapManager.map_arr)
        {
            if (map.name == name)
                return map;
        }
        return null;
    }
}
