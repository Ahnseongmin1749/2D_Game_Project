using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab; // Inspector���� ������ ���
    public GameObject uiPrefab;     // Inspector���� ������ ���
    private GameObject currentPlayer;
    private GameObject currentUI;

    void Awake()
    {
        // �� �ε� �Ϸ� �̺�Ʈ ����
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // �̺�Ʈ ���� (�޸� ���� ����)
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SpawnPlayerAndUI();
    }

    private void SpawnPlayerAndUI()
    {
        // Player ���� ��ġ ã��
        Transform spawnPoint = GameObject.FindWithTag("SpawnPoint")?.transform;
        if (spawnPoint == null)
        {
            Debug.LogWarning("SpawnPoint�� ���� ����! (Player�� Vector3.zero�� ������)");
            spawnPoint = new GameObject("TempSpawn").transform;
            spawnPoint.position = Vector3.zero;
        }

        // ���� �÷��̾� ����
        if (currentPlayer != null) Destroy(currentPlayer);
        // �� �÷��̾� ����
        currentPlayer = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);

        // ���� UI ����
        if (currentUI != null) Destroy(currentUI);
        // �� UI ����
        currentUI = Instantiate(uiPrefab);
    }
}