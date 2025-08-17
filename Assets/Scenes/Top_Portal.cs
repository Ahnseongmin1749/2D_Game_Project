using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Top_Portal : MonoBehaviour
{
    bool tryPortal;
    int sceneIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator TryReset()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("check");
        tryPortal = false;
    }
    private void Awake()
    {
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        // æ¿¿« ¿Œµ¶Ω∫
        sceneIndex = currentScene.buildIndex;

        // æ¿ ¿Ã∏ß
        string sceneName = currentScene.name;

        Debug.Log("«ˆ¿Á æ¿ ¿Œµ¶Ω∫: " + sceneIndex);
        Debug.Log("«ˆ¿Á æ¿ ¿Ã∏ß: " + sceneName);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interaction"))
        {
            tryPortal = true;
            StartCoroutine(TryReset());
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && tryPortal)
        {
            tryPortal = false;

            if (sceneIndex == 0)
            {
                SceneManager.LoadScene(1);
            }
            if (sceneIndex == 1)
            {
                SceneManager.LoadScene(0);
                collision.gameObject.transform.position = new Vector3(0,0,0);
                Player_State state = collision.GetComponent<Player_State>();
                state.SwitchToTopDown();
            }
        }
    }
}
