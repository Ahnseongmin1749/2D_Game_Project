using System.Collections.Generic;
using UnityEngine;

public class Weapon_Manager : MonoBehaviour
{
    Dictionary<GameObject, int> weapon_strength;
    public GameObject[] weapons;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        weapon_strength = new Dictionary<GameObject, int>();
        Weapon_Add();
    }

    void Weapon_Add()
    {
        weapon_strength.Add(weapons[0], 1);
        weapon_strength.Add(weapons[1], 10);
    }

    void Start()
    {
        
    }

    public float Get_Weapon(int weapon_index)
    {
        return weapon_strength[weapons[weapon_index]];
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
