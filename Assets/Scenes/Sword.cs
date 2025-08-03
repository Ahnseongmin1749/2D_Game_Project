using UnityEngine;

public class Sword : MonoBehaviour
{
    public GameObject sworld;
    public GameObject player;
    bool isVisible;
    SpriteRenderer spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        isVisible = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        spriteRenderer.color = new Color(0, 0, 0, 0);
        /*Vector2 vec = player.GetComponent < Player_P >
        transform.position = player.transform.position + vec;*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
