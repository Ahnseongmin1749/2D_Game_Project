using UnityEngine;
using UnityEngine.Tilemaps;

public class Portal : MonoBehaviour
{
    public int portal_id;
    public MapManager mapManager;
    bool tryPortal;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
            Debug.Log(currentmap);
            Debug.Log(targetmap);
            currentmap.SetActive(false);
            targetmap.SetActive(true);

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
